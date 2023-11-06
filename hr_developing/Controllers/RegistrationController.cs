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
    public class RegistrationController : Controller
    {

        HrV3Context database;

        public RegistrationController(HrV3Context database)
        {
            this.database = database;
        }

        
        public IActionResult Index()
        {
            ViewBag.Title = "Index";
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View("registrationForm");
        }

        [HttpPost]
        public async Task<ActionResult> Registration(RegClientViewModel client)
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
            claimsList.Add(new Claim("Password", client.Password));
            claimsList.Add(new Claim(ClaimTypes.Name, client.Name));
            claimsList.Add(new Claim(ClaimTypes.Surname, client.Surname));


            var claimsIdentity = new ClaimsIdentity(claimsList, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect("~/Authentication/Index");
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
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckEmail(string email)
        {
            var client = database.Clients.FirstOrDefault(client => client.Email == email);
            if (client != null)
            {
                return Json(false);
            }
            return Json(true);
        }


    }
}
