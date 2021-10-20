using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Persistence.Migrations
{
    public partial class UpdateJobSpec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "JobSpecifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "JobSpecifications");
        }
    }
}
