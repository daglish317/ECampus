using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AjouterUniciteProgrammePublie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId_AnneeAcademiqueId_DateDebutSemaine",
                table: "ProgrammesHebdomadaires",
                columns: new[] { "ClasseId", "AnneeAcademiqueId", "DateDebutSemaine" },
                unique: true,
                filter: "\"Statut\" = 'Publie'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId_AnneeAcademiqueId_DateDebutSemaine",
                table: "ProgrammesHebdomadaires");
        }
    }
}
