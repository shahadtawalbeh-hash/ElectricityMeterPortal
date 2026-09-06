using ElectricityMeterportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ElectricityMeterportal.Controllers
{
    [Authorize(Roles = "Employee")]
    public class PaymentsController : Controller
    {
        private readonly ElectricityMeterDbContext _context;
        public PaymentsController(ElectricityMeterDbContext context)
        {
            _context = context;
        }
       
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.Bill)
                .ThenInclude(b => b.Meter)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
            return View(payments);
        }
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var payment = await _context.Payments
                .Include(p => p.Bill)
                .ThenInclude(b => b.Meter)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            var bills = _context.Bills
                .Include(b => b.Meter)
                .OrderByDescending(b => b.BillDate)
                .ToList();
            ViewBag.Bills = new SelectList(
                bills,
                "BillId",
                "BillId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            var bill = await _context.Bills
                .FirstOrDefaultAsync(b => b.BillId == payment.BillId);
            if (bill == null)
            {
                ModelState.AddModelError(
                    "BillId",
                    "Please select a valid bill.");
            }
           
            ModelState.Remove("Bill");
            if (payment.AmountPaid <= 0)
            {
                ModelState.AddModelError(
                    "AmountPaid",
                    "Payment amount must be greater than zero.");
            }
            if (bill != null && payment.AmountPaid > bill.Amount)
            {
                ModelState.AddModelError(
                    "AmountPaid",
                    "Payment amount cannot be greater than the bill amount.");
            }
            if (payment.PaymentDate == null)
            {
                payment.PaymentDate = DateTime.Now;
            }
            if (ModelState.IsValid)
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var bills = _context.Bills
                .Include(b => b.Meter)
                .OrderByDescending(b => b.BillDate)
                .ToList();
            ViewBag.Bills = new SelectList(
                bills,
                "BillId",
                "BillId",
                payment.BillId);
            return View(payment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var payment = await _context.Payments
                .FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            var bills = _context.Bills
                .OrderByDescending(b => b.BillDate)
                .ToList();
            ViewBag.Bills = new SelectList(
                bills,
                "BillId",
                "BillId",
                payment.BillId);
            return View(payment);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }
            var bill = await _context.Bills
                .FirstOrDefaultAsync(b => b.BillId == payment.BillId);
            if (bill == null)
            {
                ModelState.AddModelError(
                    "BillId",
                    "Please select a valid bill.");
            }
            if (payment.AmountPaid <= 0)
            {
                ModelState.AddModelError(
                    "AmountPaid",
                    "Payment amount must be greater than zero.");
            }
            if (bill != null && payment.AmountPaid > bill.Amount)
            {
                ModelState.AddModelError(
                    "AmountPaid",
                    "Payment amount cannot be greater than the bill amount.");
            }
            if (ModelState.IsValid)
            {
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var bills = _context.Bills
                .OrderByDescending(b => b.BillDate)
                .ToList();
            ViewBag.Bills = new SelectList(
                bills,
                "BillId",
                "BillId",
                payment.BillId);
            return View(payment);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var payment = await _context.Payments
                .Include(p => p.Bill)
                .ThenInclude(b => b.Meter)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments
                .FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}