using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using BulkyWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bulky.Utility;
using System.Linq;
//ctrl+shift+f     ----->it is used to replace
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
    options.SignIn.RequireConfirmedAccount = true);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, Email_Sender>();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[]
    {
        SD.Role_Customer,
        SD.Role_Employee,
        SD.Role_Admin,
        SD.Role_Company
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Unable to create the '{role}' role: {string.Join("; ", roleResult.Errors.Select(error => error.Description))}");
            }
        }
    }

    if (app.Environment.IsDevelopment())
    {
        var adminEmail = app.Configuration["AdminAccount:Email"];
        var adminPassword = app.Configuration["AdminAccount:Password"];
        var adminConfigured = !string.IsNullOrWhiteSpace(adminEmail) || !string.IsNullOrWhiteSpace(adminPassword);

        if (adminConfigured && (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword)))
        {
            throw new InvalidOperationException(
                "Both AdminAccount:Email and AdminAccount:Password must be configured to seed a development admin account.");
        }

        if (adminConfigured)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminUser = await userManager.FindByEmailAsync(adminEmail!);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var userResult = await userManager.CreateAsync(adminUser, adminPassword!);
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unable to create the development admin account: {string.Join("; ", userResult.Errors.Select(error => error.Description))}");
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, SD.Role_Admin))
            {
                var roleResult = await userManager.AddToRoleAsync(adminUser, SD.Role_Admin);
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Unable to assign the Admin role to the configured development account: {string.Join("; ", roleResult.Errors.Select(error => error.Description))}");
                }
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Customer/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();

app.MapGet("/", () => Results.Redirect("/Customer/Home/Index"));

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
