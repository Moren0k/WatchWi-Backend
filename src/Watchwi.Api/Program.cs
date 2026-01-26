using Microsoft.EntityFrameworkCore;
using Watchwi.Infrastructure.Persistence;

// =============================================================
// BUILD WEB APPLICATION
// =============================================================
var builder = WebApplication.CreateBuilder(args);

// =============================================================
// CONFIG: DATABASE
// =============================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// =============================================================
// DEPENDENCY INJECTION: REPOSITORIES
// =============================================================

// =============================================================
// DEPENDENCY INJECTION: PROVIDERS
// =============================================================

// =============================================================
// DEPENDENCY INJECTION: SERVICES
// =============================================================

// =============================================================
// CONTROLLERS
// =============================================================
builder.Services.AddControllers();

// =============================================================
// SWAGGER/OPENAPI
// =============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================================================
// JWT/SECURITY
// =============================================================

// =============================================================
// CLOUDINARY
// =============================================================

// =============================================================
// CORS
// =============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// =============================================================
// BUILD APP
// =============================================================
var app = builder.Build();

// =============================================================
// DB CONNECTIVITY CHECK AT STARTUP
// =============================================================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        if (db.Database.CanConnect())
        {
            Console.WriteLine("Database connected successfully");
        }
        else
        {
            Console.WriteLine("Database connection failed");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occured: {ex.Message}");
    }
}


// =============================================================
// MIDDLEWARE PIPELINE
// =============================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();