using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheduling_Simulator.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Scheduling_Simulator.Controllers
{
    public class UserController : Controller
    {

        private readonly AppDbContext _context;
        

       
        public UserController(AppDbContext context)
        {
            _context = context;
           
        }

        public IActionResult Index()
        {
            return View();
        }

      
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            var emailClaim = result.Principal.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                return RedirectToAction("Login");
            }


            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == emailClaim.Value);

            if (user == null)
            {
                user = new User { Email = emailClaim.Value };
                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }


        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
