using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Infrastructure.Migrations
{
    public partial class QuestionTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "question_completed",
                schema: "project");

            migrationBuilder.CreateTable(
                name: "question_times",
                schema: "project",
                columns: table => new
                {
                    student_tests_id = table.Column<long>(type: "bigint", nullable: false),
                    question_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    question_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question_times", x => new { x.question_start, x.student_tests_id });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "question_times",
                schema: "project");

            migrationBuilder.CreateTable(
                name: "question_completed",
                schema: "project",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    completed_persentage = table.Column<double>(type: "double precision", nullable: false),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    student_tests_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question_completed", x => x.id);
                });
        }
    }
}
