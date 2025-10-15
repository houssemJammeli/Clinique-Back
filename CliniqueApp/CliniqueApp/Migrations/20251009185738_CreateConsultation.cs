using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliniqueApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateConsultation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factures_RendezVous_ConsultationId",
                table: "Factures");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordonnances_RendezVous_ConsultationId",
                table: "Ordonnances");

            migrationBuilder.DropForeignKey(
                name: "FK_RendezVous_Utilisateurs_MedecinId",
                table: "RendezVous");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Cliniques_CliniqueId",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "RendezVous");

            migrationBuilder.AlterColumn<int>(
                name: "Statut",
                table: "RendezVous",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateConsultation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RendezVousId = table.Column<int>(type: "int", nullable: false),
                    Rapport = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedecinId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultations_RendezVous_RendezVousId",
                        column: x => x.RendezVousId,
                        principalTable: "RendezVous",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultations_Utilisateurs_MedecinId",
                        column: x => x.MedecinId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_MedecinId",
                table: "Consultations",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_RendezVousId",
                table: "Consultations",
                column: "RendezVousId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factures_Consultations_ConsultationId",
                table: "Factures",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordonnances_Consultations_ConsultationId",
                table: "Ordonnances",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RendezVous_Utilisateurs_MedecinId",
                table: "RendezVous",
                column: "MedecinId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Cliniques_CliniqueId",
                table: "Utilisateurs",
                column: "CliniqueId",
                principalTable: "Cliniques",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factures_Consultations_ConsultationId",
                table: "Factures");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordonnances_Consultations_ConsultationId",
                table: "Ordonnances");

            migrationBuilder.DropForeignKey(
                name: "FK_RendezVous_Utilisateurs_MedecinId",
                table: "RendezVous");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Cliniques_CliniqueId",
                table: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.AlterColumn<int>(
                name: "Statut",
                table: "RendezVous",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "RendezVous",
                type: "varchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Factures_RendezVous_ConsultationId",
                table: "Factures",
                column: "ConsultationId",
                principalTable: "RendezVous",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordonnances_RendezVous_ConsultationId",
                table: "Ordonnances",
                column: "ConsultationId",
                principalTable: "RendezVous",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RendezVous_Utilisateurs_MedecinId",
                table: "RendezVous",
                column: "MedecinId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Cliniques_CliniqueId",
                table: "Utilisateurs",
                column: "CliniqueId",
                principalTable: "Cliniques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
