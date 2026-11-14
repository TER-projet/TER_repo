using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ter.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class S1_DB_CreateProfilsEtSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agent_crous",
                columns: table => new
                {
                    id_agent_crous = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    id_compte = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_crous", x => x.id_agent_crous);
                    table.ForeignKey(
                        name: "FK_agent_crous_comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "comptes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "etudiant",
                columns: table => new
                {
                    id_etudiant = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    est_boursier = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    classe = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    id_compte = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etudiant", x => x.id_etudiant);
                    table.ForeignKey(
                        name: "FK_etudiant_comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "comptes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personnel",
                columns: table => new
                {
                    id_personnel = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fonction = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    id_compte = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personnel", x => x.id_personnel);
                    table.ForeignKey(
                        name: "FK_personnel_comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "comptes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    id_session = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_debut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    date_expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    id_compte = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.id_session);
                    table.ForeignKey(
                        name: "FK_session_comptes_id_compte",
                        column: x => x.id_compte,
                        principalTable: "comptes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_agent_crous_id_compte",
                table: "agent_crous",
                column: "id_compte",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_etudiant_id_compte",
                table: "etudiant",
                column: "id_compte",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personnel_id_compte",
                table: "personnel",
                column: "id_compte",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_session_id_compte",
                table: "session",
                column: "id_compte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agent_crous");

            migrationBuilder.DropTable(
                name: "etudiant");

            migrationBuilder.DropTable(
                name: "personnel");

            migrationBuilder.DropTable(
                name: "session");
        }
    }
}
