using ElectricityMeterportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;



namespace ElectricityMeterportal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        { 
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CitizenRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CitizenRegister(
            string nationalId,
            string fullname,
            string phonenumber,
            string password,
            string confirmpassword)
        {
            if (password != confirmpassword)
            { 
            ModelState.AddModelError(
                "password","password do not match.");
       
            }
            if(!ModelState.IsValid)
            { return View(); }
            var exitingUser = await _userManager.FindByNameAsync(nationalId);
            if (exitingUser != null)
            { ModelState.AddModelError("nationalId ", "This National ID is already registered. ");
            return View();
            }
            var user = new ApplicationUser
            { 
                UserName = nationalId,
                PhoneNumber = phonenumber,
                FullName=fullname
              
            };

            var result =await _userManager.CreateAsync(user,password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Citizen");
                return RedirectToAction("CitizenLogin");

            }
            foreach (var error in result.Errors)
            {

                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CitizenLogin()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CitizenLogin(string nationalid ,string password)
        {
            var user = await _userManager.FindByNameAsync(nationalid);
            if (user == null)
            {

                ModelState.AddModelError(" ", "Invalid NationalID or Password.");
                return View();
            
            }
            var passwordResult = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordResult)
            {
                ModelState.AddModelError(" ", "Invalid NationalID or Password.");
                return View();

            }
            var isCitizen = await _userManager.IsInRoleAsync(user, "Citizen");
            if(!isCitizen)
            {

                ModelState.AddModelError("", "This account isn't a Citizen account.");
                return View();
            }
            await _signInManager.SignInAsync(user, isPersistent:false);
            return RedirectToAction("CitizenDashboard", "Account");

        
        }
        [HttpGet]
        public IActionResult EmployeeLogin()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeLogin(string employeeid, string password)
        {
            var user = await _userManager.FindByNameAsync(employeeid);
            if (user == null)
            {

                ModelState.AddModelError(" ", "Invalid EmployeeID or Password.");
                return View();

            }
            var passwordResult = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordResult)
            {
                ModelState.AddModelError(" ", "Invalid EmployeeID or Password.");
                return View();

            }
            var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");
            if (!isEmployee)
            {

                ModelState.AddModelError("", "This account isn't a Employee account.");
                return View();
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("EmployeeDashboard", "Account");


        }

        [HttpGet]
        [Authorize(Roles="Employee")]
        public IActionResult EmployeeDashboard()
        {
            return View("~/Views/Account/EmployeeDashboard.cshtml");
        }

        [HttpGet]
        public IActionResult CitizenDashboard()
        {
            return View("~/Views/Account/CitizenDashboard.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
