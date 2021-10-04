using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CheckYourCar.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using CheckYourCar.Data;


namespace CheckYourCar.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, ApplicationDbContext context)
        {
            _context = context;

            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public IEnumerable<SelectListItem> carmake { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Phone Number")]
            public string Phone { get; set; }
            public string userrole { get; set; }



            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            //[Required]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "Password")]
            //public string Password { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ViewData["carmake"] = new SelectList(_context.CarsMake, "ID", "Company");
            ViewData["carmodel"] = new SelectList(_context.CarsModel, "ID", "Model");
            List<CarMake> availableCarMake = _context.CarsMake.ToList();
            carmake = availableCarMake.Select(x => new SelectListItem() { Text = x.Company.ToString(), Value = x.ID.ToString() }).ToList();
            ViewData["carmake"] = carmake;
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        //get models

        public async Task<IActionResult> OnPostAsync(CarUsers carUsers, IFormCollection form)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            //var carmodel = _context.CarsModel.Where(cm => cm.ID == Convert.ToInt32(carUsers.CarModel)).FirstOrDefault().Model;

            //var carmake = _context.CarsMake.Where(cm => cm.ID == Convert.ToInt32(carUsers.CarMake)).FirstOrDefault().Company;
            //carUsers.CarMake = carmake;
            //carUsers.CarModel = carmodel;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid) {
                //  var user = new ApplicationUser {UserName = Input.Email, Email = Input.Email,EmailConfirmed=true };
                // var result = await _userManager.CreateAsync(user, Input.Password);
                // var result1 = await _userManager.AddToRoleAsync(user, "User");
                if (ModelState.IsValid) {
                    _context.Add(carUsers);
                    await _context.SaveChangesAsync();
                    TempData["msg"] = "Account created successfully! You receive email when any recall occur for your car";

                    // return RedirectToAction(nameof(Index));
                }
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("User created a new account with password.");
                //    TempData["msg"] = "Account created successfully! Please login to continue.";

                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                //var callbackUrl = Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                //{
                //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                //}
                //else
                //{
                //    await _signInManager.SignInAsync(user, isPersistent: false);
                //    return LocalRedirect(returnUrl);
                //}
            }
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //}

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
