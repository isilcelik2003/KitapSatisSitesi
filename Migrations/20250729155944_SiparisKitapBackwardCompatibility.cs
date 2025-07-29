using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitapSatisSitesi.Migrations
{
    /// <inheritdoc />
    public partial class SiparisKitapBackwardCompatibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KitapID",
                table: "Siparisler",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_KitapID",
                table: "Siparisler",
                column: "KitapID");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Kitaplar_KitapID",
                table: "Siparisler",
                column: "KitapID",
                principalTable: "Kitaplar",
                principalColumn: "KitapID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Kitaplar_KitapID",
                table: "Siparisler");

            migrationBuilder.DropIndex(
                name: "IX_Siparisler_KitapID",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "KitapID",
                table: "Siparisler");
        }
    }
}
