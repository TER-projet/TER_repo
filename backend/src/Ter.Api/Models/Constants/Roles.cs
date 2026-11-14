namespace Ter.Api.Models.Constants;

public static class Roles
{
    public const string Etudiant = "Etudiant";
    public const string Personnel = "Personnel";
    public const string AgentCrous = "AgentCrous";
    public const string Administrateur = "Administrateur";
    public const string SuperAdmin = "SuperAdmin";

    public static readonly string[] Tous = [Etudiant, Personnel, AgentCrous, Administrateur, SuperAdmin];
}
