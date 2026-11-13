namespace Ter.Api.Models.Constants;

public static class Roles
{
    public const string Etudiant = "Etudiant";
    public const string AgentCrous = "AgentCrous";
    public const string Administrateur = "Administrateur";

    public static readonly string[] Tous = [Etudiant, AgentCrous, Administrateur];
}