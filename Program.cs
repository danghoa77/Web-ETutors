using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eTutoring.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);

var app = builder.Build();

// Run migrations automatically with retry logic
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var maxAttempts = 10;
    var attempts = 0;
    var migrated = false;

    while (!migrated && attempts < maxAttempts)
    {
        try
        {
            dbContext.Database.Migrate();
            migrated = true;
        }
        catch (Exception ex)
        {
            attempts++;
            Console.WriteLine($"Migration attempt {attempts} failed: {ex.Message}");
            Thread.Sleep(5000); // wait for 5 seconds before retrying
        }
    }

    if (!migrated)
    {
        Console.WriteLine("Database migration failed after multiple attempts.");
        throw new Exception("Database migration failed.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
