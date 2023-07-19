using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyActualWebsite.Data.Migrations
{
    public partial class AddSummaryToProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Project");
        }
    }
}
