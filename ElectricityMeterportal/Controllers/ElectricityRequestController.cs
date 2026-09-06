using Microsoft.EntityFrameworkCore;
using ElectricityMeterportal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Authorization;

namespace ElectricityMeterportal.Controllers
{
    public class ElectricityRequestController : Controller
    {

        private readonly ElectricityMeterDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        private readonly UserManager<ApplicationUser> _userManager;
        public ElectricityRequestController(ElectricityMeterDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _enviroment = environment;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ElectricityRequest request, IFormFile nationalidDocument, IFormFile ownershipDocument, IFormFile clearanceDocument)
        {
            if (nationalidDocument == null || ownershipDocument == null || clearanceDocument == null)
            {
                ModelState.AddModelError(" ", "All three Documents are required");
            }
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {

                    return RedirectToAction("CitizenLogin", "Account");
                }
                request.UserId = userId;
                request.status = "Pending";
                _context.ElectricityRequests.Add(request);
                await _context.SaveChangesAsync();
                var uploadsfolder = Path.Combine(_enviroment.WebRootPath, "uploads", "documents");
                if (!Directory.Exists(uploadsfolder))
                {
                    Directory.CreateDirectory(uploadsfolder);
                }
                await SaveDocument(nationalidDocument, "National ID", request.ID, uploadsfolder);
                await SaveDocument(ownershipDocument, "Proof of OwnerShip", request.ID, uploadsfolder);
                await SaveDocument(clearanceDocument, "Clearance Certificate", request.ID, uploadsfolder);
                return RedirectToAction(nameof(Index));

            }
            return View(request);

        }
        private async Task SaveDocument(IFormFile file, string documentType, int requestId, string uploadsfolder)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filepath = Path.Combine(uploadsfolder, uniqueFileName);
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var document = new RequestDocument
            {
                ElectricityRequestID = requestId,
                DocumentType = documentType,
                FileName = file.FileName,
                FilePath = "/uploads/documents/" + uniqueFileName
            };
            _context.RequestDocuments.Add(document);
            await _context.SaveChangesAsync();

        }
        [Authorize(Roles ="Employee")]
        public IActionResult Index()
        {
            var requests = _context.ElectricityRequests.ToList();
            return View(requests);

        }
        public IActionResult Details(int id)
        {
            var request = _context.ElectricityRequests.Include(r => r.Documents).FirstOrDefault(r => r.ID == id);
            if (request == null)
            {
                return NotFound();

            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole("Citizen") && request.UserId != userId)
            {
                return Forbid();
            }

            return View(request);


        }
        [HttpGet]
        public IActionResult Edit(int id)
        {

            var request = _context.ElectricityRequests.FirstOrDefault(r => r.ID == id);
            if (request == null)
            {
                return NotFound();

            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole("Citizen") && request.UserId != userId)
            {
                return Forbid();
            }
            if (request.status != "Pending")
            {
                return BadRequest("You can't edit this request");

            }
            return View(request);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ElectricityRequest request)
        {

            if (id != request.ID)
            {
                return NotFound();

            }
            var exitingrequest = _context.ElectricityRequests.FirstOrDefault(r => r.ID == id);
            if (exitingrequest == null)
            {
                return NotFound();

            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole("Citizen") && request.UserId != userId)
            {
                return Forbid();
            }
            if (exitingrequest.status != "Pending")
            {
                return BadRequest("You can't edit this request");


            }
            if (ModelState.IsValid)
            {
                exitingrequest.NationalID = request.NationalID;
                exitingrequest.FullName = request.FullName;
                exitingrequest.PhoneNumber = request.PhoneNumber;
                exitingrequest.Address = request.Address;
                exitingrequest.MeterType = request.MeterType;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View(request);

        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var request = _context.ElectricityRequests.FirstOrDefault(r => r.ID == id);
            if (request == null)
            {
                return NotFound();
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole("Citizen") && request.UserId != userId)
            {
                return Forbid();
            }
            if (request.status != "Pending")
            {
                return BadRequest("You can't delete this request");
            }

            return View(request);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var request = _context.ElectricityRequests.FirstOrDefault(r => r.ID == id);
            if (request == null)
            {
                return NotFound();
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole("Citizen") && request.UserId != userId)
            {
                return Forbid();
            }
            if (request.status != "Pending")
            {
                return BadRequest("You can't delete this request.");
            }
            _context.ElectricityRequests.Remove(request);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult MyRequests()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) {
                return RedirectToAction("CitizenLogin", "Account");

            }
            var requests =_context.ElectricityRequests.Where(r =>r.UserId==userId).ToList();

            return View(requests);
        
        }
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var request = _context.ElectricityRequests
                .FirstOrDefault(r => r.ID == id);

            if (request == null)
            {
                return NotFound();
            }

            request.status = "Approved";

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var request = _context.ElectricityRequests
                .FirstOrDefault(r => r.ID == id);

            if (request == null)
            {
                return NotFound();
            }

            request.status = "Rejected";

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}