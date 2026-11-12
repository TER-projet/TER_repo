using Microsoft.AspNetCore.Identity;

namespace Ter.Api.Models.Entities;

/// <summary>
/// Utilisateur de l'application (étudiant, agent du CROUS ou administrateur).
/// Hérite d'IdentityUser : e-mail, mot de passe haché, etc. Ajoutez ici les champs propres au projet
/// (prénom, nom…) si un ticket le demande.
/// </summary>
public class AppUser : IdentityUser<Guid>
{
}