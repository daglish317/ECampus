using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AjouterUniciteVersionsProgrammes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId_AnneeAcademiqueId_DateDebutSemaine_NumeroVersion",
                table: "ProgrammesHebdomadaires",
                columns: new[] { "ClasseId", "AnneeAcademiqueId", "DateDebutSemaine", "NumeroVersion" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId_AnneeAcademiqueId_DateDebutSemaine_NumeroVersion",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId",
                table: "ProgrammesHebdomadaires",
                column: "ClasseId");
        }
    }
}
