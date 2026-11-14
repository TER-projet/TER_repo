using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ter.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTablePointsDeVente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "points_de_vente",
                columns: table => new
                {
                    id_point_de_vente = table.Column<Guid>(type: "uuid", nullable: false),
                    nom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    adresse = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    latitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    longitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    telephone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    capacite_accueil = table.Column<int>(type: "integer", nullable: true),
                    statut = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_points_de_vente", x => x.id_point_de_vente);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "points_de_vente");
        }
    }
}
