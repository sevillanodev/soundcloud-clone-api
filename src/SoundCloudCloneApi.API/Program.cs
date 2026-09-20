using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using SoundCloudCloneApi.API.Middleware;
using SoundCloudCloneApi.Application.Common.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SoundCloudCloneApi.Infrastructure.Persistence.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Configuración fuertemente tipada de JWT ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

// --- Controllers ---
builder.Services.AddControllers();

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// --- Autenticación JWT ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

builder.Services.AddAuthorization();
// --- Inyección de dependencias: Repositorios y Servicios ---
builder.Services.AddScoped<SoundCloudCloneApi.Domain.Interfaces.IUserRepository, SoundCloudCloneApi.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.IPasswordHasher, SoundCloudCloneApi.Infrastructure.Services.PasswordHasher>();
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.IJwtTokenGenerator, SoundCloudCloneApi.Infrastructure.Services.JwtTokenGenerator>();
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.IAuthService, SoundCloudCloneApi.Application.Services.AuthService>();

// --- FluentValidation ---
builder.Services.AddValidatorsFromAssemblyContaining<SoundCloudCloneApi.Application.Validators.Auth.RegisterRequestValidator>();

// --- Swagger con soporte para JWT Bearer y Multipart/Form-Data ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SoundCloud Clone API",
        Version = "v1",
        Description = "Backend Web API estilo SoundCloud — Clean Architecture, .NET 8, JWT, Audio Streaming."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT así: Bearer {tu token}"
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

builder.Services.Configure<SoundCloudCloneApi.Application.Common.Models.FileStorageSettings>(builder.Configuration.GetSection("FileStorage"));
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.IFileStorageService, SoundCloudCloneApi.Infrastructure.Services.LocalFileStorageService>();
builder.Services.AddScoped<SoundCloudCloneApi.Domain.Interfaces.ITrackRepository, SoundCloudCloneApi.Infrastructure.Repositories.TrackRepository>();
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.ITrackService, SoundCloudCloneApi.Application.Services.TrackService>();
builder.Services.AddScoped<SoundCloudCloneApi.Domain.Interfaces.ILikeRepository, SoundCloudCloneApi.Infrastructure.Repositories.LikeRepository>();
builder.Services.AddScoped<SoundCloudCloneApi.Domain.Interfaces.IFollowRepository, SoundCloudCloneApi.Infrastructure.Repositories.FollowRepository>();
builder.Services.AddScoped<SoundCloudCloneApi.Domain.Interfaces.IPlaylistRepository, SoundCloudCloneApi.Infrastructure.Repositories.PlaylistRepository>();
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.ILikeService, SoundCloudCloneApi.Application.Services.LikeService>();
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.IFollowService, SoundCloudCloneApi.Application.Services.FollowService>();
builder.Services.AddScoped<SoundCloudCloneApi.Application.Interfaces.IPlaylistService, SoundCloudCloneApi.Application.Services.PlaylistService>();

var app = builder.Build();

// --- Middleware pipeline ---
app.UseGlobalExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SoundCloud Clone API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseStaticFiles(); // necesario para servir wwwroot/uploads más adelante

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();