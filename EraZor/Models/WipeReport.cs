﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace EraZor.Models
{
    public class WipeReport
    {
        [ForeignKey("WipeJobId")]
        public int WipeJobId { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Status { get; set; }
        public string? DiskType { get; set; }
        public int Capacity { get; set; }
        [Required] 
        public required string SerialNumber { get; set; }
        public string? Manufacturer { get; set; }
        public required string WipeMethodName { get; set; }
        public int OverwritePasses { get; set; }
        public required string performedBy { get; set; }

        [JsonIgnore]
        public virtual WipeJob WipeJob { get; set; }  // Navigation property

        // Fremmed nøgle til IdentityUser
        [ForeignKey("UserId")]
        public string? UserId { get; set; }

        // Navigation property til IdentityUser
        public virtual IdentityUser? User { get; set; }
    }
}
