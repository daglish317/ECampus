using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AjouterVersionnementProgrammesHebdomadaires : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProgrammeHebdomadaireId",
                table: "Seances",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClasseId",
                table: "ProgrammesHebdomadaires",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateDebutSemaine",
                table: "ProgrammesHebdomadaires",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePublication",
                table: "ProgrammesHebdomadaires",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumeroVersion",
                table: "ProgrammesHebdomadaires",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId",
                table: "ProgrammesHebdomadaires",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires",
                column: "ProgrammePrecedentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgrammesHebdomadaires_Classes_ClasseId",
                table: "ProgrammesHebdomadaires",
                column: "ClasseId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgrammesHebdomadaires_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires",
                column: "ProgrammePrecedentId",
                principalTable: "ProgrammesHebdomadaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgrammesHebdomadaires_Classes_ClasseId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgrammesHebdomadaires_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropIndex(
                name: "IX_ProgrammesHebdomadaires_ClasseId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropIndex(
                name: "IX_ProgrammesHebdomadaires_ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropColumn(
                name: "ClasseId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropColumn(
                name: "DateDebutSemaine",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropColumn(
                name: "DatePublication",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropColumn(
                name: "NumeroVersion",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.DropColumn(
                name: "ProgrammePrecedentId",
                table: "ProgrammesHebdomadaires");

            migrationBuilder.AlterColumn<int>(
                name: "ProgrammeHebdomadaireId",
                table: "Seances",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
