using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySchool_System.Data;
using MySchool_System.Models;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<SchoolDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpClient<StudentApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5220/"); // ✅ correct URI with scheme
});


builder.Services.AddHttpClient();
builder.Services.AddHttpClient<StudentApiService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 👇 Role + Admin Seed yahin par karenge
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Roles list
    string[] roles = { "Admin", "Teacher", "Student" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ✅ Default Admin User
    var adminEmail = "Student@myschool.com";
    var adminPassword = "Admin@123"; // ⚠️ production me strong password rakhna

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Student");
        }
    }
}

// Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
