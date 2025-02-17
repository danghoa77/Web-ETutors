using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using eTutoring.Data;
using eTutoring.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IMeetingRecordRepository, MeetingRecordRepository>();
builder.Services.AddScoped<ITutorRequestRepository, TutorRequestRepository>();

// Register DbContext as a service
builder.Services.AddScoped<DbContext>(provider => provider.GetService<ApplicationDbContext>()!);

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
