using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Watchwi.Application.IProviders.Cloudinary;
using Watchwi.Application.IProviders.Security;
using Watchwi.Domain.Common.IRepositories;
using Watchwi.Domain.Entities.Categories;
using Watchwi.Domain.Entities.Images;
using Watchwi.Domain.Entities.Medias;
using Watchwi.Domain.Entities.Users;
using Watchwi.Infrastructure.Persistence;
using Watchwi.Infrastructure.Providers.Cloudinary;
using Watchwi.Infrastructure.Providers.Security;
using Watchwi.Infrastructure.Repositories;
using Watchwi.Infrastructure.Repositories.Common;

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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();

// =============================================================
// DEPENDENCY INJECTION: PROVIDERS
// =============================================================
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ICloudinaryProvider, CloudinaryProvider>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

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
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = Environment.GetEnvironmentVariable("Jwt__Key") ??
                        throw new InvalidOperationException("JWT_KEY not configured");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


// =============================================================
// CLOUDINARY
// =============================================================
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(CloudinarySettings.SectionName));

builder.Services.PostConfigure<CloudinarySettings>(settings =>
{
    if (string.IsNullOrWhiteSpace(settings.CloudName))
        throw new InvalidOperationException("Cloudinary CloudName not configured");

    if (string.IsNullOrWhiteSpace(settings.ApiKey))
        throw new InvalidOperationException("Cloudinary ApiKey not configured");

    if (string.IsNullOrWhiteSpace(settings.ApiSecret))
        throw new InvalidOperationException("Cloudinary ApiSecret not configured");
});

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