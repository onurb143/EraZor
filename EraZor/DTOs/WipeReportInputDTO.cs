namespace WebKlient.DTO_s
{
    public class WipeReportCreateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string WipeMethodName { get; set; }
        public int OverwritePasses { get; set; }
    }
}
