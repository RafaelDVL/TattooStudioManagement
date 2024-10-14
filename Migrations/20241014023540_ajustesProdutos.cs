using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudioTattooManagement.Migrations
{
    /// <inheritdoc />
    public partial class ajustesProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fotos",
                table: "Produtos",
                newName: "ImagemUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagemUrl",
                table: "Produtos",
                newName: "Fotos");
        }
    }
}
