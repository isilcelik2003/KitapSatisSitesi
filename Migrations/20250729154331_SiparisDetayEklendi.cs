using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitapSatisSitesi.Migrations
{
    /// <inheritdoc />
    public partial class SiparisDetayEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "SiparisDetaylar",
                columns: table => new
                {
                    SiparisDetayID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SiparisID = table.Column<int>(type: "INTEGER", nullable: false),
                    KitapID = table.Column<int>(type: "INTEGER", nullable: false),
                    Adet = table.Column<int>(type: "INTEGER", nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ToplamFiyat = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiparisDetaylar", x => x.SiparisDetayID);
                    table.ForeignKey(
                        name: "FK_SiparisDetaylar_Kitaplar_KitapID",
                        column: x => x.KitapID,
                        principalTable: "Kitaplar",
                        principalColumn: "KitapID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiparisDetaylar_Siparisler_SiparisID",
                        column: x => x.SiparisID,
                        principalTable: "Siparisler",
                        principalColumn: "SiparisID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylar_KitapID",
                table: "SiparisDetaylar",
                column: "KitapID");

            migrationBuilder.CreateIndex(
                name: "IX_SiparisDetaylar_SiparisID",
                table: "SiparisDetaylar",
                column: "SiparisID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiparisDetaylar");

            migrationBuilder.AddColumn<int>(
                name: "KitapID",
                table: "Siparisler",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_KitapID",
                table: "Siparisler",
                column: "KitapID");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Kitaplar_KitapID",
                table: "Siparisler",
                column: "KitapID",
                principalTable: "Kitaplar",
                principalColumn: "KitapID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
