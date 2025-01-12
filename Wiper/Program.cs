#!/bin/bash

# Funktion: Log fejl og information til en logfil
using System.Numerics;
using System.Threading;

log_file = "/var/log/disk_wipe.log"
log() {
    echo "$(date +'%Y-%m-%d %H:%M:%S') - $1" | tee - a "$log_file"
}

# API URL'er
BASE_API = "https://192.168.32.15:5002/api"
AUTH_API = "$BASE_API/Auth/login"
DISK_API = "$BASE_API/disks"
WIPE_METHODS_API = "$BASE_API/wipeMethods"
WIPE_REPORT_API = "$BASE_API/wipeReports"

# Brugerlogin
echo "Indtast dine loginoplysninger for at fortsætte:"
read -p "E-mail: " email
read -s -p "Adgangskode: " password
echo

# Autentificer og hent JWT-token
log "Autentificerer og henter JWT-token..."
auth_response=$(curl -k -s -X POST "$AUTH_API" \
    -H "Content-Type: application/json" \
    -d "{\"email\":\"$email\",\"password\":\"$password\"}")

# Parse JWT-token
jwt_token=$(echo "$auth_response" | jq -r '.token')

if [[ -z "$jwt_token" || "$jwt_token" == "null" ]]; then
    log "Fejl: Kunne ikke hente JWT-token! Kontrollér loginoplysningerne."
    echo "Fejl: Kunne ikke hente JWT-token! Kontrollér loginoplysningerne."
    exit 1
fi

log "JWT-token hentet med succes."

# Find bootdisken
boot_disk=$(findmnt -n -o SOURCE / | sed 's/[0-9]*$//')
log "Bootdisken er identificeret som $boot_disk. Denne disk vil ikke blive vist som en mulighed for sletning."

# Find og vis tilgængelige diske undtagen bootdisken
log "Finder tilgængelige diske (ekskluderer bootdisken)..."
echo "Tilgængelige diske:"
lsblk -ndo NAME, SIZE, MODEL,SERIAL | grep -v "$(basename "$boot_disk")"

# Brugeren vælger en disk
read -p "Vælg disk (indtast diskens navn, f.eks. sdb): " selected_disk
disk_path="/dev/$selected_disk"

# Kontrollér, om disken findes og ikke er bootdisken
if [["$disk_path" == "$boot_disk"]]; then
    log "Valgt disk er bootdisken ($disk_path), og kan ikke slettes."
    echo "Du kan ikke slette bootdisken ($disk_path). Vælg en anden disk."
    exit 1
fi

if [[ ! -b "$disk_path" ]]; then
    log "Disken $disk_path blev ikke fundet."
    echo "Disken $disk_path blev ikke fundet. Kontrollér valget og prøv igen."
    exit 1
fi

# Prøv at aflæse oplysninger med smartctl
log "Aflæser diskoplysninger for $disk_path..."
smartctl_info=$(smartctl -i "$disk_path" || smartctl -d sat -i "$disk_path")
if [[ -z "$smartctl_info" ]]; then
    log "Kunne ikke aflæse diskoplysninger for $disk_path."
    echo "Kunne ikke aflæse diskoplysninger. Kontrollér disken."
    exit 1
fi
log "Diskoplysninger aflæst."

# Ekstraher diskoplysninger med fallback-værdier
serial_number=$(echo "$smartctl_info" | grep "Serial Number" | awk -F: '{print $2}' | xargs)
serial_number =${ serial_number: -"UNKNOWN"}

manufacturer =$(lsblk - no VENDOR "$disk_path" | xargs)
manufacturer=${manufacturer:-"UNKNOWN"}

capacity =$(lsblk - bno SIZE "$disk_path" | awk '{print $1 / 1024 / 1024 / 1024}' | cut -d. -f1)
disk_type=$(echo "$smartctl_info" | grep "Rotation Rate" | grep -q "Solid State" && echo "SSD" || echo "HDD")

# Vælg slettemetode
echo "Tilgængelige slettemetoder:"
wipe_methods=$(curl -k -s "$WIPE_METHODS_API" -H "Authorization: Bearer $jwt_token")
if [[ -z "$wipe_methods" ]]; then
    log "Kunne ikke hente slettemetoder fra API."
    echo "Kunne ikke hente slettemetoder fra API. Kontrollér forbindelsen."
    exit 1
fi
echo "$wipe_methods" | jq -r '.[] | "\(.wipeMethodID): \(.name) - \(.description)"'

read -p "Vælg slettemetode (indtast ID): " selected_method
method=$(echo "$wipe_methods" | jq -e ".[] | select(.wipeMethodID == $selected_method)")

method_name=$(echo "$method" | jq -r '.name')

read -p "Vil du slette disken $disk_path med $method_name? (y/n): " confirm
if [["$confirm" != "y"]]; then
    log "Sletning annulleret af brugeren."
    echo "Sletning annulleret."
    exit 0
fi

# Registrer starttidspunkt
start_time=$(date -u +'%Y-%m-%dT%H:%M:%SZ')
log "Sletning af $disk_path med $method_name startet kl. $start_time"

# Udfør sletning baseret på valgt metode
echo "Sletter data på $disk_path med metode $method_name..."
case $selected_method in
    1) hdparm --user-master u --security-set-pass p "$disk_path" && hdparm --user-master u --security-erase p "$disk_path" ; ;
2) dd if=/dev/zero of="$disk_path" bs=1M status=progress ; ;
3) dd if=/dev/urandom of="$disk_path" bs=1M status=progress ; ;
4) shred - n 35 - v "$disk_path"; ;
5) shred - n 3 - v "$disk_path"; ;
6) shred - n 1 - z - v "$disk_path"; ;
7) shred - n 7 - v "$disk_path"; ; # Schneier Method
    8) shred - n 3 - v "$disk_path"; ; # HMG IS5 (Enhanced)
    9) shred - n 35 - v "$disk_path"; ; # Peter Gutmann's Method
    10) dd if=/dev/zero of="$disk_path" bs=1M status=progress ; ; # Single Pass Zeroing
    11) shred - n 4 - v "$disk_path"; ; # DoD 5220.22-M (E)
    12) dd if=/dev/zero of="$disk_path" bs=1M status=progress ; ; # ISO/IEC 27040
    *) log "Slettemetoden er ikke implementeret."; exit 1 ; ;
esac

# Registrer sluttidspunkt
end_time =$(date -u +'%Y-%m-%dT%H:%M:%SZ')
log "Sletning fuldført for $disk_path med $method_name kl. $end_time"

# Generer og send sletterapport
wipe_report=$(jq -n \
    --arg startTime "$start_time" \
    --arg endTime "$end_time" \
    --arg status "Completed" \
    --arg diskType "$disk_type" \
    --argjson capacity "$capacity" \
    --arg serialNumber "$serial_number" \
    --arg manufacturer "$manufacturer" \
    --arg wipeMethodName "$method_name" \
    '{startTime: $startTime, endTime: $endTime, status: $status, diskType: $diskType, capacity: $capacity, serialNumber: $serialNumber, manufacturer: $manufacturer, wipeMethodName: $wipeMethodName}')

response=$(curl -sk -w "\n%{http_code}" -X POST "$WIPE_REPORT_API" -H "Content-Type: application/json" -H "Authorization: Bearer $jwt_token" -d "$wipe_report")

http_status=$(echo "$response" | tail -n1)
server_response=$(echo "$response" | head -n -1)

if [["$http_status" - ne 200 && "$http_status" - ne 201]]; then
    log "Fejl ved afsendelse af sletterapport. HTTP status: $http_status. Server response: $server_response"
    echo "Fejl ved afsendelse af sletterapport."
else
    log "Sletterapport sendt med succes. Server response: $server_response"
    echo "Sletterapport sendt med succes!"
fi

exit 0
