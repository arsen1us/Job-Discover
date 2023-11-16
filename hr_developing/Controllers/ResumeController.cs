using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace hr_developing.Controllers
{
    public class ResumeController : Controller
    {
        IGenerateId IdGenerator;
        HrV3Context database;
        //ResumeService resumeService;
        UserService userService;
        public ResumeController(HrV3Context database, IGenerateId idGenerator, UserService userService)
        {
            this.database = database;
            IdGenerator = idGenerator;
            //this.resumeService = resumeService;
            this.userService = userService;
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

            currentClient.Resumes.Add(resume);

            await database.Resumes.AddAsync(resume);
            await database.SaveChangesAsync();
            await database.Database.CloseConnectionAsync();


            return Redirect("~/Profile/Index");
        }

        public async Task<IActionResult> ResumeList()
        {
            var allresumes = await database.Resumes.ToListAsync();

            return View(allresumes);
        }

        public IActionResult RemoveResume()
        {
            throw new Exception();
        }

        //public IActionResult Method()
        //{
        //    var resumes = resumeService.AddResumesToDistributedCache();
        //    return View("ResumeList", resumes);
        //}
        //[HttpPost]
        //public string ResumesSearch(string searchString, bool notUsed)
        //{
        //    return "From [HttpPost]Index: filter on " + searchString;
        //}


        public async Task<IActionResult> ResumesSearch(string searchString)
        {
            if(database.Resumes == null)
            {
                return Problem("No resumes fields in Resume database");
            }
            var resumes = from resume in database.Resumes
                          select resume;

            if(!string.IsNullOrEmpty(searchString))
            {
                resumes = resumes.Where(resume => resume.Profession.Contains(searchString) || resume.Keyskills.Contains(searchString));    
            }
            return View("ResumeList", await resumes.ToListAsync());
            
        }

        public async Task<IActionResult> ResumePage(string resumeId)
        {
            if(resumeId == null)
            {
                throw new Exception("resumeId == null");
            }
            
            var resume = await database.Resumes.FirstOrDefaultAsync(resume => resume.Id == resumeId);

            if(resume == null)
            {
                return StatusCode(404, "Resume not found");
            }

            //var client = await userService.GetUserByIdAsync(resume.FkClientId);

            return View("resumePage", resume);
        }


    }
}
