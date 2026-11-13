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
}