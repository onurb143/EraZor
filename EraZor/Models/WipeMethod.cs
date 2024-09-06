using System.ComponentModel.DataAnnotations;

namespace EraZor.Models
{

    public class WipeMethod
    {
        [Key] // Denne linje fortæller EF, at MethodID er primærnøglen
        public int MethodID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OverwritePass { get; set; }
    }
}
