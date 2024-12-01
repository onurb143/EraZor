using System.ComponentModel.DataAnnotations;

namespace EraZor.Models
{

    public class WipeMethod
    {
        [Key] // Denne linje fortæller EF, at MethodID er primærnøglen
        public int MethodID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OverwritePass { get; set; }
    }
}


/*
using System.ComponentModel.DataAnnotations;

namespace EraZor.Models
{
    public class WipeMethod
    {
        [Key] // Angiver primærnøglen
        public int MethodID { get; set; }

        [Required] // Gør navn obligatorisk
        public string Name { get; set; }

        public string Description { get; set; }

        // Antal overskrivninger. 1 for SSD'er, højere for HDD'er.
        public int OverwritePass { get; set; }

        // Kommando eller metode, der skal kaldes, hvis dette skal automatiseres.
        public string Command { get; set; }

        // Parameterfelt til fremtidige metoder
        public string Parameters { get; set; }
    }
}
*/