namespace Ter.Api.Models.Entities;

/// <summary>
/// Profil d'un agent du CROUS, rattaché à un compte utilisateur (table : agent_crous).
/// Le rôle est géré par Identity ; le point de vente sera ajouté ultérieurement.
/// </summary>
public class AgentCrous
{
    /// <summary>Identifiant de l'agent (colonne : id_agent_crous).</summary>
    public int Id { get; set; }

    /// <summary>Nom de famille (colonne : nom).</summary>
    public string Nom { get; set; } = string.Empty;

    /// <summary>Prénom (colonne : prenom).</summary>
    public string Prenom { get; set; } = string.Empty;

    /// <summary>Identifiant du compte associé (colonne : id_compte).</summary>
    public Guid CompteId { get; set; }

    /// <summary>Compte utilisateur associé.</summary>
    public AppUser Compte { get; set; } = null!;
}
