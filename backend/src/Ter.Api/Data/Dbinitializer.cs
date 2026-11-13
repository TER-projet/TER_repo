using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ter.Api.Models.Constants;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data;


/// Exécuté à chaque démarrage de l'API :
/// 1. applique les migrations en attente ;
/// 2. crée les rôles manquants ;
/// 3. crée le compte administrateur initial s'il n'existe pas (Admin:Email / Admin:Password).
/// Tout est idempotent : relancer l'API ne crée jamais de doublon.

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken ct = default)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");

        await provider.GetRequiredService<AppDbContext>().Database.MigrateAsync(ct);
        await CreerRolesAsync(provider, logger);
        await CreerAdministrateurAsync(provider, logger);
    }

    private static async Task CreerRolesAsync(IServiceProvider provider, ILogger logger)
    {
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach (var role in Roles.Tous)
        {
            if (await roleManager.RoleExistsAsync(role)) continue;

            var resultat = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            if (!resultat.Succeeded)
                throw new InvalidOperationException(
                    $"Création du rôle '{role}' impossible : {string.Join(" ; ", resultat.Errors.Select(e => e.Description))}");

            logger.LogInformation("Rôle créé : {Role}", role);
        }
    }

    private static async Task CreerAdministrateurAsync(IServiceProvider provider, ILogger logger)
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var email = configuration["Admin:Email"];
        var motDePasse = configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(motDePasse))
        {
            logger.LogWarning("Admin:Email ou Admin:Password non défini : aucun compte administrateur créé.");
            return;
        }

        var userManager = provider.GetRequiredService<UserManager<AppUser>>();
        if (await userManager.FindByEmailAsync(email) is not null) return;

        var admin = new AppUser { UserName = email, Email = email, EmailConfirmed = true };

        var resultat = await userManager.CreateAsync(admin, motDePasse);
        if (!resultat.Succeeded)
            throw new InvalidOperationException(
                $"Création de l'administrateur initial impossible : {string.Join(" ; ", resultat.Errors.Select(e => e.Description))}");

        await userManager.AddToRoleAsync(admin, Roles.Administrateur);
        logger.LogInformation("Compte administrateur initial créé : {Email}", email);
    }
}