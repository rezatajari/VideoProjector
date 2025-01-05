using VideoProjector.Configuration.Db;
using VideoProjector.Configuration.IdentityLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// =========================== Configuration Folder =========================== //

// --------------------------- Database configure services --------------------------- //
builder.Services.ConfigureDbContext(builder.Configuration);
// --------------------------- Identity configure --------------------------- //
builder.Services.ConfigureIdentity();

// =========================== Configuration Folder =========================== //

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


