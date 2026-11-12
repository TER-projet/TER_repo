using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ter.Api.Data;

namespace Ter.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController(AppDbContext db) : ControllerBase
{
    // GET /health : vérifie que l'API tourne ET qu'elle joint la base de données
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var baseJoignable = await db.Database.CanConnectAsync(ct);

        return baseJoignable
            ? Ok(new { status = "healthy", database = "up" })
            : StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = "unhealthy", database = "down" });
    }
}