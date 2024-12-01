using System.ComponentModel.DataAnnotations.Schema;

namespace EraZor.Models
{
    [Table("wipejob")]
    public class WipeJob
    {
        [Column("wipejobid")]
        public int WipeJobId { get; set; }

        [Column("starttime")]
        public DateTime StartTime { get; set; }

        [Column("endtime")]
        public DateTime EndTime { get; set; }

        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("diskid")]
        public int DiskId { get; set; }

        [Column("methodid")]
        public int MethodId { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        // Navigations
        public Disk Disk { get; set; } = new Disk();
        public WipeMethod Method { get; set; } = new WipeMethod();
        public User User { get; set; } = new User();

    }
}
