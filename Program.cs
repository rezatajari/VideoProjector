using Serilog;
using VideoProjector.Configuration;
using VideoProjector.Data.Repositories.Implementations;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.Middleware;
using VideoProjector.Services.Impelements;
using VideoProjector.Services.Impelements.Account;
using VideoProjector.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddLogging();

// =========================== Configuration Folder =========================== //

// --------------------------- Database configure services --------------------------- //
builder.Services.ConfigureDbContext(builder.Configuration);
// --------------------------- Identity configure --------------------------- //
builder.Services.ConfigureIdentity();
// --------------------------- Jwt configure --------------------------- //
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<JwtTokenService>();
// --------------------------- Logger configure --------------------------- //
LoggerConfig.ConfigureLogger(builder.Configuration);
builder.Host.UseSerilog(); // Replace default .NET logger with Serilog
// --------------------------- Email configure --------------------------- //
builder.Services.ConfigureEmail();

// =========================== Configuration Folder =========================== //

// -------- Account -------- //
builder.Services.AddScoped<IAccountService, AccountService>();
// -------- Profile -------- //
builder.Services.AddScoped<IProfileService, ProfileService>();
// -------- Product -------- //
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// -------- Order -------- //
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

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

