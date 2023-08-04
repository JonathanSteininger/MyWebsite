using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyActualWebsite.Data.Migrations
{
    public partial class addStatBarCatagories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatBarCatagoryID",
                table: "StatBar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StatBarCatagory",
                columns: table => new
                {
                    StatBarCatagoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatBarCatagory", x => x.StatBarCatagoryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatBar_StatBarCatagoryID",
                table: "StatBar",
                column: "StatBarCatagoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_StatBar_StatBarCatagory_StatBarCatagoryID",
                table: "StatBar",
                column: "StatBarCatagoryID",
                principalTable: "StatBarCatagory",
                principalColumn: "StatBarCatagoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatBar_StatBarCatagory_StatBarCatagoryID",
                table: "StatBar");

            migrationBuilder.DropTable(
                name: "StatBarCatagory");

            migrationBuilder.DropIndex(
                name: "IX_StatBar_StatBarCatagoryID",
                table: "StatBar");

            migrationBuilder.DropColumn(
                name: "StatBarCatagoryID",
                table: "StatBar");
        }
    }
}
