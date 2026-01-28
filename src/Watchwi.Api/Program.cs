using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Watchwi.Application.IProviders.Cloudinary;
using Watchwi.Application.IProviders.Security;
using Watchwi.Application.Services.AuthService;
using Watchwi.Application.Services.MediaService;
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMediaService, MediaService>();


// =============================================================
// CONTROLLERS
// =============================================================
builder.Services.AddControllers();

// =============================================================
// SWAGGER/OPENAPI
// =============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WatchWi API",
        Version = "v1",
        Description = "API for WatchWi platform"
    });

    // 🔐 JWT Bearer definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like this: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =============================================================
// JWT / SECURITY
// =============================================================
builder.Services.AddOptions<JwtSettings>().Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
    .Validate(s =>
            !string.IsNullOrWhiteSpace(s.Key) &&
            !string.IsNullOrWhiteSpace(s.Issuer) &&
            !string.IsNullOrWhiteSpace(s.Audience) &&
            s.AccessTokenMinutes > 0,
        "Invalid JwtSettings")
    .ValidateOnStart();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection(JwtSettings.SectionName);

        var issuer = jwtSection.GetValue<string>("Issuer")
                     ?? throw new InvalidOperationException("Jwt:Issuer not configured");

        var audience = jwtSection.GetValue<string>("Audience")
                       ?? throw new InvalidOperationException("Jwt:Audience not configured");

        var key = jwtSection.GetValue<string>("Key")
                  ?? throw new InvalidOperationException("Jwt:Key not configured");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// =============================================================
// CLOUDINARY
// =============================================================
builder.Services
    .AddOptions<CloudinarySettings>()
    .Bind(builder.Configuration.GetSection(CloudinarySettings.SectionName))
    .Validate(s =>
            !string.IsNullOrWhiteSpace(s.CloudName) &&
            !string.IsNullOrWhiteSpace(s.ApiKey) &&
            !string.IsNullOrWhiteSpace(s.ApiSecret),
        "Invalid CloudinarySettings")
    .ValidateOnStart();

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