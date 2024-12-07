namespace EraZor.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public ICollection<WipeJob> WipeJobs { get; set; } = new List<WipeJob>();
    }
}

