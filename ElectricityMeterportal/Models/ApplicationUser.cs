
using Microsoft.AspNetCore.Identity;
namespace ElectricityMeterportal.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}

