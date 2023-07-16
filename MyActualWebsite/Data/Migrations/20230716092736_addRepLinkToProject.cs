using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyActualWebsite.Data.Migrations
{
    public partial class addRepLinkToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResipitoryUrl",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResipitoryUrl",
                table: "Project");
        }
    }
}
