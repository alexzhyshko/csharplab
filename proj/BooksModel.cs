using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain;

namespace Library.Model
{
    public class BooksModel
    {
        
        private Dictionary<Guid, Book> _books = new Dictionary<Guid, Book>();

        public BooksModel()
        {

        }

        public List<Book> TryGetAll()
        {
            List<Book> result = new List<Book>();
            foreach (Guid id in _books.Keys)
            {
                result.Add(TryGet(id));
            }
            return result;
        }

        public Book TryPickByName(string name)
        {
            foreach (Book book in _books.Values)
            {
                if (book.name.Equals(name)&&!RentalModel.GetBookRentStatus(book.id))
                {
                    return book;
                }
            }
            return null;
        }

        public bool TryAdd(Book book)
        {
            if (_books.ContainsKey(book.id))
            {
                return false;
            }
            long startSize = _books.Count;
            _books.Add(book.id, book);
            return startSize != _books.Count;

        }
        public bool TryRemove(Guid id)
        {
            if (!_books.ContainsKey(id))
            {
                return false;
            }
            long startSize = _books.Count;
            _books.Remove(id);
            return startSize - _books.Count == 1;
        }

        public Book TryGet(Guid id)
        {
            if (!_books.ContainsKey(id))
            {
                return null;
            }
            return _books[id];
        }
    }
}
