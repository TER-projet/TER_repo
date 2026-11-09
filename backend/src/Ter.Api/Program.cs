using Microsoft.EntityFrameworkCore;
using Ter.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.MapControllers();

app.MapGet("/health", async (AppDbContext db, CancellationToken ct) =>
{
    var canConnect = await db.Database.CanConnectAsync(ct);

    return canConnect
        ? Results.Ok(new { status = "healthy", database = "up" })
        : Results.Json(new { status = "unhealthy", database = "down" }, statusCode: StatusCodes.Status503ServiceUnavailable);
});

app.Run();