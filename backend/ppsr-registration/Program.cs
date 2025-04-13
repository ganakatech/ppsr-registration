using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.RateLimiting;
using PpsrRegistration.Data;
using PpsrRegistration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure file upload size limit (25MB)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 25 * 1024 * 1024; // 25MB in bytes
});

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var config = builder.Configuration;
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(config.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CsvProcessor>();
builder.Services.AddValidatorsFromAssemblyContaining<VehicleRegistrationValidator>();
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
app.UseRateLimiter();

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