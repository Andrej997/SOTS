﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Infrastructure.Migrations
{
    public partial class EdgeDomainId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "domain_id",
                schema: "project",
                table: "edges",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "domain_id",
                schema: "project",
                table: "edges");
        }
    }
}