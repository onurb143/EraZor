namespace EraZor.Models
{
    public class LogEntry
    {
        public int LogId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public int JobId { get; set; }

        public WipeJob Job { get; set; }
    }
}
