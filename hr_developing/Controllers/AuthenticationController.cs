using hr_developing.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hr_developing.Controllers
{
    public class AuthenticationController : Controller
    {
        HrV3Context database;
        IAuthenticationService authenticationService;
        public AuthenticationController(HrV3Context database, IAuthenticationService authenticationService)
        {
            this.database = database;
            this.authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Authentication()
        {
            return View("authenticationForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Authentication(AuthClientViewModel authenticationClient)
        {
            string email = authenticationClient.Email;
            string password = authenticationClient.Password;

            var client = database.Clients.FirstOrDefault(client => client.Email == email && client.Password == password);

			if(client == null)
            {
                return Redirect("/Authentication/AccessDenied");
            }

            List<Claim> clientClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, client.Id),
                new Claim(ClaimTypes.Email, email),
                new Claim("Password", password)
            };

            ClaimsIdentity clientIdentity = new ClaimsIdentity(clientClaims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clientIdentity));
            return Redirect("/Profile/Index");
        }

        public IActionResult Quit()
        {
            return View("signOut");
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Profile/Index");
        }

        public IActionResult AccessDenied()
        {
            return Content("AccessDenied");
        }
    }
}
