using System.Collections.Generic;
using SimpleLibraryWithBooks.Models.Book;

namespace SimpleLibraryWithBooks.Services
{
    public static class BookRepository
    {
        public readonly static List<BookModel> Books = new List<BookModel>()
        {
            new BookModel() { Title  = "Горе от ума", Author = "Александр Грибоедов", Genre = "Комедия" },
            new BookModel() { Title  = "Гордость и предубеждение", Author = "Джейн Остин", Genre = "Роман" },
            new BookModel() { Title  = "Тёмные начала", Author = "Филип Пулман", Genre = "Фэнтези" },
        };
    }
}
