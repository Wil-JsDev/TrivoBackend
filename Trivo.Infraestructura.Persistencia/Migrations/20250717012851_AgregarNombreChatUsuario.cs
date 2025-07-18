using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trivo.Infraestructura.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNombreChatUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Chat");

            migrationBuilder.AddColumn<string>(
                name: "NombreChat",
                table: "ChatUsuario",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreChat",
                table: "ChatUsuario");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Chat",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
