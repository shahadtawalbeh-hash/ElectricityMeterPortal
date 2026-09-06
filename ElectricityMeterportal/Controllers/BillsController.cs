using ElectricityMeterportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ElectricityMeterportal.Controllers
{
    [Authorize(Roles = "Employee")]
    public class BillsController : Controller
    {
        private readonly ElectricityMeterDbContext _context;
        public BillsController(ElectricityMeterDbContext context)
        {
            _context = context;
        }
      
        public async Task<IActionResult> Index()
        {
            var bills = await _context.Bills
                .Include(b => b.Meter)
                .OrderByDescending(b => b.BillDate)
                .ToListAsync();
            return View(bills);
        }
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = await _context.Bills
                .Include(b => b.Meter)
                .FirstOrDefaultAsync(b => b.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            var meters = _context.Meters
                .OrderBy(m => m.MeterNumber)
                .ToList();
            ViewBag.Meters = new SelectList(
                meters,
                "MeterId",
                "MeterNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bill bill)
        {
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m => m.MeterId == bill.MeterId);
            if (meter == null)
            {
                ModelState.AddModelError(
                    "MeterId",
                    "Please select a valid meter.");
            }
            
            ModelState.Remove("Meter");
            
            if (bill.CurrentReading < bill.PreviousReading)
            {
                ModelState.AddModelError(
                    "CurrentReading",
                    "Current reading cannot be less than previous reading.");
            }
           
            bill.Consumption =
                bill.CurrentReading - bill.PreviousReading;
            if (bill.BillDate == null)
            {
                bill.BillDate = DateTime.Now;
            }
            if (ModelState.IsValid)
            {
                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var meters = _context.Meters
                .OrderBy(m => m.MeterNumber)
                .ToList();
            ViewBag.Meters = new SelectList(
                meters,
                "MeterId",
                "MeterNumber",
                bill.MeterId);
            return View(bill);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = await _context.Bills
                .FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            var meters = _context.Meters
                .OrderBy(m => m.MeterNumber)
                .ToList();
            ViewBag.Meters = new SelectList(
                meters,
                "MeterId",
                "MeterNumber",
                bill.MeterId);
            return View(bill);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Bill bill)
        {
            if (id != bill.BillId)
            {
                return NotFound();
            }
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m => m.MeterId == bill.MeterId);
            if (meter == null)
            {
                ModelState.AddModelError(
                    "MeterId",
                    "Please select a valid meter.");
            }
            if (ModelState.IsValid)
            {
                bill.Consumption =
                    bill.CurrentReading - bill.PreviousReading;
                _context.Bills.Update(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var meters = _context.Meters
                .OrderBy(m => m.MeterNumber)
                .ToList();
            ViewBag.Meters = new SelectList(
                meters,
                "MeterId",
                "MeterNumber",
                bill.MeterId);
            return View(bill);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = await _context.Bills
                .Include(b => b.Meter)
                .FirstOrDefaultAsync(b => b.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.Bills
                .FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}