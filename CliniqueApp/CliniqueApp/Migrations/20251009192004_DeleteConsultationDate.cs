using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliniqueApp.Migrations
{
    /// <inheritdoc />
    public partial class DeleteConsultationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Cliniques_Medecin_CliniqueId",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "DateConsultation",
                table: "Consultations");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Cliniques_Medecin_CliniqueId",
                table: "Utilisateurs",
                column: "Medecin_CliniqueId",
                principalTable: "Cliniques",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Cliniques_Medecin_CliniqueId",
                table: "Utilisateurs");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateConsultation",
                table: "Consultations",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Cliniques_Medecin_CliniqueId",
                table: "Utilisateurs",
                column: "Medecin_CliniqueId",
                principalTable: "Cliniques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
