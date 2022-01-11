using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class sortQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sort_by",
                schema: "project",
                table: "tests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sort_by",
                schema: "project",
                table: "tests");
        }
    }
}
