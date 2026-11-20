using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddETextBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CahiersTexte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProgrammeAcademiqueId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Actif = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CahiersTexte", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CahiersTexte_ProgrammesAcademiques_ProgrammeAcademiqueId",
                        column: x => x.ProgrammeAcademiqueId,
                        principalTable: "ProgrammesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntreesCahier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CahierTexteId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeanceId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnseignantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContenuEnseigne = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    ActivitesRealisees = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    ActivitesPrevues = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    DateSoumission = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntreesCahier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntreesCahier_CahiersTexte_CahierTexteId",
                        column: x => x.CahierTexteId,
                        principalTable: "CahiersTexte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntreesCahier_Enseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "Enseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntreesCahier_Seances_SeanceId",
                        column: x => x.SeanceId,
                        principalTable: "Seances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DecisionsCahier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntreeCahierId = table.Column<int>(type: "INTEGER", nullable: false),
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: false),
                    Decision = table.Column<int>(type: "INTEGER", nullable: false),
                    Commentaire = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    DateDecision = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionsCahier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionsCahier_EntreesCahier_EntreeCahierId",
                        column: x => x.EntreeCahierId,
                        principalTable: "EntreesCahier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DecisionsCahier_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CahiersTexte_ProgrammeAcademiqueId",
                table: "CahiersTexte",
                column: "ProgrammeAcademiqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecisionsCahier_EntreeCahierId",
                table: "DecisionsCahier",
                column: "EntreeCahierId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionsCahier_UtilisateurId",
                table: "DecisionsCahier",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_EntreesCahier_CahierTexteId",
                table: "EntreesCahier",
                column: "CahierTexteId");

            migrationBuilder.CreateIndex(
                name: "IX_EntreesCahier_EnseignantId",
                table: "EntreesCahier",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_EntreesCahier_SeanceId",
                table: "EntreesCahier",
                column: "SeanceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionsCahier");

            migrationBuilder.DropTable(
                name: "EntreesCahier");

            migrationBuilder.DropTable(
                name: "CahiersTexte");
        }
    }
}
