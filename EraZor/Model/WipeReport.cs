using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace EraZor.Model
{
    public class WipeReport
    {
        [ForeignKey("WipeJobId")]
        public int WipeJobId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string DiskType { get; set; }
        public int Capacity { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string WipeMethodName { get; set; }
        public int OverwritePasses { get; set; }
        public string PerformedBy { get; set; }  // PerformedBy refers to IdentityUser username

        [JsonIgnore]
        public virtual WipeJob WipeJob { get; set; }

        // ForeignKey to IdentityUser (PerformedBy)
        [ForeignKey("UserId")]
        public string UserId { get; set; }  // PerformedBy user ID

        public virtual IdentityUser User { get; set; }
    }

}
