namespace Ter.Api.Models.Constants;

/// <summary>
/// Politiques d'autorisation. Usage : [Authorize(Policy = Policies.Administrateur)]
/// </summary>
public static class Policies
{
    /// <summary>Réservé aux administrateurs.</summary>
    public const string Administrateur = "Administrateur";

    /// <summary>Agents du CROUS ET administrateurs (personnel du restaurant).</summary>
    public const string Personnel = "Personnel";

    /// <summary>Réservé aux étudiants.</summary>
    public const string Etudiant = "Etudiant";
}