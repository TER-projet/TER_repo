
namespace Ter.Api.Models.Entities;

public class PointDeVente
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nom { get; set; } = string.Empty;

    public string Adresse { get; set; } = string.Empty;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Telephone { get; set; }

    public int? CapaciteAccueil { get; set; }

    public string Statut { get; set; } = "Actif";
}
