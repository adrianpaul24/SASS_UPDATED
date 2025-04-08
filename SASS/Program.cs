using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using SASS.Models;
using SASS.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configure MySQL using MySql.EntityFrameworkCore
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString)); // Using MySQL provider for MariaDB

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Make sure Razor Pages are added to the service collection

// Add Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";  // Redirect to login if not authenticated
        options.LogoutPath = "/Auth/Logout"; // Path for logging out
        options.AccessDeniedPath = "/Auth/Login"; // Redirect here if access is denied
    });

builder.Services.AddAuthorization();

var app = builder.Build();

//verify db connection, count the number of users and show in CLI/Terminal upon running code
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    var users = dbContext.Users.ToList();
//    Console.WriteLine($"Connected! Found {users.Count} users in the database.");
//}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Auth/Login");
        return;
    }
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();