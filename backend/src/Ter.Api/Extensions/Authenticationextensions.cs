using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Ter.Api.Models.Constants;
using Ter.Api.Settings;

namespace Ter.Api.Extensions;

public static class AuthenticationExtensions
{
    /// <summary>Configure l'authentification par jeton JWT et les politiques d'autorisation par rôle.</summary>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Paramètres JWT validés au démarrage
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Schéma d'authentification : "Authorization: Bearer <jeton>"
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((bearer, jwt) =>
                bearer.TokenValidationParameters = jwt.Value.ToValidationParameters());

        // Politiques par rôle
        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.Administrateur, policy => policy.RequireRole(Roles.Administrateur))
            .AddPolicy(Policies.Personnel, policy => policy.RequireRole(Roles.AgentCrous, Roles.Administrateur))
            .AddPolicy(Policies.Etudiant, policy => policy.RequireRole(Roles.Etudiant));

        return services;
    }
}