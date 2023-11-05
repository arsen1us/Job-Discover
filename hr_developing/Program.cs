using hr_developing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

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


var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json");
var configuration = configurationBuilder.Build();

string connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HrV3Context>(builder =>
{
    builder.UseSqlServer(connectionString);
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
