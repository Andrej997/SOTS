using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class FKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                ALTER TABLE project.user_roles
                    ADD CONSTRAINT fk_user FOREIGN KEY (user_id)
                    REFERENCES project.users (id)
                    ON UPDATE NO ACTION
                    ON DELETE NO ACTION
                    NOT VALID;

                ALTER TABLE project.user_roles
                    ADD CONSTRAINT fk_role FOREIGN KEY (role_id)
                    REFERENCES project.roles (id)
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
