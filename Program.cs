using Microsoft.OpenApi.Models;
using Serilog;
using VideoProjector.Configuration;
using VideoProjector.Data.Repositories.Implementations;
using VideoProjector.Data.Repositories.Interfaces;
using VideoProjector.Middleware;
using VideoProjector.Services.Impelements;
using VideoProjector.Services.Impelements.Account;
using VideoProjector.Services.Implementations;
using VideoProjector.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add Swagger services
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "VideoProjector.xml");
    options.IncludeXmlComments(xmlFile);
});


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
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

app.UseStaticFiles();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();  // Access Swagger UI at /swagger

// Use custom error handling middleware
app.UseMiddleware<ErrorHandling>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();