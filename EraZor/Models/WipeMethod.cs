using System.ComponentModel.DataAnnotations;

namespace EraZor.Models
{

    public class WipeMethod
    {
        [Key] // Denne linje fortæller EF, at MethodID er primærnøglen
        public int WipeMethodID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OverwritePass { get; set; }

        public ICollection<WipeJob> WipeJobs { get; set; }
    }
}
