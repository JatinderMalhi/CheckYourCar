using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using CheckYourCar.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourCar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;

        }
        //get models
        [HttpPost]
        public ActionResult GetModelsByCarMakeID(string makeID)
        {
            var carmodels = _context.CarsModel.ToList();

            var models = carmodels.Where(z => z.CompanyID == Convert.ToInt32(makeID)).Select(a => "<option value='" + a.ID + "'>" + a.Model + "</option>'");

            return Content(String.Join("", models));


        }
        [HttpPost]
        public ActionResult GetCarsRecalls(int carmake, int ddlModels)
        {
            var carrecalls = _context.CarsRecalls.Where(z => z.CompanyID == carmake && z.ModelID == ddlModels).ToList();

            TempData["Company"] = _context.CarsMake.Where(m => m.ID == carmake).FirstOrDefault().Company;
            TempData["Model"] = _context.CarsModel.Where(m => m.ID == ddlModels).FirstOrDefault().Model;
            return View(carrecalls);

        }

        //
        public IActionResult AddCarMake()
        {
            return View();
        }

        public IActionResult Index()
        {
            ViewData["carmake"] = new SelectList(_context.CarsMake, "ID", "Company");
            ViewData["carmodel"] = new SelectList(_context.CarsModel, "ID", "Model");


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        //creating roles
        public async Task CreateRolesandUsers()
        {

            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x) {
                // first we create Admin role   
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);
            }
            //Then we create a user 
            var user = new ApplicationUser();
            user.UserName = "Admin";
            user.Email = "admin@gmail.com";
            string userPWD = "Abc123!";

            IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

            //Add default User to Role Admin    
            if (chkUser.Succeeded) {
                var result = await _userManager.AddToRoleAsync(user, "Admin");
            }

        }
    }
}
