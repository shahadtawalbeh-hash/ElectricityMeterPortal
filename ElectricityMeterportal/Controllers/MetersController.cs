using ElectricityMeterportal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ElectricityMeterportal.Controllers
{
    public class MetersController : Controller
    {
        private readonly ElectricityMeterDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MetersController(ElectricityMeterDbContext context, UserManager<ApplicationUser> userManager)
        { 
            _context = context;
            _userManager= userManager;
        }
        [Authorize(Roles ="Employee")]
    
        public IActionResult Index()
        {
            var meters = _context.Meters.Include(m=>m.Request).ToList();

            return View(meters);
        }
        [HttpGet]
        public async Task<IActionResult> MyMeter()
        {
            var userId = _userManager.GetUserId(User);
            if(userId==null)
            {
                return RedirectToAction("CitizenLogin", "Account");
            }
            var meter=await _context.Meters.Include(m => m.Request).FirstOrDefaultAsync(m=>m.Request.UserId== userId&& m.Request.status=="Approved");
            if(meter==null)
            {
                return View(null);
            }
            return View(meter);
        }
        [HttpGet]
        public async Task<IActionResult> MyReadings()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToAction("CitizenLogin", "Account");
            }
            var readings = await _context.MeterReadings.Include(r => r.Meter).ThenInclude(m => m.Request)
                .Where(r => r.Meter.Request.UserId == userId && r.Meter.Request.status == "Approved").OrderByDescending(r => r.ReadingDate).ToListAsync();
          
           
            return View(readings);
        }
        [HttpGet]
        public async Task<IActionResult> MyBills()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return RedirectToAction("CitizenLogin", "Account");
            }

            var bills = await _context.Bills
                .Include(b => b.Meter)
                .ThenInclude(m => m.Request)
                .Where(b =>
                    b.Meter.Request.UserId == userId &&
                    b.Meter.Request.status == "Approved")
                .OrderByDescending(b => b.BillDate)
                .ToListAsync();

            return View(bills);
        }
        [HttpGet]
        public async Task<IActionResult> MyPayments()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return RedirectToAction("CitizenLogin", "Account");
            }

            var payments = await _context.Payments
                .Include(p => p.Bill)
                .ThenInclude(b => b.Meter)
                .ThenInclude(m => m.Request)
                .Where(p =>
                    p.Bill.Meter.Request.UserId == userId &&
                    p.Bill.Meter.Request.status == "Approved")
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return View(payments);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult Create()
        {
            var approvedRequests = _context.ElectricityRequests
                .Where(r => r.status == "Approved")
                .ToList();
            ViewBag.Requests = new SelectList(
                approvedRequests,
                "ID",
                "FullName");
            return View();
        }
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Meter meter)
        {
            if (_context.Meters.Any(m => m.MeterNumber == meter.MeterNumber))
            {
                ModelState.AddModelError(
                    "MeterNumber",
                    "This meter number already exists.");
            }
            var request = _context.ElectricityRequests
                .FirstOrDefault(r => r.ID == meter.RequestId);
            if (request == null)
            {
                ModelState.AddModelError(
                    "RequestId",
                    "Please select a valid request.");
            }
            else if (request.status != "Approved")
            {
                ModelState.AddModelError(
                    "RequestId",
                    "A meter can only be assigned to an approved request.");
            }
            if (ModelState.IsValid)
            {
                meter.CreatedAt = DateTime.Now;
                _context.Meters.Add(meter);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            var approvedRequests = _context.ElectricityRequests
                .Where(r => r.status == "Approved")
                .ToList();
            ViewBag.Requests = new SelectList(
                approvedRequests,
                "ID",
                "FullName",
                meter.RequestId);
            return View(meter);
        }
        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var meter = _context.Meters.Find(id);
            if (meter == null)
            {
                return NotFound();
            }
            return View(meter);
        }
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Meter meter)
        { 
          if(id != meter.MeterId)
            {
                return NotFound();
            }
          if(_context.Meters.Any(m=> m.MeterNumber == meter.MeterNumber && m.MeterId!=meter.MeterId))
            { ModelState.AddModelError("MeterNumber", "This Meter already exists"); }

          if (ModelState.IsValid)
            {
                _context.Meters.Update(meter);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View(meter);
        }
        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var meter = _context.Meters.Find(id);
            if(meter == null)
            { 
                return NotFound(); 
            }

            return View(meter);
        }
        [Authorize(Roles = "Employee")]
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var meter =_context.Meters.Find(id);
            if (meter == null)
            {
                return NotFound();
            }
            _context.Meters.Remove(meter);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));



        }





    }
}
