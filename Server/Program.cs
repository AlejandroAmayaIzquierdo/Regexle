using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WebServer.Data;
using WebServer.Middlewares;
using WebServer.Models.Auth;
using WebServer.Services;
using WebServer.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new RoutePrefixConvention("api"));
});

// TODO change to use open telemetry
builder.Logging.AddConsole();

builder.Services.AddOpenApi();

string? connectionString = builder.Configuration.GetConnectionString("Mysql");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWTSecurity:Issuer"],
            ValidAudience = builder.Configuration["JWTSecurity:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWTSecurity:Token"]!)
            ),
        };
    });

builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy("UserAccess", policy => policy.RequirePermissions(PermissionTypes.UserAccess))
    .AddPolicy("AdminAccess", policy => policy.RequirePermissions(PermissionTypes.AdminAccess))
    .AddPolicy(
        "ModeratorAccess",
        policy =>
            policy.RequirePermissions(
                PermissionTypes.CreateChallenge,
                PermissionTypes.EditChallenge,
                PermissionTypes.DeleteChallenge,
                PermissionTypes.ViewFutureChallenges
            )
    );

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "https://daily-regex.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddHttpContextAccessor();

// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JWTHandler>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ChallengeService>();

builder.Services.AddTransient<UserContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
