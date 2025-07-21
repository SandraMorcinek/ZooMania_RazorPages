using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt2.Migrations
{
    /// <inheritdoc />
    public partial class DropCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //name: "FK_Cart_Clients_ClientId",
            //table: "Cart");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Cart_Products_ProductId",
            //    table: "Cart");

            migrationBuilder.DropTable(
                name: "Cart");
        }
    }
}
