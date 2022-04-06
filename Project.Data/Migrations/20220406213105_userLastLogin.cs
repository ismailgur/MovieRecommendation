using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class userLastLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastLoggedIn",
                table: "Users",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoggedIn",
                table: "Users");
        }
    }
}
