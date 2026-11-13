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
                options.Password.RequiredLength = 12;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;


            
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}