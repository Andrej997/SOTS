using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Infrastructure.Migrations
{
    public partial class GradesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grades",
                schema: "project",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grade", x => x.id);
                });

            var sql = @"
                INSERT INTO project.grades (label) VALUES ('5');
                INSERT INTO project.grades (label) VALUES ('6');
                INSERT INTO project.grades (label) VALUES ('7');
                INSERT INTO project.grades (label) VALUES ('8');
                INSERT INTO project.grades (label) VALUES ('9');
                INSERT INTO project.grades (label) VALUES ('10');
                ";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grades",
                schema: "project");
        }
    }
}
