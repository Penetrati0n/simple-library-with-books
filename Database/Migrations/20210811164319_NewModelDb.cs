using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Database.Migrations
{
    public partial class NewModelDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "People",
                newName: "person");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "book");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "person",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "person",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "person",
                newName: "birth_date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "person",
                newName: "person_id");

            migrationBuilder.RenameColumn(
                name: "Patronymic",
                table: "person",
                newName: "middle_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "book",
                newName: "book_id");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "person",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "person",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "author_id",
                table: "book",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "book",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_person",
                table: "person",
                column: "person_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_book",
                table: "book",
                column: "book_id");

            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_author", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "library_card",
                columns: table => new
                {
                    book_book_id = table.Column<int>(type: "integer", nullable: false),
                    person_person_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_library_card", x => new { x.book_book_id, x.person_person_id });
                    table.ForeignKey(
                        name: "FK_library_card_book_book_book_id",
                        column: x => x.book_book_id,
                        principalTable: "book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_library_card_person_person_person_id",
                        column: x => x.person_person_id,
                        principalTable: "person",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dim_genre",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    genre_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_genre", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "book_genre_lnk",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    genre_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_genre_lnk", x => new { x.book_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_book_genre_lnk_book_book_id",
                        column: x => x.book_id,
                        principalTable: "book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_book_genre_lnk_dim_genre_genre_id",
                        column: x => x.genre_id,
                        principalTable: "dim_genre",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_book_author_id",
                table: "book",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_genre_lnk_genre_id",
                table: "book_genre_lnk",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_library_card_person_person_id",
                table: "library_card",
                column: "person_person_id");

            migrationBuilder.AddForeignKey(
                name: "FK_book_author_author_id",
                table: "book",
                column: "author_id",
                principalTable: "author",
                principalColumn: "author_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_book_author_author_id",
                table: "book");

            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropTable(
                name: "book_genre_lnk");

            migrationBuilder.DropTable(
                name: "library_card");

            migrationBuilder.DropTable(
                name: "dim_genre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_person",
                table: "person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_book",
                table: "book");

            migrationBuilder.DropIndex(
                name: "IX_book_author_id",
                table: "book");

            migrationBuilder.DropColumn(
                name: "author_id",
                table: "book");

            migrationBuilder.DropColumn(
                name: "name",
                table: "book");

            migrationBuilder.RenameTable(
                name: "person",
                newName: "People");

            migrationBuilder.RenameTable(
                name: "book",
                newName: "Books");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "People",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "People",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "birth_date",
                table: "People",
                newName: "Birthday");

            migrationBuilder.RenameColumn(
                name: "person_id",
                table: "People",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "middle_name",
                table: "People",
                newName: "Patronymic");

            migrationBuilder.RenameColumn(
                name: "book_id",
                table: "Books",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "People",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "People",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PersonBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: true),
                    DateTimeReceipt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonBooks_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonBooks_BookId",
                table: "PersonBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonBooks_PersonId",
                table: "PersonBooks",
                column: "PersonId");
        }
    }
}
