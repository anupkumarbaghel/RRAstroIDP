using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using IdentityServerWithAspNetIdentity.Models;
using IdentityServerWithAspNetIdentity.Models.AccountViewModels;
using IdentityServerWithAspNetIdentity.Services;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http.Authentication;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityServerWithAspNetIdentity.ApiControllers
{
    [Produces("application/json")]
    [Route("api/loginout")]
    public class LoginoutController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly AccountService _account;

        public LoginoutController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            IIdentityServerInteractionService interaction,
            IHttpContextAccessor httpContext,
            IClientStore clientStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<LoginoutController>();
            _interaction = interaction;
            _clientStore = clientStore;

            _account = new AccountService(interaction, httpContext, clientStore);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostRegisterModel([FromBody] RegisterViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email
                    ,FullName=model.FullName
                    ,DateOfBirth=model.DateOfBirth
                    ,TimeOfBirth=model.TimeOfBirth
                    ,BirthCity=model.BirthCity
                    ,BirthCountry=model.BirthCountry
                };
    
             
                user.Claims.Add(new IdentityUserClaim<string>
                {
                    ClaimType = "email",
                    ClaimValue = model.Email
                });
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return Ok("Register Successfull");
                }
                return BadRequest("User Creation failed");
            }

            // If we got this far, something failed, redisplay form
            return BadRequest("Data is not valid");
        }


    }
}
