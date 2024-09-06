using System.ComponentModel.DataAnnotations;

namespace EraZor.Models
{
    public class LogEntry
    {
        [Key] // Denne linje fortæller EF, at LogID er primærnøglen
        public int LogID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public int JobID { get; set; }

        // Navigation property
        public WipeJob WipeJob { get; set; }
    }
}