using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace API.Infrastructure.Persistence.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = $@"
                insert into project.roles (id, name) values (1, 'admin');
                insert into project.roles (id, name) values (2, 'professor');
                insert into project.roles (id, name) values (3, 'student');

                insert into project.users (id, name, surname, username, password_hash, created_at) values (1, 'admin', 'admin', 'admin', 'admin', '{DateTime.UtcNow}');
                insert into project.user_roles (user_id, role_id) values (1, 1);
            ";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
