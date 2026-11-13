using System.Linq;

namespace Ter.Domain.Security;

public static class PasswordPolicy
{
    public static List<string> Validate(string password)
    {
        var errors = new List<string>();

        // Règle 1 : au moins 12 caractères
        if (password.Length < 12)
        {
            errors.Add("Le mot de passe doit faire au moins 12 caractères");
        }

        // Règle 2 : au maximum 64 caractères
        if (password.Length > 64)
        {
            errors.Add("Le mot de passe doit faire au maximum 64 caractères");
        }

        // Règle 3 : au moins une majuscule
        if (!password.Any(char.IsUpper))
        {
            errors.Add("Le mot de passe doit contenir au moins une majuscule");
        }

        // Règle 4 : au moins une minuscule
        if (!password.Any(char.IsLower))
        {
            errors.Add("Le mot de passe doit contenir au moins une minuscule");
        }

        // Règle 5 : au moins un chiffre
        if (!password.Any(char.IsDigit))
        {
            errors.Add("Le mot de passe doit contenir au moins un chiffre");
        }

        // Règle 6 : au moins un caractère spécial
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
        {
            errors.Add("Le mot de passe doit contenir au moins un caractère spécial (! @ # - ...)");
        }

        // Règle 7 : pas d'espace au début ni à la fin
        if (password != password.Trim())
        {
            errors.Add("Le mot de passe ne doit pas commencer ou finir par un espace");
        }

        return errors;
    }
}