using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EraZor.Models
{
    public class Disk
    {
        [Key]
        [Column("diskID")] // Matcher kolonnenavnet i PostgreSQL
        public int diskID { get; set; }

        // SSD, HDD, USB, osv.
        public string Type { get; set; }

        // Kapacitet i GB eller TB
        public int Capacity { get; set; }

        // Status: Active, Inactive, Locked, osv.
        public string Status { get; set; }

        // Sti til disken, f.eks. /dev/sda eller D:
        public string Path { get; set; }

        // Serienummer for at identificere disken unikt
        public string SerialNumber { get; set; }

        // Brugbar til logning: Modelnavn, producent osv.
        public string Manufacturer { get; set; }
    }
}
