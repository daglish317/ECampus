using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class SimplifierLienSeanceProgrammeAcademique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seances_ProgrammesAcademiques_ProgrammeAcademiqueId",
                table: "Seances");

            migrationBuilder.DropIndex(
                name: "IX_Seances_ProgrammeAcademiqueId",
                table: "Seances");

            migrationBuilder.DropColumn(
                name: "ProgrammeAcademiqueId",
                table: "Seances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgrammeAcademiqueId",
                table: "Seances",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Seances_ProgrammeAcademiqueId",
                table: "Seances",
                column: "ProgrammeAcademiqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seances_ProgrammesAcademiques_ProgrammeAcademiqueId",
                table: "Seances",
                column: "ProgrammeAcademiqueId",
                principalTable: "ProgrammesAcademiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
