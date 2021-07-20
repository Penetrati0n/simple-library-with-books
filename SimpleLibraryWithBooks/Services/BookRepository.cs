using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Book;

namespace SimpleLibraryWithBooks.Services
{
    public static class BookRepository
    {
        public readonly static List<BookDetailDto> Books = new List<BookDetailDto>()
        {
            new BookDetailDto() { Title  = "Горе от ума", Author = "Александр Грибоедов", Genre = "Комедия" },
            new BookDetailDto() { Title  = "Гордость и предубеждение", Author = "Джейн Остин", Genre = "Роман" },
            new BookDetailDto() { Title  = "Тёмные начала", Author = "Филип Пулман", Genre = "Фэнтези" },
        };
    }
}
