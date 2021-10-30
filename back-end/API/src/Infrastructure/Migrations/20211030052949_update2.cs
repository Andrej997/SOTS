using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "max_points",
                schema: "project",
                table: "tests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_questions_test_id",
                schema: "project",
                table: "questions",
                column: "test_id");

            migrationBuilder.AddForeignKey(
                name: "FK_questions_tests_test_id",
                schema: "project",
                table: "questions",
                column: "test_id",
                principalSchema: "project",
                principalTable: "tests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_tests_test_id",
                schema: "project",
                table: "questions");

            migrationBuilder.DropIndex(
                name: "IX_questions_test_id",
                schema: "project",
                table: "questions");

            migrationBuilder.DropColumn(
                name: "max_points",
                schema: "project",
                table: "tests");
        }
    }
}
