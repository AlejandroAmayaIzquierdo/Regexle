using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WebServer.Data;
using WebServer.Endpoints;
using WebServer.Middlewares;
using WebServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

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
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JWTHandler>();
builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapAuthEndpoints();
app.MapChallengeEndpoints();

app.Run();
