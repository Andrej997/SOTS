using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "question_id",
                schema: "project",
                table: "tests");

            migrationBuilder.AddColumn<long>(
                name: "test_id",
                schema: "project",
                table: "questions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_answers_question_id",
                schema: "project",
                table: "answers",
                column: "question_id");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_questions_question_id",
                schema: "project",
                table: "answers",
                column: "question_id",
                principalSchema: "project",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_questions_question_id",
                schema: "project",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_question_id",
                schema: "project",
                table: "answers");

            migrationBuilder.DropColumn(
                name: "test_id",
                schema: "project",
                table: "questions");

            migrationBuilder.AddColumn<long>(
                name: "question_id",
                schema: "project",
                table: "tests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
