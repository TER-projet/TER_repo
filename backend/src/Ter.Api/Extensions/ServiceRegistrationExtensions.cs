namespace Ter.Api.Extensions;

public static class ServiceRegistrationExtensions
{
    /// <summary>
    /// Enregistre automatiquement chaque classe "XxxService" située dans Services/
    /// qui implémente l'interface "IXxxService" (durée de vie : une instance par requête).
    /// Ainsi, ajouter un service ne demande aucune modification de Program.cs.
    /// </summary>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        var implementations = typeof(Program).Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && t.Name.EndsWith("Service")
                        && t.Namespace is not null
                        && t.Namespace.StartsWith("Ter.Api.Services"));

        foreach (var implementation in implementations)
        {
            var contract = implementation.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{implementation.Name}");

            if (contract is not null)
                services.AddScoped(contract, implementation);
        }

        return services;
    }
}