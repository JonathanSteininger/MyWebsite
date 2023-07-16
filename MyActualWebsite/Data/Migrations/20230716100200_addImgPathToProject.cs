using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyActualWebsite.Data.Migrations
{
    public partial class addImgPathToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Project");
        }
    }
}
