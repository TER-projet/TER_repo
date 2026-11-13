using Ter.Domain.Security;
using Xunit;

namespace Ter.Domain.Tests.Security;

public class PasswordPolicyTests
{
    // ✅ Un bon mot de passe ne doit avoir AUCUNE erreur
    [Fact]
    public void MotDePasseValide_EstAccepte()
    {
        var errors = PasswordPolicy.Validate("Velo-Rouge#2026");

        Assert.Empty(errors);
    }

    // Règle 1 : au moins 12 caractères
    [Fact]
    public void MotDePasseTropCourt_EstRefuse()
    {
        var errors = PasswordPolicy.Validate("Abc1!");

        Assert.Contains("Le mot de passe doit faire au moins 12 caractères", errors);
    }

    // Règle 2 : au maximum 64 caractères
    [Fact]
    public void MotDePasseTropLong_EstRefuse()
    {
        var password = "Abcdefgh1!" + new string('x', 60); // 70 caractères

        var errors = PasswordPolicy.Validate(password);

        Assert.Contains("Le mot de passe doit faire au maximum 64 caractères", errors);
    }

    // Règle 3 : au moins une majuscule
    [Fact]
    public void SansMajuscule_EstRefuse()
    {
        var errors = PasswordPolicy.Validate("velo-rouge#2026");

        Assert.Contains("Le mot de passe doit contenir au moins une majuscule", errors);
    }

    // Règle 4 : au moins une minuscule
    [Fact]
    public void SansMinuscule_EstRefuse()
    {
        var errors = PasswordPolicy.Validate("VELO-ROUGE#2026");

        Assert.Contains("Le mot de passe doit contenir au moins une minuscule", errors);
    }

    // Règle 5 : au moins un chiffre
    [Fact]
    public void SansChiffre_EstRefuse()
    {
        var errors = PasswordPolicy.Validate("Velo-Rouge#Deux");

        Assert.Contains("Le mot de passe doit contenir au moins un chiffre", errors);
    }

    // Règle 6 : au moins un caractère spécial
    [Fact]
    public void SansCaractereSpecial_EstRefuse()
    {
        var errors = PasswordPolicy.Validate("VeloRouge2026");

        Assert.Contains("Le mot de passe doit contenir au moins un caractère spécial (! @ # - ...)", errors);
    }

    // Règle 7 : pas d'espace au début ni à la fin
    [Fact]
    public void EspaceAuDebut_EstRefuse()
    {
        var errors = PasswordPolicy.Validate(" Velo-Rouge#2026");

        Assert.Contains("Le mot de passe ne doit pas commencer ou finir par un espace", errors);
    }
}