using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tests_test_time",
                schema: "project");

            migrationBuilder.CreateIndex(
                name: "IX_tests_test_time_id",
                schema: "project",
                table: "tests",
                column: "test_time_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tests_test_time_test_time_id",
                schema: "project",
                table: "tests",
                column: "test_time_id",
                principalSchema: "project",
                principalTable: "test_time",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tests_test_time_test_time_id",
                schema: "project",
                table: "tests");

            migrationBuilder.DropIndex(
                name: "IX_tests_test_time_id",
                schema: "project",
                table: "tests");

            migrationBuilder.CreateTable(
                name: "tests_test_time",
                schema: "project",
                columns: table => new
                {
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    test_time_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tests_test_time", x => new { x.test_id, x.test_time_id });
                });
        }
    }
}
