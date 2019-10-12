

namespace Library
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Library.Domain;
    using Library.Model;

    public class RentalController
    {
        private BookModel _books;
        private RentalModel _rental;
        private ReaderModel _readers;
        private AdminModel _admins;

        public RentalController(BookModel books, RentalModel rental, ReaderModel readers, AdminModel admins)
        {
            _books = books;
            _rental = rental;
            _readers = readers;
            _admins = admins;
        }


        public bool TryAddAdmin(Admin admin)
        {
            return _admins.TryAdd(admin) && _readers.TryAdd(admin);
        }

        public bool TryRemoveAdmin(Admin admin)
        {
            return _admins.TryRemove(admin);
        }

        public bool CheckRights(Reader reader)
        {
            return _admins.CheckRights(reader);
        }

        public bool TryRegisterUser(Reader reader)
        {
           
            return _readers.TryAdd(reader);
        }

        public bool TryRemoveUser(Reader reader)
        {
            return _readers.TryRemove(reader.Id);
        }

        public bool TryRentBook(Book book, Reader reader)
        {
            if (!CheckParams(book, reader))
            {
                return false;
            }

            if (RentalModel.GetBookRentStatus(book.Id))
            {
                return false;
            }

            return _rental.TryUpdate(book.Id, reader.Id);

        }

        public bool TryReturnBook(Book book, Reader reader)
        {



            if (!CheckParams(book, reader))
            {
                return false;
            }

            if (!RentalModel.GetBookRentStatus(book.Id))
            {
                return false;
            }

            OptionalGuid result = _rental.TryGet(book.Id);
            if (result.ContainsResult)
            {
                if (result.Id.Equals(reader.Id))
                {
                    return _rental.TryUpdate(book.Id, Guid.Empty);
                }
            }
            return false;

        }

        public Book TryGetBookById(Guid Id)
        {
            return _books.TryGet(Id);
        }

        public bool TryRemoveBook(Book book)
        {

            if (!RentalModel.GetBookRentStatus(book.Id))
            {
                return _books.TryRemove(book.Id) && _rental.TryRemove(book.Id);
            }
            return false;
            
        }

        public bool TryAddBook(Book book)
        {
            return _books.TryAdd(book) && _rental.TryAdd(book.Id);

        }

        public List<Book> GetAllNonBookedBooks()
        {
            List<Book> res = new List<Book>();
            List<Book> temp = _books.TryGetAll();
            foreach (Book book in temp)
            {
                if (!RentalModel.GetBookRentStatus(book.Id))
                {
                    res.Add(book);
                }
            }
            return res;
        }

        public List<Book> GetAllBooks()
        {
            return _books.TryGetAll();
        }

        public Reader TryGetReader(Book book)
        {
            OptionalGuid res = _rental.TryGet(book.Id);
            if (res.ContainsResult)
            {
                return _readers.TryGet(res.Id);
            }
            return null;
        }

        public Reader TryPickUserByName(string username)
        {
            return _readers.TryPickByName(username);
        }

        public Book TryPickBookByName(string title)
        {
            return _books.TryPickByName(title);
        }

        public List<Book> GetUserBooks(Reader reader)
        {
            List<Book> res = new List<Book>();
            List<Guid> bookIds = _rental.GetUserBooks(reader);
            foreach (Guid Id in bookIds)
            {
                res.Add(_books.TryGet(Id));
            }
            return res;

        }


        public Dictionary<Book, Reader> GetRentBooks()
        {
            Dictionary<Book, Reader> res = new Dictionary<Book, Reader>();

            Dictionary<Guid, Guid> ids = _rental.GetRentBooks();

            foreach (Guid bookid in ids.Keys)
            {
                res.Add(_books.TryGet(bookid), _readers.TryGet(ids[bookid]));
            }
            return res;
        }

        public List<Book> FindBooks(string input)
        {
            return _books.Find(input);
        }

        private bool CheckParams(Book book, Reader reader)
        {
            return _rental.BookExists(book)&& _readers.ReaderExists(reader);
        }


    }
}
