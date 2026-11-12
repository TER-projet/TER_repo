using Microsoft.AspNetCore.Identity;
using Ter.Api.Data;
using Ter.Api.Models.Entities;

namespace Ter.Api.Extensions;

public static class IdentityExtensions
{
    /// <summary>Enregistre ASP.NET Core Identity (utilisateurs + rôles) branché sur AppDbContext.</summary>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services
            .AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                // Les règles de mot de passe (longueur, majuscule, chiffre, caractère spécial)
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}