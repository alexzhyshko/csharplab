using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Library.Domain;

namespace Library.Model
{
    public class BookModel
    {
        
        private Dictionary<Guid, Book> _books = new Dictionary<Guid, Book>();

        public BookModel()
        {

        }

      
        public List<Book> Find(string text)
        {
            List<Book> res = new List<Book>();
            foreach (Book book in _books.Values)
            {
                if(book.Name.Contains(text))
                {
                    res.Add(book);
                }
               
            }
            return res;
        }
        public Book TryPickByName(string name)
        {
            foreach (Book book in _books.Values)
            {
                if (book.Name.Equals(name)&&!RentalModel.GetBookRentStatus(book.Id))
                {
                    return book;
                }
            }
            return null;
        }

        public bool TryAdd(Book book)
        {
            if (_books.ContainsKey(book.Id))
            {
                return false;
            }
            long startSize = _books.Count;
            _books.Add(book.Id, book);
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
        

        public List<Book> TryGetAll()
        {
            List<Book> res = new List<Book>();
            foreach (Book book in _books.Values)
            {
                res.Add(new Book(book.Id, book.Name, book.Authors));
            }
            return res;
           
        }
    }
}
