namespace Ter.Api.Models.Entities;

/// <summary>
/// Profil d'un administrateur, rattaché au compte avec lequel il se connecte (table : administrateur).
/// Le rôle (administrateur, super admin) est géré par Identity.
/// </summary>
public class Administrateur
{
    /// <summary>Identifiant de l'administrateur (colonne : id_administrateur).</summary>
    public int Id { get; set; }

    /// <summary>Nom de famille (colonne : nom).</summary>
    public string Nom { get; set; } = string.Empty;

    /// <summary>Prénom (colonne : prenom).</summary>
    public string Prenom { get; set; } = string.Empty;

    /// <summary>Identifiant du compte de connexion (colonne : id_compte).</summary>
    public Guid CompteId { get; set; }

    /// <summary>Compte utilisateur avec lequel l'administrateur se connecte.</summary>
    public AppUser Compte { get; set; } = null!;

    /// <summary>Comptes gérés par cet administrateur (table d'association administrer).</summary>
    public ICollection<Administrer> ComptesAdministres { get; set; } = new List<Administrer>();
}
