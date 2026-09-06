
using System.ComponentModel.DataAnnotations;
namespace ElectricityMeterportal.Models
{
    public class RequestDocument
    {
        public int ID { get; set; }

        [Required]
        public int ElectricityRequestID { get; set; }
        public  ElectricityRequest ElectricityRequest { get; set; }

        [Required]
        public string DocumentType { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
