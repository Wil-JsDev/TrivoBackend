using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trivo.Infraestructura.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionDeRelacionesChatMensaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKEmisorId",
                table: "Mensaje");

            migrationBuilder.AlterColumn<Guid>(
                name: "FkEmisorId",
                table: "Mensaje",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "FKReceptorId",
                table: "Mensaje",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_FKReceptorId",
                table: "Mensaje",
                column: "FKReceptorId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmisorId",
                table: "Mensaje",
                column: "FkEmisorId",
                principalTable: "Usuario",
                principalColumn: "PkUsuarioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceptorId",
                table: "Mensaje",
                column: "FKReceptorId",
                principalTable: "Usuario",
                principalColumn: "PkUsuarioId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmisorId",
                table: "Mensaje");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceptorId",
                table: "Mensaje");

            migrationBuilder.DropIndex(
                name: "IX_Mensaje_FKReceptorId",
                table: "Mensaje");

            migrationBuilder.DropColumn(
                name: "FKReceptorId",
                table: "Mensaje");

            migrationBuilder.AlterColumn<Guid>(
                name: "FkEmisorId",
                table: "Mensaje",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FKEmisorId",
                table: "Mensaje",
                column: "FkEmisorId",
                principalTable: "Usuario",
                principalColumn: "PkUsuarioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
