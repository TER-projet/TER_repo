namespace Ter.Api.Models.Entities;

/// <summary>
/// Session de connexion d'un compte utilisateur (table : session).
/// </summary>
public class Session
{
    /// <summary>Identifiant de la session (colonne : id_session).</summary>
    public int Id { get; set; }

    /// <summary>Date de début de la session, en UTC (colonne : date_debut).</summary>
    public DateTime DateDebut { get; set; } = DateTime.UtcNow;

    /// <summary>Date d'expiration de la session, en UTC (colonne : date_expiration).</summary>
    public DateTime DateExpiration { get; set; }

    /// <summary>Identifiant du compte propriétaire (colonne : id_compte).</summary>
    public Guid CompteId { get; set; }

    /// <summary>Compte utilisateur propriétaire de la session.</summary>
    public AppUser Compte { get; set; } = null!;
}
