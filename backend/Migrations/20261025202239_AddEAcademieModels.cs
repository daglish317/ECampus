using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddEAcademieModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnneesAcademiques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    DateDebut = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    EstActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnneesAcademiques", x => x.Id);
                    table.CheckConstraint("CK_AnneeAcademique_Dates", "\"DateFin\" >= \"DateDebut\"");
                });

            migrationBuilder.CreateTable(
                name: "Matieres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matieres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Niveaux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Niveaux", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClerkUserId = table.Column<string>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammesHebdomadaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnneeAcademiqueId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomFichier = table.Column<string>(type: "TEXT", nullable: false),
                    DateImportation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Statut = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammesHebdomadaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgrammesHebdomadaires_AnneesAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalTable: "AnneesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Semestres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    DateDebut = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    AnneeAcademiqueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semestres", x => x.Id);
                    table.CheckConstraint("CK_Semestre_Dates", "\"DateFin\" >= \"DateDebut\"");
                    table.ForeignKey(
                        name: "FK_Semestres_AnneesAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalTable: "AnneesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    NiveauId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Niveaux_NiveauId",
                        column: x => x.NiveauId,
                        principalTable: "Niveaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enseignants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enseignants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enseignants_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Etudiants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etudiants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etudiants_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammesAcademiques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatiereId = table.Column<int>(type: "INTEGER", nullable: false),
                    NiveauId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnneeAcademiqueId = table.Column<int>(type: "INTEGER", nullable: false),
                    SemestreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammesAcademiques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgrammesAcademiques_AnneesAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalTable: "AnneesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgrammesAcademiques_Matieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "Matieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgrammesAcademiques_Niveaux_NiveauId",
                        column: x => x.NiveauId,
                        principalTable: "Niveaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgrammesAcademiques_Semestres_SemestreId",
                        column: x => x.SemestreId,
                        principalTable: "Semestres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InscriptionsAcademiques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EtudiantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClasseId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnneeAcademiqueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscriptionsAcademiques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscriptionsAcademiques_AnneesAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalTable: "AnneesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InscriptionsAcademiques_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InscriptionsAcademiques_Etudiants_EtudiantId",
                        column: x => x.EtudiantId,
                        principalTable: "Etudiants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AffectationsEnseignants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnseignantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProgrammeAcademiqueId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateDebut = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffectationsEnseignants", x => x.Id);
                    table.CheckConstraint("CK_AffectationEnseignant_Dates", "\"DateFin\" IS NULL OR \"DateFin\" >= \"DateDebut\"");
                    table.ForeignKey(
                        name: "FK_AffectationsEnseignants_Enseignants_EnseignantId",
                        column: x => x.EnseignantId,
                        principalTable: "Enseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AffectationsEnseignants_ProgrammesAcademiques_ProgrammeAcademiqueId",
                        column: x => x.ProgrammeAcademiqueId,
                        principalTable: "ProgrammesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    HeureDebut = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    DureeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Salle = table.Column<string>(type: "TEXT", nullable: false),
                    ProgrammeAcademiqueId = table.Column<int>(type: "INTEGER", nullable: false),
                    AffectationEnseignantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProgrammeHebdomadaireId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seances", x => x.Id);
                    table.CheckConstraint("CK_Seance_DureePositive", "\"DureeMinutes\" > 0");
                    table.ForeignKey(
                        name: "FK_Seances_AffectationsEnseignants_AffectationEnseignantId",
                        column: x => x.AffectationEnseignantId,
                        principalTable: "AffectationsEnseignants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seances_ProgrammesAcademiques_ProgrammeAcademiqueId",
                        column: x => x.ProgrammeAcademiqueId,
                        principalTable: "ProgrammesAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seances_ProgrammesHebdomadaires_ProgrammeHebdomadaireId",
                        column: x => x.ProgrammeHebdomadaireId,
                        principalTable: "ProgrammesHebdomadaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AffectationsEnseignants_EnseignantId",
                table: "AffectationsEnseignants",
                column: "EnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_AffectationsEnseignants_ProgrammeAcademiqueId",
                table: "AffectationsEnseignants",
                column: "ProgrammeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_AnneesAcademiques_EstActive",
                table: "AnneesAcademiques",
                column: "EstActive",
                unique: true,
                filter: "\"EstActive\" = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AnneesAcademiques_Nom",
                table: "AnneesAcademiques",
                column: "Nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_NiveauId",
                table: "Classes",
                column: "NiveauId");

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_UtilisateurId",
                table: "Enseignants",
                column: "UtilisateurId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Etudiants_UtilisateurId",
                table: "Etudiants",
                column: "UtilisateurId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InscriptionsAcademiques_AnneeAcademiqueId",
                table: "InscriptionsAcademiques",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_InscriptionsAcademiques_ClasseId",
                table: "InscriptionsAcademiques",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_InscriptionsAcademiques_EtudiantId_AnneeAcademiqueId",
                table: "InscriptionsAcademiques",
                columns: new[] { "EtudiantId", "AnneeAcademiqueId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesAcademiques_AnneeAcademiqueId",
                table: "ProgrammesAcademiques",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesAcademiques_MatiereId_NiveauId_AnneeAcademiqueId_SemestreId",
                table: "ProgrammesAcademiques",
                columns: new[] { "MatiereId", "NiveauId", "AnneeAcademiqueId", "SemestreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesAcademiques_NiveauId",
                table: "ProgrammesAcademiques",
                column: "NiveauId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesAcademiques_SemestreId",
                table: "ProgrammesAcademiques",
                column: "SemestreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_AnneeAcademiqueId",
                table: "ProgrammesHebdomadaires",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Seances_AffectationEnseignantId",
                table: "Seances",
                column: "AffectationEnseignantId");

            migrationBuilder.CreateIndex(
                name: "IX_Seances_ProgrammeAcademiqueId",
                table: "Seances",
                column: "ProgrammeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Seances_ProgrammeHebdomadaireId",
                table: "Seances",
                column: "ProgrammeHebdomadaireId");

            migrationBuilder.CreateIndex(
                name: "IX_Semestres_AnneeAcademiqueId",
                table: "Semestres",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_ClerkUserId",
                table: "Utilisateurs",
                column: "ClerkUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InscriptionsAcademiques");

            migrationBuilder.DropTable(
                name: "Seances");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Etudiants");

            migrationBuilder.DropTable(
                name: "AffectationsEnseignants");

            migrationBuilder.DropTable(
                name: "ProgrammesHebdomadaires");

            migrationBuilder.DropTable(
                name: "Enseignants");

            migrationBuilder.DropTable(
                name: "ProgrammesAcademiques");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Matieres");

            migrationBuilder.DropTable(
                name: "Niveaux");

            migrationBuilder.DropTable(
                name: "Semestres");

            migrationBuilder.DropTable(
                name: "AnneesAcademiques");
        }
    }
}
