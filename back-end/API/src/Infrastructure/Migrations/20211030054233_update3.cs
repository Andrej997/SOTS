using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "points",
                schema: "project",
                table: "questions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "points",
                schema: "project",
                table: "questions");
        }
    }
}
