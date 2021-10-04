using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CheckYourCar.Data;
using CheckYourCar.Models;

namespace CheckYourCar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IEnumerable<SelectListItem> carmake { get; set; }

        public CarsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;

        }


        //// GET: CarMake
        public async Task<IActionResult> Index()
        {

            return View(await _context.CarsMake.ToListAsync());
        }

        // GET: CarMake/Create
        public IActionResult AddCarMake()
        {
            return View();
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("ID,Company")] CarMake carmake)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid) {
                _context.Add(carmake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carmake);
        }
        public IActionResult AddModel(int id, string name)
        {
            TempData["companyid"] = id;
            TempData["companyName"] = name;

            return View();
        }
        //add car recalls
        public IActionResult AddRecall()
        {
            ViewData["carmake"] = new SelectList(_context.CarsMake, "ID", "Company");
            ViewData["carmodel"] = new SelectList(_context.CarsModel, "ID", "Model");
            List<CarMake> availableCarMake = _context.CarsMake.ToList();
            carmake = availableCarMake.Select(x => new SelectListItem() { Text = x.Company.ToString(), Value = x.ID.ToString() }).ToList();
            ViewData["carmake"] = carmake;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveCarRecall(CarsRecall carsrecall)
        {
            carsrecall.RecallDate = DateTime.Today;
            if (ModelState.IsValid) {
                var carusersEmail = _context.CarUsers.Where(cu => cu.CarMake == carsrecall.CompanyID && cu.CarModel == carsrecall.ModelID).ToList();

                string body1 = carsrecall.Issues.ToString();
                for (var i = 0; i < carusersEmail.Count; i++) {

                    var fromAddress = new MailAddress("checkrecallincar@gmail.com", "Check Your Car");
                    var toAddress = new MailAddress(carusersEmail[i].Email, carusersEmail[i].FirstName + " " + carusersEmail[i].LastName);
                    const string fromPassword = "*@RogerFederer";
                    const string subject = "Recall for your car";
                    string body = body1;

                    var smtp = new SmtpClient {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress) {
                        Subject = subject,
                        Body = body
                    }) {
                        smtp.Send(message);
                    }
                }
                _context.Add(carsrecall);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("AddRecall");

        }


        public async Task<IActionResult> ViewModel(int id, string name)
        {
            TempData["companyid"] = id;
            TempData["companyName"] = name;

            return View(await _context.CarsModel.Where(m => m.CompanyID == id).ToListAsync());
        }
        //for models
        // GET: carmake/Edit/5
        public async Task<IActionResult> EditModel(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var carmodel = await _context.CarsModel.FindAsync(id);
            if (carmodel == null) {
                return NotFound();
            }
            TempData["companyName"] = _context.CarsMake.Where(c => c.ID == carmodel.CompanyID).FirstOrDefault().Company;
            return View(carmodel);
        }

        // POST: carmake/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModel(int id, [Bind("ID,CompanyID,Model")] CarModel carmodel)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (id != carmodel.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(carmodel);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    throw;

                }
                return RedirectToAction("Index");
            }
            return View(carmodel);
        }

        // GET: carmake/Delete/5
        public async Task<IActionResult> DeleteModel(int? id)
        {
            if (id == null) {
                return NotFound();
            }
            var carmodel = await _context.CarsModel.FindAsync(id);
            _context.CarsModel.Remove(carmodel);
            await _context.SaveChangesAsync();

            if (carmodel == null) {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModel([Bind("CompanyID,Model")] CarModel carmodel)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid) {

                _context.Add(carmodel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carmodel);
        }


        // GET: carmake/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var carmake = await _context.CarsMake.FindAsync(id);
            if (carmake == null) {
                return NotFound();
            }
            return View(carmake);
        }

        // POST: carmake/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Company")] CarMake carmake)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (id != carmake.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(carmake);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(carmake);
        }

        // GET: carmake/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var carmake = await _context.CarsMake.FindAsync(id);
            _context.CarsMake.Remove(carmake);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
