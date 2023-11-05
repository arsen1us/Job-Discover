using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using hr_developing.Models;

namespace hr_developing.Controllers
{
    public class ClientController : Controller
    {
        IAuthenticationService authenticationService;
        IAuthorizationService authorizationService;
        HrV3Context database;

        public ClientController(HrV3Context database, IAuthenticationService authenticationService, IAuthorizationService authorizationService)
        {
            this.database = database;
            this.authenticationService = authenticationService;
            this.authorizationService = authorizationService;
        }

        
        public IActionResult Index()
        {
            ViewBag.Title = "Index";
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View("registration_form");
        }

        [HttpPost]
        public async Task<ActionResult> Registration(AuthClientModel client)
        {
            using var db = database;

            if (client == null)
            {
                throw new Exception("Client == null");
            }

            if (client.Name == null || client.Surname == null || client.Email == null || client.Password == null)
            {
                throw new Exception("Client Properties == null");
            }

            client.Id = GenerateUserId();
            try
            {
                await database.Database.OpenConnectionAsync();
                await database.AddAsync(client);
                await database.SaveChangesAsync();
                await database.Database.CloseConnectionAsync();
            }
            catch
            {
                throw new Exception("Не удалось подключиться к базе данных");
            }

            List<Claim> claimsList = new List<Claim>(4);
            if (client.Id == null)
            {
                throw new Exception("id, name, surname is null");
            }

            claimsList.Add(new Claim(ClaimTypes.NameIdentifier, client.Id));
            claimsList.Add(new Claim(ClaimTypes.Email, client.Email));
            claimsList.Add(new Claim(ClaimTypes.Name, client.Name));
            claimsList.Add(new Claim(ClaimTypes.Surname, client.Surname));

            var claimsIdentity = new ClaimsIdentity(claimsList, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect("~/Client/Index");
        }
        public string GenerateUserId()
        {
            string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random rand = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 15; i++)
            {
                int index = rand.Next(0, symbols.Length - 1);
                sb.Append(symbols[index]);
            }
            string userid = sb.ToString();
            return userid;

            // claimTypes.UserIdentifier("id", "userid)
        }
    }
}
