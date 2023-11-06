using hr_developing.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace hr_developing.Controllers
{
    public class ProfileController : Controller
    {
        HrV3Context database;
        public ProfileController(HrV3Context database)
        {
            this.database = database;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ClientProfile()
        {
            if(User.FindFirst(ClaimTypes.NameIdentifier).Value == null)
            {
                return Redirect("clientNoRegAndNoAuth");
            }

            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var client = database.Clients.FirstOrDefault(client => client.Id == id);

            if(client == null)
            {
                return Redirect("clientNoRegAndNoAuth"); // перенапрявляем на главную страницу (можно
            }

            return View(client);
        }

    }
}
