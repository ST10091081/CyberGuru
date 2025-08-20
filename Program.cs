using CyberGamify.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CyberGamify.Models;

var builder = WebApplication.CreateBuilder(args);

// Add logging and config (optional, but best practice for diagnostics)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Get connection string, fallback to SQLite for dev/hackathon
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=cybergamify.db";

// Register DbContext with SQLite provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Configure Identity with default UI and EF stores
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Use environment variable for email confirmation in prod
    options.SignIn.RequireConfirmedAccount = !builder.Environment.IsDevelopment();
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add Razor Pages
builder.Services.AddRazorPages();

// Optional: Add health checks for monitoring
// builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure error handling and security for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // In development, detailed errors and no HSTS
    app.UseDeveloperExceptionPage();
}

// Only redirect to HTTPS outside development (optional)
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

// Optional: health check endpoint
// app.MapHealthChecks("/health");

app.Run();