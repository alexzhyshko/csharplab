

namespace Library.Application
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Library.Domain;
    using Library.Managers;
    using MySql.Data.MySqlClient;

    public class RentalController
    {
        private MySqlConnection _connection;
        private BookManager _books;
        private RentalManager _rental;
        private ReaderManager _readers;
        private AdminManager _admins;

        public RentalController(String connStr, BookManager books, RentalManager rental, ReaderManager readers, AdminManager admins)
        {
            _connection = new MySqlConnection(connStr);
            _connection.Open();

            books.Connection = _connection;
            rental.Connection = _connection;
            readers.Connection = _connection;
            admins.Connection = _connection;


            _books = books;
            _rental = rental;
            _readers = readers;
            _admins = admins;
        }

        public Admin TryPickAdminByUsername(string username)
        {
            return _admins.GetByName(username);
        }


        public List<Admin> GetAllAdmins()
        {
            return _admins.TryGetAll();
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
            return _rental.TryUpdate(book.Id, reader.Id);

        }

        public bool TryReturnBook(Book book, Reader reader)
        {




            if (!_rental.GetBookRentStatus(book.Id))
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
            return _books.TryRemove(book.Id);
        }

        public bool TryAddBook(Book book)
        {
            return _books.TryAdd(book) && _rental.TryAdd(book.Id);

        }

        public List<Book> GetAllNonBookedBooks()
        {
            return _books.GetNonRentBooks();
        }

        public IReadOnlyCollection<Book> GetAllBooks()
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
            List<Book> books = _books.Find(input);
            List<Book> res = new List<Book>();
            foreach (Book book in books)
            {
                if (!_rental.GetBookRentStatus(book.Id))
                {
                    res.Add(book);
                }
            }
            return res;
        }

        private bool CheckParams(Book book, Reader reader)
        {
            return _rental.BookExists(book)&& _readers.ReaderExists(reader);
        }


    }
}
