using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EraZor.Models
{
    public class WipeReport
    {
        public int? WipeJobId { get; set; }  // Fremmed nøgle til WipeJob (nullable)
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Status { get; set; }
        public string? DiskType { get; set; }
        public int Capacity { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string WipeMethodName { get; set; }
        public int OverwritePasses { get; set; }

        [JsonIgnore]
        public virtual WipeJob WipeJob { get; set; }  // Navigation property
    }
}
