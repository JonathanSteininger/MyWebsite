using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyActualWebsite.Data.Migrations
{
    public partial class featuredoptiontforprojects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Featured",
                table: "Project",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Featured",
                table: "Project");
        }
    }
}
