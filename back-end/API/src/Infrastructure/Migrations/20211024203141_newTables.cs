using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Infrastructure.Migrations
{
    public partial class newTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "question_id",
                schema: "project",
                table: "tests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "test_time_id",
                schema: "project",
                table: "tests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "from_procentage",
                schema: "project",
                table: "grades",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "to_procentage",
                schema: "project",
                table: "grades",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "answers",
                schema: "project",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text_answer = table.Column<string>(type: "text", nullable: true),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question_completed",
                schema: "project",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_tests_id = table.Column<long>(type: "bigint", nullable: false),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    completed_persentage = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question_completed", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                schema: "project",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text_question = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "student_tests",
                schema: "project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    took_test = table.Column<bool>(type: "boolean", nullable: false),
                    points = table.Column<double>(type: "double precision", nullable: false),
                    grade_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_test", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "test_time",
                schema: "project",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_time", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "user_subject",
                schema: "project",
                columns: table => new
                {
                    subject_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_subject", x => new { x.subject_id, x.user_id });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers",
                schema: "project");

            migrationBuilder.DropTable(
                name: "question_completed",
                schema: "project");

            migrationBuilder.DropTable(
                name: "questions",
                schema: "project");

            migrationBuilder.DropTable(
                name: "student_tests",
                schema: "project");

            migrationBuilder.DropTable(
                name: "test_time",
                schema: "project");

            migrationBuilder.DropTable(
                name: "tests_test_time",
                schema: "project");

            migrationBuilder.DropTable(
                name: "user_subject",
                schema: "project");

            migrationBuilder.DropColumn(
                name: "question_id",
                schema: "project",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "test_time_id",
                schema: "project",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "from_procentage",
                schema: "project",
                table: "grades");

            migrationBuilder.DropColumn(
                name: "to_procentage",
                schema: "project",
                table: "grades");
        }
    }
}
