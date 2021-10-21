using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class FkForTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                ALTER TABLE project.tests
                    ADD CONSTRAINT fk_subject_id FOREIGN KEY (subject_id)
                    REFERENCES project.subjects (id)
                    ON UPDATE NO ACTION
                    ON DELETE NO ACTION
                    NOT VALID;
                ";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
