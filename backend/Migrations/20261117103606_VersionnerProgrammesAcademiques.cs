using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class VersionnerProgrammesAcademiques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgrammesAcademiques_MatiereId_NiveauId_AnneeAcademiqueId_SemestreId",
                table: "ProgrammesAcademiques");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesAcademiques_MatiereId_NiveauId_AnneeAcademiqueId_SemestreId_DateEffet",
                table: "ProgrammesAcademiques",
                columns: new[] { "MatiereId", "NiveauId", "AnneeAcademiqueId", "SemestreId", "DateEffet" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgrammesAcademiques_MatiereId_NiveauId_AnneeAcademiqueId_SemestreId_DateEffet",
                table: "ProgrammesAcademiques");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesAcademiques_MatiereId_NiveauId_AnneeAcademiqueId_SemestreId",
                table: "ProgrammesAcademiques",
                columns: new[] { "MatiereId", "NiveauId", "AnneeAcademiqueId", "SemestreId" },
                unique: true);
        }
    }
}
