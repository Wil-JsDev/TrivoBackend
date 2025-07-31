using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trivo.Infraestructura.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTipoMensaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Mensaje",
                type: "varchar(50)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Mensaje");
        }
    }
}
