using PJ_P_Installation_Management_System.Data;
using PJ_P_Installation_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using PJ_P_Installation_Management_System.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register DbContext
builder.Services.AddDbContext<PJInstallationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.


// Add logging if not already present
builder.Services.AddLogging();

// Add session services
builder.Services.AddSession();

var app = builder.Build();

// Enable session
app.UseSession();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

// Map static files and routes
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
