using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrechoForte.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaConfiguracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeLoja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenhaAdmin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracoes");
        }
    }
}
