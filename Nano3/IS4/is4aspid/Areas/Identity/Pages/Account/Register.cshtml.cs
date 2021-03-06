﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using is4aspid.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using is4aspid.Data;
using Microsoft.EntityFrameworkCore;

namespace is4aspid.Areas.Identity.Pages.Account
{
    [Authorize(Policy ="RequireAdministratorRole")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly HrContext _hrContext;
        private readonly NetSqlAzmanContext _azmanContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            HrContext hrContext,
            NetSqlAzmanContext azmanContext,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hrContext = hrContext;
            _azmanContext = azmanContext;
            _logger = logger;
            _emailSender = emailSender;
        }

        public IList<LookupItem> Divisions{get;set;}

        public IList<LookupItem> Employees{get;set;}

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Division is required")]
            [Display(Name = "Division")]
            public int DivisionId { get; set; }

            [Required(ErrorMessage ="Select an Employee to get Employee ID")]
            [Display(Name = "Employee ID")]
            public int EmployeeId { get; set; }


            [Required(ErrorMessage ="Please select an Employee to get Employee ID")]
            [Display(Name = "Employee Name")]
            public string EmployeeName { get; set; }

             [Required(ErrorMessage ="Display Name is required")]
            [Display(Name = "Display Name")]
            public string DisplayName { get; set; }

            [Required]
            [RegularExpression(@"^[\d\w\._\-]+@([\d\w\._\-]+\.)+[\w]+$", ErrorMessage = "Email is invalid")]
            //[Remote("CheckEmailAddress","Account",ErrorMessage ="Email is already registered",HttpMethod ="POST")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //Divisions = new List<LookupItem>
            //{
            //    new LookupItem{Id=1,Name="Musaffah Store"},    
            //    new LookupItem{Id=2,Name="Dubai Store"}
            //};

            Divisions=await _azmanContext
                .Divisions
                .Where(x=> x.ApplicationType=="ABS")
                .Select(x=> new LookupItem{Id=x.Id,Name=x.Name}).ToListAsync();

            Employees = await _hrContext.Employees
                .Where(x=> x.IsStaff && x.IsWorking)
                .Select(x=> new LookupItem {Id=x.Id,Name=x.Name}).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    DivisionId=Input.DivisionId,
                    EmployeeId=Input.EmployeeId,
                    EmployeeName=Input.DisplayName,
                    UserName = Input.Email,
                    Email = Input.Email
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

