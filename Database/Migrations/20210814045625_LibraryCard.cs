using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class LibraryCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_create",
                table: "library_card",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_edit",
                table: "library_card",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_return",
                table: "library_card",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "library_card",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql($"UPDATE \"public\".\"library_card\" SET \"time_create\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"library_card\" SET \"time_edit\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"library_card\" SET \"time_return\" = \'{DateTimeOffset.Now.AddDays(7)}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"library_card\" SET \"version\" = {1};");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_create",
                table: "library_card");

            migrationBuilder.DropColumn(
                name: "time_edit",
                table: "library_card");

            migrationBuilder.DropColumn(
                name: "time_return",
                table: "library_card");

            migrationBuilder.DropColumn(
                name: "version",
                table: "library_card");
        }
    }
}
