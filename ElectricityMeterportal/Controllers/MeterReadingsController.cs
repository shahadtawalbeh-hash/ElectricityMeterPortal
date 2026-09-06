using ElectricityMeterportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ElectricityMeterportal.Controllers
{
    [Authorize(Roles = "Employee")]
    public class MeterReadingsController : Controller
    {
        private readonly ElectricityMeterDbContext _context;
        public MeterReadingsController(ElectricityMeterDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var readings = await _context.MeterReadings
                .Include(r => r.Meter)
                .OrderByDescending(r => r.ReadingDate)
                .ToListAsync();
            return View(readings);
        }
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reading = await _context.MeterReadings
                .Include(r => r.Meter)
                .FirstOrDefaultAsync(r => r.ReadingId == id);
            if (reading == null)
            {
                return NotFound();
            }
            return View(reading);
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
        public async Task<IActionResult> Create(MeterReading reading)
        {
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m => m.MeterId == reading.MeterId);
            if (meter == null)
            {
                ModelState.AddModelError(
                    "MeterId",
                    "Please select a valid meter.");
            }
           
            ModelState.Remove("Meter");
            if (ModelState.IsValid)
            {
                if (reading.ReadingDate == null)
                {
                    reading.ReadingDate = DateTime.Now;
                }
                _context.MeterReadings.Add(reading);
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
                reading.MeterId);
            return View(reading);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reading = await _context.MeterReadings
                .FindAsync(id);
            if (reading == null)
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
                reading.MeterId);
            return View(reading);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            MeterReading reading)
        {
            if (id != reading.ReadingId)
            {
                return NotFound();
            }
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m => m.MeterId == reading.MeterId);
            if (meter == null)
            {
                ModelState.AddModelError(
                    "MeterId",
                    "Please select a valid meter.");
            }
            if (ModelState.IsValid)
            {
                _context.MeterReadings.Update(reading);
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
                reading.MeterId);
            return View(reading);
        }
        // GET: MeterReadings/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reading = await _context.MeterReadings
                .Include(r => r.Meter)
                .FirstOrDefaultAsync(r => r.ReadingId == id);
            if (reading == null)
            {
                return NotFound();
            }
            return View(reading);
        }
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reading = await _context.MeterReadings
                .FindAsync(id);
            if (reading == null)
            {
                return NotFound();
            }
            _context.MeterReadings.Remove(reading);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}