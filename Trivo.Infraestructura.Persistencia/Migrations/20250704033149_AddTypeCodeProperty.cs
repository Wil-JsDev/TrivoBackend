using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trivo.Infraestructura.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeCodeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Codigo",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Codigo");
        }
    }
}
