using hr_developing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;
using static System.GC;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(builder =>
{
    builder.LoginPath = "";
    builder.AccessDeniedPath = "";
});

builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("Policy1", policy =>
    {
        policy.RequireClaim("", "");
    });

    configure.AddPolicy("Policy2", policy =>
    {
        policy.RequireClaim("", "");
    });
});

builder.Services.AddTransient<IGenerateId, GenerateId>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ResumeService>();
builder.Services.AddTransient<WorkExperienceService>();


var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json");
var configuration = configurationBuilder.Build();

string connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HrV3Context>(builder =>
{
    builder.UseSqlServer(connectionString);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
    options.InstanceName = "local";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public interface IGenerateId
{
    string Symbols { get; }

    string Generate(int idLength = 15);
}

public class GenerateId : IGenerateId, IDisposable
{
    public string Symbols { get; } = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string Generate(int idLength = 15)
    {
        Random rand = new Random();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < idLength; i++)
        {
            int index = rand.Next(0, Symbols.Length - 1);
            sb.Append(Symbols[index]);
        }
        string userid = sb.ToString();
        return userid;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

//public class ResumeService
//{
//    public HrV3Context database;

//    public IDistributedCache cache;

//    public ResumeService(HrV3Context database, IDistributedCache distributedCache)
//    {
//        this.database = database;
//        cache = distributedCache;
//    }

//    public List<Resume> AddResumesToDistributedCache()
//    {
//        var jsonResumes = cache.GetString("resumes");
//        if (jsonResumes != null)
//        {
//            var resumes = JsonSerializer.Deserialize<List<Resume>>(jsonResumes);
//            return resumes;
//        }

//        var resumesFromDb = database.Resumes.ToList();

//        var serializedResumes = JsonSerializer.Serialize<List<Resume>>(resumesFromDb);

//        try
//        {
//            cache.SetString("resumes", serializedResumes, new DistributedCacheEntryOptions
//            {
//                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
//            });
//        }
//        catch
//        {
//            throw new Exception("OOPS! Problems with Redis");
//        }
//        return resumesFromDb;
//    }
//}

public class UserService 
{
    HrV3Context database;

    public UserService(HrV3Context database)
    {
        this.database = database;
    }

    public async Task<Client> GetUserByIdAsync(string clientId)
    {
        if(clientId == null)
        {
            throw new ArgumentNullException("clientId == null");
        }
        return await database.Clients.FirstOrDefaultAsync(client => client.Id == clientId);
    }

}

public class ResumeService
{
    HrV3Context database;

    public ResumeService(HrV3Context database)
    {
        this.database = database;
    }

    public async Task<List<Resume>> GetUserResumesAsync(string  clientId)
    {
        if (clientId == null)
        {
            throw new ArgumentNullException("clientId == null");
        }
        return await database.Resumes.Where(r => r.FkClientId == clientId).ToListAsync();
    }
}

public class WorkExperienceService
{
    HrV3Context database;

    public WorkExperienceService(HrV3Context database)
    {
        this.database = database;
    }

    public List<WorkExperience> GetWorkExperience(string clientId)
    {
        //var workexperience = await database.WorkExperiences.ToListAsync();

        var workexperience = from wk in database.WorkExperiences
                             where wk.FkClientId == clientId
                             select wk;

        if(workexperience == null)
        {
            return new List<WorkExperience>();
        }
        return workexperience.ToList();

    }
}


