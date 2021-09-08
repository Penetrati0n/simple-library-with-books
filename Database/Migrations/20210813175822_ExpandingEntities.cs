using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class ExpandingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_create",
                table: "person",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_edit",
                table: "person",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "person",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_create",
                table: "dim_genre",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_edit",
                table: "dim_genre",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "dim_genre",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_create",
                table: "book",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_edit",
                table: "book",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "book",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_create",
                table: "author",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_edit",
                table: "author",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "author",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql($"UPDATE \"public\".\"person\" SET \"time_create\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"person\" SET \"time_edit\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"person\" SET \"version\" = {1};");
            migrationBuilder.Sql($"UPDATE \"public\".\"book\" SET \"time_create\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"book\" SET \"time_edit\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"book\" SET \"version\" = {1};");
            migrationBuilder.Sql($"UPDATE \"public\".\"dim_genre\" SET \"time_create\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"dim_genre\" SET \"time_edit\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"dim_genre\" SET \"version\" = {1};");
            migrationBuilder.Sql($"UPDATE \"public\".\"author\" SET \"time_create\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"author\" SET \"time_edit\" = \'{DateTimeOffset.Now}\';");
            migrationBuilder.Sql($"UPDATE \"public\".\"author\" SET \"version\" = {1};");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_create",
                table: "person");

            migrationBuilder.DropColumn(
                name: "time_edit",
                table: "person");

            migrationBuilder.DropColumn(
                name: "version",
                table: "person");

            migrationBuilder.DropColumn(
                name: "time_create",
                table: "dim_genre");

            migrationBuilder.DropColumn(
                name: "time_edit",
                table: "dim_genre");

            migrationBuilder.DropColumn(
                name: "version",
                table: "dim_genre");

            migrationBuilder.DropColumn(
                name: "time_create",
                table: "book");

            migrationBuilder.DropColumn(
                name: "time_edit",
                table: "book");

            migrationBuilder.DropColumn(
                name: "version",
                table: "book");

            migrationBuilder.DropColumn(
                name: "time_create",
                table: "author");

            migrationBuilder.DropColumn(
                name: "time_edit",
                table: "author");

            migrationBuilder.DropColumn(
                name: "version",
                table: "author");
        }
    }
}
