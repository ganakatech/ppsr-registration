using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;
using PpsrRegistration.Data;
using PpsrRegistration.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(config.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CsvProcessor>();
builder.Services.AddValidatorsFromAssemblyContaining<VehicleRegistrationValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAntiforgery();

app.MapPost("/upload", async (IFormFile file, CsvProcessor processor, [FromServices] IAntiforgery antiforgery, HttpContext context) =>
{
    try
    {
        //await antiforgery.ValidateRequestAsync(context);

        if (file == null || file.Length == 0)
            return Results.BadRequest("File is empty");

        var summary = await processor.ProcessAsync(file.OpenReadStream());
        return Results.Ok(summary);
    }
    catch (AntiforgeryValidationException)
    {
        return Results.BadRequest("Invalid antiforgery token");
    }
})
.DisableAntiforgery(); // Disable the default antiforgery validation since we're handling it manually

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();