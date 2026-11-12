using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Ter.Api.Models.Constants;
using Ter.Api.Settings;
using Xunit;

namespace Ter.Tests.Settings;

public class JwtOptionsTests
{
    private static JwtOptions Options() => new()
    {
        Key = new string('k', 40),
        Issuer = "ter-api",
        Audience = "ter-client",
        ExpirationMinutes = 60,
    };

    private static string CreerToken(JwtOptions options, string cle, string role, DateTime expire)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Issuer,
            Audience = options.Audience,
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, role) }),
            NotBefore = expire.AddMinutes(-10),
            Expires = expire,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cle)),
                SecurityAlgorithms.HmacSha256),
        };
        return new JsonWebTokenHandler().CreateToken(descriptor);
    }

    [Fact]
    public async Task Token_valide_est_accepte_et_expose_le_role()
    {
        var options = Options();
        var token = CreerToken(options, options.Key, Roles.Administrateur, DateTime.UtcNow.AddMinutes(5));

        var resultat = await new JsonWebTokenHandler().ValidateTokenAsync(token, options.ToValidationParameters());

        Assert.True(resultat.IsValid);
        Assert.True(resultat.ClaimsIdentity.HasClaim(ClaimTypes.Role, Roles.Administrateur));
    }

    [Fact]
    public async Task Token_signe_avec_une_autre_cle_est_refuse()
    {
        var options = Options();
        var token = CreerToken(options, new string('x', 40), Roles.Etudiant, DateTime.UtcNow.AddMinutes(5));

        var resultat = await new JsonWebTokenHandler().ValidateTokenAsync(token, options.ToValidationParameters());

        Assert.False(resultat.IsValid);
    }

    [Fact]
    public async Task Token_expire_est_refuse()
    {
        var options = Options();
        var token = CreerToken(options, options.Key, Roles.Etudiant, DateTime.UtcNow.AddMinutes(-5));

        var resultat = await new JsonWebTokenHandler().ValidateTokenAsync(token, options.ToValidationParameters());

        Assert.False(resultat.IsValid);
    }
}