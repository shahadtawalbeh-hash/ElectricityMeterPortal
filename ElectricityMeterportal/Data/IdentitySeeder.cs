using Microsoft.AspNetCore.Identity;
using ElectricityMeterportal.Models;
namespace ElectricityMeterportal.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            
            if (!await roleManager.RoleExistsAsync("Citizen"))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("Citizen"));
            }
            
            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("Employee"));
            }
        }
        public static async Task SeedEmployeeAsync(
            UserManager<ApplicationUser> userManager)
        {
            
            string employeeId = "EMP001";
            string password = "Employee@123";
            
            var employee = await userManager.FindByNameAsync(employeeId);
            if (employee == null)
            {
                employee = new ApplicationUser
                {
                    UserName = employeeId,
                    FullName = "System Employee"
                };
                var result = await userManager.CreateAsync(
                    employee,
                    password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        employee,
                        "Employee");
                }
            }
        }
    }
}