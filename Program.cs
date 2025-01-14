using Serilog;
using VideoProjector.Configuration;
using VideoProjector.Middleware;
using VideoProjector.Services.Impelements;
using VideoProjector.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

// =========================== Configuration Folder =========================== //

// --------------------------- Database configure services --------------------------- //
builder.Services.ConfigureDbContext(builder.Configuration);
// --------------------------- Identity configure --------------------------- //
builder.Services.ConfigureIdentity();
// --------------------------- Jwt configure --------------------------- //
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
// --------------------------- Logger configure --------------------------- //
LoggerConfig.ConfigureLogger(builder.Configuration);
builder.Host.UseSerilog(); // Replace default .NET logger with Serilog

// =========================== Configuration Folder =========================== //

var app = builder.Build();

app.UseStaticFiles();

// Use custom error handling middleware
app.UseMiddleware<ErrorHandling>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

