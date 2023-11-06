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
        public async Task<ActionResult> Authentication(AuthClientViewModel authenticationClient)
        {
            string email = authenticationClient.Email;
            string password = authenticationClient.Password;

            if (database.Clients.FirstOrDefault(client => client.Email == email && client.Password == password) == null)
            {
                return Redirect("/Authentication/AccessDenied");
            }

            List<Claim> clientClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("Password", password)
            };

            ClaimsIdentity clientIdentity = new ClaimsIdentity(clientClaims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clientIdentity));
            return Redirect("/shared/_layout");

        }

        public IActionResult AccessDenied()
        {
            return Content("AccessDenied");
        }
    }
}
