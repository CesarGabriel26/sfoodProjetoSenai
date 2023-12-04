using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sfood.Migrations
{
    /// <inheritdoc />
    public partial class required4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmExtoque",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmExtoque",
                table: "Produtos");
        }
    }
}
