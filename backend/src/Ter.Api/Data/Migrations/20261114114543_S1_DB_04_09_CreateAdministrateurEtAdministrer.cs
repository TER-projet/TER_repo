using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ter.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class S1_DB_04_09_CreateAdministrateurEtAdministrer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrateur",
                columns: table => new
                {
                    id_administrateur = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    id_compte = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrateur", x => x.id_administrateur);
                    table.ForeignKey(
                        name: "FK_administrateur_comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "comptes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "administrer",
                columns: table => new
                {
                    id_administrateur = table.Column<int>(type: "integer", nullable: false),
                    id_compte = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrer", x => new { x.id_administrateur, x.id_compte });
                    table.ForeignKey(
                        name: "FK_administrer_administrateur_id_administrateur",
                        column: x => x.id_administrateur,
                        principalTable: "administrateur",
                        principalColumn: "id_administrateur",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_administrer_comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "comptes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_administrateur_id_compte",
                table: "administrateur",
                column: "id_compte",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_administrer_id_compte",
                table: "administrer",
                column: "id_compte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrer");

            migrationBuilder.DropTable(
                name: "administrateur");
        }
    }
}
