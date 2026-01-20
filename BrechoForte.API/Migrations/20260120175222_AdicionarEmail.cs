using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrechoForte.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailContato",
                table: "Configuracoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailContato",
                table: "Configuracoes");
        }
    }
}
