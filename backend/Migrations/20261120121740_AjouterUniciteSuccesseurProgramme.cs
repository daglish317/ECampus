using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AjouterUniciteSuccesseurProgramme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires",
                column: "ProgrammePrecedentId",
                unique: true,
                filter: "\"ProgrammePrecedentId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires",
                column: "ProgrammePrecedentId");
        }
    }
}
