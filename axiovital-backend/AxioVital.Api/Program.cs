using System.Text;
using AxioVital.Api.Middleware;
using AxioVital.Application;
using AxioVital.Infrastructure;
using AxioVital.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ──────────────────────────────── Serilog ────────────────────────────────
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithThreadId()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} | {Message:lj}{NewLine}{Exception}")
        .WriteTo.File("logs/axiovital-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30);
});

// ──────────────────────────────── Services ────────────────────────────────

// Application & Infrastructure layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AxioVital API",
        Version = "v1",
        Description = "AxioVital Healthcare Platform REST API"
    });
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings?.Secret ?? "DEVELOPMENT_SECRET_KEY_REPLACE_IN_PRODUCTION_MIN_32_CHARS!!")),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings?.Issuer ?? "AxioVital",
            ValidateAudience = true,
            ValidAudience = jwtSettings?.Audience ?? "AxioVital.Desktop",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Health Checks
var healthBuilder = builder.Services.AddHealthChecks();

var pgConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(pgConnection))
{
    healthBuilder.AddNpgSql(pgConnection, name: "postgresql");
}

var redisConnection = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnection))
{
    healthBuilder.AddRedis(redisConnection, name: "redis");
}

// CORS (development only)
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ──────────────────────────────── App Pipeline ────────────────────────────────
var app = builder.Build();

// Global exception handler
app.UseMiddleware<GlobalExceptionMiddleware>();

// Correlation ID
app.UseMiddleware<CorrelationIdMiddleware>();

// Request logging
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? "localhost");
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString() ?? "Unknown");
    };
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AxioVital API v1"));
    app.UseCors("Development");
}

app.UseAuthentication();

// Tenant resolution
app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthorization();

app.MapControllers();

// Health endpoint
app.MapHealthChecks("/health");

app.Run();

// Required for integration testing with WebApplicationFactory
namespace AxioVital.Api
{
    public partial class Program { }
}
