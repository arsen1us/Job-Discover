using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace hr_developing.Controllers
{
    public class ResumeController : Controller
    {
        IGenerateId IdGenerator;
        HrV3Context database;
        public ResumeController(HrV3Context database, IGenerateId idGenerator)
        {
            this.database = database;
            IdGenerator = idGenerator;
        }

        [HttpGet]
        public IActionResult AddResume()
        {
            return View("addResume");
        }

        [HttpPost]
        public async Task<ActionResult> AddResume(Resume resume)
        {

            string id = IdGenerator.Generate();
            resume.Id = id;
            //string profession = resume.Profession;
            //double salary = resume.Salary;
            //string keySkills = resume.Keyskills;

            string? clientId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (clientId == null)
            {
                throw new Exception("You are not authenticated or registrated");
            }
            Client? currentClient = await database.Clients.FirstOrDefaultAsync(client => client.Id == clientId);
            if (currentClient == null)
            {
                throw new Exception("Your are not registrated");
            }

            resume.FkClientId = clientId;
            resume.FkClient = currentClient;

            await database.Resumes.AddAsync(resume);
            await database.SaveChangesAsync();
            await database.Database.CloseConnectionAsync();


            return Redirect("~/Profile/Index");
        }

        public IActionResult RemoveResume()
        {
            throw new Exception();
        }


    }
}
