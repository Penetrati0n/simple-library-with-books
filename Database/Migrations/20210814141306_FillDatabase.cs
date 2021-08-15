using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class FillDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            InsertData(
               table: "dim_genre",
               columns: new[] { "genre_id", "genre_name", "time_create", "time_edit", "version" },
               values: new object[][]
               {
                    new object[] { 1000, "Роман", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1001, "Детектив", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1002, "Фантастика", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1003, "Приключения", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1004, "Научная книга", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
               });

            InsertData(
                table: "author",
                columns: new[] { "author_id", "first_name", "last_name", "middle_name", "time_create", "time_edit", "version" },
                values: new object[][]
                {
                    new object[] { 1000, "Эрих", "Ремарк", "Мария", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1001, "Анджей", "Сапковский", "", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1002, "Дина", "Рубина", "Ильинична", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1003, "Дэвид", "Грегори", "Робертс", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1004, "Лев", "Толстой", "Николаевич", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                });

            InsertData(
                table: "person",
                columns: new[] { "person_id", "last_name", "first_name", "middle_name", "birth_date", "time_create", "time_edit", "version" },
                values: new object[][]
                {
                    new object[] { 1000, "Лютов", "Дорофей", "Валентинович", new DateTimeOffset(1973, 7, 17, 0, 0, 0, TimeSpan.Zero), DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1001, "Фролов", "Виссарион", "Сергеевич", new DateTimeOffset(1999, 9, 2, 0, 0, 0, TimeSpan.Zero), DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1002, "Никитина", "Виктория", "Сергеевна", new DateTimeOffset(1999, 3, 10, 0, 0, 0, TimeSpan.Zero), DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1003, "Ерофеев", "Ануфри", "Святославович", new DateTimeOffset(2002, 6, 16, 0, 0, 0, TimeSpan.Zero), DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1004, "Беляков", "Лаврентий", "ВаИвановичлентинович", new DateTimeOffset(1990, 8, 23, 0, 0, 0, TimeSpan.Zero), DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                });

            InsertData(
                table: "book",
                columns: new[] { "book_id", "author_id", "name", "time_create", "time_edit", "version" },
                values: new object[][]
                {
                    new object[] { 1000, 1000, "Мастер и Маргарита", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1001, 1001, "Мёртвые души", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1002, 1002, "Двенадцать стульев", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1003, 1003, "Собачье сердце", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                    new object[] { 1004, 1004, "Война и мир", DateTimeOffset.Now, DateTimeOffset.Now, 1 },
                });

            InsertData(
                table: "book_genre_lnk",
                columns: new[] { "book_id", "genre_id" },
                values: new object[][]
                {
                    new object[] { 1000, 1004 },
                    new object[] { 1001, 1003 },
                    new object[] { 1002, 1002 },
                    new object[] { 1003, 1001 },
                    new object[] { 1004, 1000 },
                });

            InsertData(
                table: "library_card",
                columns: new[] { "book_book_id", "person_person_id", "time_create", "time_edit", "time_return", "version" },
                values: new object[][]
                {
                    new object[] { 1000, 1004, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(7), 1 },
                    new object[] { 1001, 1003, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(7), 1 },
                    new object[] { 1002, 1002, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(7), 1 },
                    new object[] { 1003, 1001, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(7), 1 },
                    new object[] { 1004, 1000, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(7), 1 },
                });

            void InsertData(string table, string[] columns, object[][] values)
            {
                foreach (var value in values)
                    migrationBuilder.InsertData(
                        table: table,
                        columns: columns,
                        values: value);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
