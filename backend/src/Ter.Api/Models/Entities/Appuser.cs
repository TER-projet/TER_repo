using Microsoft.AspNetCore.Identity;

namespace Ter.Api.Models.Entities;

/// <summary>
/// Compte utilisateur de l'application (étudiant, agent du CROUS ou administrateur).
/// Hérite d'IdentityUser : e-mail, mot de passe haché, etc. Le rôle est géré par Identity.
/// Ajoutez ici les champs propres au projet (prénom, nom…) si un ticket le demande.
/// </summary>
public class AppUser : IdentityUser<Guid>
{
    /// <summary>Date de création du compte, en UTC (colonne : date_creation).</summary>
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    /// <summary>État du compte (colonne : statut_compte).</summary>
    public StatutCompte Statut { get; set; } = StatutCompte.Actif;

    /// <summary>Profil étudiant associé, s'il existe (one-to-one).</summary>
    public Etudiant? Etudiant { get; set; }

    /// <summary>Profil personnel associé, s'il existe (one-to-one).</summary>
    public Personnel? Personnel { get; set; }

    /// <summary>Profil agent du CROUS associé, s'il existe (one-to-one).</summary>
    public AgentCrous? AgentCrous { get; set; }

    /// <summary>Profil administrateur associé, s'il existe (one-to-one).</summary>
    public Administrateur? Administrateur { get; set; }

    /// <summary>Associations « administrer » : administrateurs qui gèrent ce compte (one-to-many).</summary>
    public ICollection<Administrer> AdministresPar { get; set; } = new List<Administrer>();

    /// <summary>Sessions de connexion du compte (one-to-many).</summary>
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}