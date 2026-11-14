namespace Ter.Api.Models.Entities;

/// <summary>
/// Profil d'un membre du personnel, rattaché à un compte utilisateur (table : personnel).
/// </summary>
public class Personnel
{
    /// <summary>Identifiant du personnel (colonne : id_personnel).</summary>
    public int Id { get; set; }

    /// <summary>Nom de famille (colonne : nom).</summary>
    public string Nom { get; set; } = string.Empty;

    /// <summary>Prénom (colonne : prenom).</summary>
    public string Prenom { get; set; } = string.Empty;

    /// <summary>Fonction exercée, facultative (colonne : fonction).</summary>
    public string? Fonction { get; set; }

    /// <summary>Identifiant du compte associé (colonne : id_compte).</summary>
    public Guid CompteId { get; set; }

    /// <summary>Compte utilisateur associé.</summary>
    public AppUser Compte { get; set; } = null!;
}
