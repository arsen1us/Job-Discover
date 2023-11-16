using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace hr_developing.Controllers
{
    public class WorkExperienceController : Controller
    {
        HrV3Context database;
        UserService userService;
        IGenerateId generateId;

        public WorkExperienceController(HrV3Context database, UserService service, IGenerateId generateId)
        {
            this.database = database;
            this.userService = service;
            this.generateId = generateId;
        }

        [HttpGet]
        public IActionResult AddWK()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWK(WorkExperience wk)
        {
            string clientId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (wk == null)
            {
                throw new ArgumentNullException("wk == null");
            }

            Client client = await userService.GetUserByIdAsync(clientId);
            if (client == null)
            {
                throw new ArgumentNullException("client == null");
            }
            var wkId = generateId.Generate();
            wk.Id = wkId;
            wk.FkClientId = clientId;
            wk.FkClient = client;

            await database.AddAsync(wk);
            await database.SaveChangesAsync();
            await database.Database.CloseConnectionAsync();

            return Redirect("~/Profile/Index");

            string errors = string.Empty;

            //foreach(var er in ModelState)
            //{
            //    foreach(var ers in er.Value.Errors)
            //    {
            //        errors += " " + ers.ErrorMessage;
            //    }
            //}
            return Content(errors);
        }


        [AcceptVerbs("HttpGet", "HttpPost")]
        public IActionResult WorkingTimeCheck(string endingOfWork)
        {
            return NotFound();
        }
    }
}
