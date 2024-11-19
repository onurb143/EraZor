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
        public string Status { get; set; }

        [Column("diskid")]
        public int DiskId { get; set; }

        [Column("methodid")]
        public int MethodId { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        // Navigations
        public Disk Disk { get; set; }
        public WipeMethod Method { get; set; }
        public User User { get; set; }
    }
}
