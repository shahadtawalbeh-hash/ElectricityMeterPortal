
using System.ComponentModel.DataAnnotations;
namespace ElectricityMeterportal.Models
{
    public class ElectricityRequest
    {
        public int ID { get; set; }

        [Required(ErrorMessage="National ID  is required.")]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "Full Name ID  is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        public  string PhoneNumber { get; set; }

        [Required(ErrorMessage = " Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Meter Type  is required.")]
        public string MeterType { get; set; }
        public string status { get; set; } = "pending";
        public ICollection<RequestDocument> Documents { get; set; } = new List<RequestDocument>();
        public string? UserId { get; set; }
        public virtual Meter? Meter { get; set; }
       

    }
}
