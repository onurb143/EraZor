using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EraZor.Model
{
    public class WipeReport
    {
        [ForeignKey("WipeJobId")]
        [Required]
        public int WipeJobId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Disk type cannot exceed 50 characters.")]
        public string DiskType { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "Serial number cannot exceed 18 characters.")]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(24, ErrorMessage = "Manufacturer cannot exceed 24 characters.")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Wipe method name cannot exceed 50 characters.")]
        public string WipeMethodName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Overwrite passes must be greater than 0.")]
        public int OverwritePasses { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Performed by cannot exceed 50 characters.")]
        public string PerformedBy { get; set; }

        [JsonIgnore]
        public virtual WipeJob WipeJob { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
