namespace Ter.Api.Models.Entities;

/// <summary>
/// Association entre un administrateur et un compte qu'il gère (table : administrer).
/// Clé primaire composée (id_administrateur, id_compte).
/// </summary>
public class Administrer
{
    /// <summary>Identifiant de l'administrateur (colonne : id_administrateur).</summary>
    public int AdministrateurId { get; set; }

    /// <summary>Administrateur qui gère le compte.</summary>
    public Administrateur Administrateur { get; set; } = null!;

    /// <summary>Identifiant du compte géré (colonne : id_compte).</summary>
    public Guid CompteId { get; set; }

    /// <summary>Compte géré par l'administrateur.</summary>
    public AppUser Compte { get; set; } = null!;
}
