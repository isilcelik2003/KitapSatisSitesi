using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitapSatisSitesi.Migrations
{
    /// <inheritdoc />
    public partial class YorumSistemiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YorumTarihi",
                table: "Yorumlar",
                newName: "Tarih");

            migrationBuilder.RenameColumn(
                name: "YorumMetni",
                table: "Yorumlar",
                newName: "Metin");

            migrationBuilder.AddColumn<int>(
                name: "Puan",
                table: "Yorumlar",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Puan",
                table: "Yorumlar");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "Yorumlar",
                newName: "YorumTarihi");

            migrationBuilder.RenameColumn(
                name: "Metin",
                table: "Yorumlar",
                newName: "YorumMetni");
        }
    }
}
