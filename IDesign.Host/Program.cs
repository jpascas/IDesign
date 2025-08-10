using IDesign.Access;
using IDesign.Host.Exceptions;
using IDesign.Manager;
using Microsoft.EntityFrameworkCore;
using Mapster;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration
    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Add services
builder.Services.AddDbContext<DesignDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMapster();

// add managers and access layers
builder.Services.AddManagers();
builder.Services.AddAccesses();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseExceptionHandler(options => { });
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// create database and/or execute migrations if needed
app.InitializeDatabase();

app.Run();

