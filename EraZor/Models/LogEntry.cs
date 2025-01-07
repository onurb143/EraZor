﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EraZor.Models
{
    public class LogEntry
    {
        [Key] // Primær nøgle
        public int LogID { get; set; }

        public DateTime Timestamp { get; set; }

        [MaxLength(500)]
        public string? Message { get; set; }

        [ForeignKey("WipeJob")]
        public int WipeJobId { get; set; } // Fremmednøgle til WipeJob

        public required WipeJob WipeJob { get; set; }
    }
}



