using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class StudentTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "took_test",
                schema: "project",
                table: "student_tests");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "project",
                table: "student_tests",
                newName: "id");

            migrationBuilder.AlterColumn<double>(
                name: "points",
                schema: "project",
                table: "student_tests",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<DateTime>(
                name: "test_finished",
                schema: "project",
                table: "student_tests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "test_started",
                schema: "project",
                table: "student_tests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_student_tests_grade_id",
                schema: "project",
                table: "student_tests",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_tests_test_id",
                schema: "project",
                table: "student_tests",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_tests_user_id",
                schema: "project",
                table: "student_tests",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_student_tests_grades_grade_id",
                schema: "project",
                table: "student_tests",
                column: "grade_id",
                principalSchema: "project",
                principalTable: "grades",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_tests_tests_test_id",
                schema: "project",
                table: "student_tests",
                column: "test_id",
                principalSchema: "project",
                principalTable: "tests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_tests_users_user_id",
                schema: "project",
                table: "student_tests",
                column: "user_id",
                principalSchema: "project",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_tests_grades_grade_id",
                schema: "project",
                table: "student_tests");

            migrationBuilder.DropForeignKey(
                name: "FK_student_tests_tests_test_id",
                schema: "project",
                table: "student_tests");

            migrationBuilder.DropForeignKey(
                name: "FK_student_tests_users_user_id",
                schema: "project",
                table: "student_tests");

            migrationBuilder.DropIndex(
                name: "IX_student_tests_grade_id",
                schema: "project",
                table: "student_tests");

            migrationBuilder.DropIndex(
                name: "IX_student_tests_test_id",
                schema: "project",
                table: "student_tests");

            migrationBuilder.DropIndex(
                name: "IX_student_tests_user_id",
                schema: "project",
                table: "student_tests");

            migrationBuilder.DropColumn(
                name: "test_finished",
                schema: "project",
                table: "student_tests");

            migrationBuilder.DropColumn(
                name: "test_started",
                schema: "project",
                table: "student_tests");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "project",
                table: "student_tests",
                newName: "Id");

            migrationBuilder.AlterColumn<double>(
                name: "points",
                schema: "project",
                table: "student_tests",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "took_test",
                schema: "project",
                table: "student_tests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
