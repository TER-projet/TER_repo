using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ter.Api.Extensions;
using Xunit;

namespace Ter.Tests.Extensions;

public class PasswordRulesTests
{
    // Récupère les réglages du mot de passe faits dans IdentityExtensions
    private static PasswordOptions GetPasswordOptions()
    {
        var services = new ServiceCollection();
        services.AddIdentityServices();
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IOptions<IdentityOptions>>().Value.Password;
    }

    [Fact]
    public void LongueurMinimum_Est12()
    {
        Assert.Equal(12, GetPasswordOptions().RequiredLength);
    }

    [Fact]
    public void Majuscule_EstObligatoire()
    {
        Assert.True(GetPasswordOptions().RequireUppercase);
    }

    [Fact]
    public void Minuscule_EstObligatoire()
    {
        Assert.True(GetPasswordOptions().RequireLowercase);
    }

    [Fact]
    public void Chiffre_EstObligatoire()
    {
        Assert.True(GetPasswordOptions().RequireDigit);
    }

    [Fact]
    public void CaractereSpecial_EstObligatoire()
    {
        Assert.True(GetPasswordOptions().RequireNonAlphanumeric);
    }
}