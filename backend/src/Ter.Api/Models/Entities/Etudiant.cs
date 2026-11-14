namespace Ter.Api.Models.Entities;

/// <summary>
/// Profil étudiant, rattaché à un compte utilisateur (table : etudiant).
/// </summary>
public class Etudiant
{
    /// <summary>Identifiant de l'étudiant (colonne : id_etudiant).</summary>
    public int Id { get; set; }

    /// <summary>Nom de famille (colonne : nom).</summary>
    public string Nom { get; set; } = string.Empty;

    /// <summary>Prénom (colonne : prenom).</summary>
    public string Prenom { get; set; } = string.Empty;

    /// <summary>Indique si l'étudiant est boursier (colonne : est_boursier).</summary>
    public bool EstBoursier { get; set; }

    /// <summary>Classe ou filière de l'étudiant, facultative (colonne : classe).</summary>
    public string? Classe { get; set; }

    /// <summary>Identifiant du compte associé (colonne : id_compte).</summary>
    public Guid CompteId { get; set; }

    /// <summary>Compte utilisateur associé.</summary>
    public AppUser Compte { get; set; } = null!;
}
