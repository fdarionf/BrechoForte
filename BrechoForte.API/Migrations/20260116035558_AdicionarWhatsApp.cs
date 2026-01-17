using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrechoForte.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarWhatsApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Whatsapp",
                table: "Configuracoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Whatsapp",
                table: "Configuracoes");
        }
    }
}
