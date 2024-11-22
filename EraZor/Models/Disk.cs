using System.ComponentModel.DataAnnotations.Schema;

namespace EraZor.Models
{

    public class Disk
    {

        public int DiskID { get; set; }


        public string Type { get; set; }

 
        public int Capacity { get; set; }


        public string Status { get; set; }

     
        public string? Path { get; set; }

  
        public string? SerialNumber { get; set; }

      
        public string? Manufacturer { get; set; }
    }
}