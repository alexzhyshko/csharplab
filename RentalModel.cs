using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain;



namespace Library.Model
{
    public class RentalModel
    {
        
        private static Dictionary<Guid, Guid> _rental = new Dictionary<Guid, Guid>();
       

        public RentalModel()
        {
           
        }

        public bool BookExists(Book book)
        {
            return _rental.ContainsKey(book.id);
        }


        //true for rent, false for free
        public static bool GetBookRentStatus(Guid bookid)
        {
            return _rental[bookid] != Guid.Empty;
        }

        

        //adds a new book without rental
        public bool TryAdd(Guid bookid)
        {
            if (_rental.ContainsKey(bookid))
            {
                return false;
            }
            long startSize = _rental.Count;
            _rental.Add(bookid, Guid.Empty);
            return startSize != _rental.Count;  
        }

        public bool TryRemove(Guid bookid)
        {
            long startSize = _rental.Count;
            if (!_rental.ContainsKey(bookid))
            {
                return false;
            }
            _rental.Remove(bookid);
            return startSize - _rental.Count==1;
        }

        public bool TryUpdate(Guid bookid, Guid readerid)
        {
            Guid startVal = _rental[bookid];
            if (!_rental.ContainsKey(bookid))
            {
                return false;
            }
            _rental.Add(bookid, readerid);
            return _rental[bookid] != startVal;
        }

        public OptionalGuid TryGet(Guid bookid)
        {
            return new OptionalGuid()
            {
                Id = _rental[bookid],
                ContainsResult = _rental.ContainsKey(bookid)
            };
        }


        public List<Guid> GetUserBooks(Reader reader)
        {
            List<Guid> res = new List<Guid>();

            foreach (Guid guid in _rental.Keys)
            {
                if (_rental[guid].Equals(reader.id))
                {
                    res.Add(guid);

                }
            }
            return res;
        }

        public Dictionary<Guid, Guid> GetRentBooks()
        {
            Dictionary <Guid, Guid> res= new Dictionary<Guid, Guid>();
            foreach (Guid id in _rental.Keys)
            {
                if (!_rental[id].Equals(Guid.Empty))
                {
                    res.Add(id, _rental[id]);

                }
            }
            return res;
        }
        
       

        //public void AddAdmin(Reader reader)
        //{

            
        //    //_admins.Add(reader);
        //}

        //public bool TryAddBook(Book book)
        //{

           

        //     if (!_rental.ContainsKey(book.id))
        //    {
        //        _rental.Add(book.id, Guid.Empty);
        //        _books.Add(book.id, book);
        //        return true;
        //    }
        //    return false;
             

        //}

        //public bool TryRentBook(Book book, Reader user)
        //{


             
        //    if (_rental.ContainsKey(book.id))
        //    {
        //        if (_rental[book.id].Equals(Guid.Empty))
        //        {
        //           _rental[book.id] = user.id;
        //            if (!_readers.ContainsKey(user.id))
        //            {
        //                _readers.Add(user.id, user);
        //            }
        //            return true;
        //        }
        //        return false;
        //    }
        //    return false;
             
        //}

        //public bool TryRemoveReader(Book book)
        //{


             
        //    if (_rental.ContainsKey(book.id))
        //    {
        //        if (!_rental[book.id].Equals(Guid.Empty))
        //        {
        //            _rental[book.id] = Guid.Empty;
        //            return true;
        //        }
        //        return false;
        //    }
        //    return false;
             
        //}

        //public bool TryRemoveBook(Book book)
        //{

             
        //    if (_rental.ContainsKey(book.id))
        //    {
        //        if (_rental[book.id].Equals(Guid.Empty))
        //        {
        //            _rental.Remove(book.id);
        //            _books.Remove(book.id);
        //            return true;
        //        }
        //        return false;
        //    }
        //    return false;
             
        //}

        //public Reader TryGetReader(Book book)
        //{
             
        //    if (_rental.ContainsKey(book.id))
        //    {
        //        return _readers[_rental[book.id]];
        //    }
        //    return null;
             
        //}

        //public Book PickBookByName(string bookName)
        //{
             
        //    foreach (Guid id in _books.Keys)
        //    {
        //        if (_books[id].name.Trim().Equals(bookName))
        //        {
        //            Guid bookid = id;
        //            if (_rental[bookid].Equals(Guid.Empty))
        //            {
        //                return _books[bookid];
        //            }
        //        }
        //    }
        //    return null;
             
        //}
        //public Reader PickUserByName(string username)
        //{
             
        //    Reader result = null;
        //    foreach (Reader reader in _readers.Values)
        //    {
        //        if (reader.name.Trim().Equals(username.Trim()))
        //        {
        //            result = reader;
        //        }
        //    }
        //    if (result==null)
        //    {
        //        result = new Reader(Guid.NewGuid(), username);
        //        _readers.Add(result.id, result);
        //    }

        //    return result;
             
        //}

        //public List<Book> GetAllBooks()
        //{
             
        //    List<Book> result = new List<Book>();
        //    foreach (Guid id in _books.Keys)
        //    {
  
        //            if (_rental[id].Equals(Guid.Empty))
        //            {
        //                result.Add(_books[id]);
        //            } 
        //    }
        //    return result;
             
        //}

        //public List<Book> GetUserBooks(Reader reader)
        //{
             
        //    List<Book> result = new List<Book>();
        //    foreach (Guid id in _rental.Keys)
        //    {
        //        if (_rental[id].Equals(reader.id))
        //        {
        //            result.Add(_books[id]);
        //        }
        //    }
        //    return result;
             
        //}

        //public Book GetBookById(Guid id)
        //{


        //    return _books[id];
        //}
        //public Dictionary<Book, Reader> GetRentBooks()
        //{

             
        //    Dictionary<Book, Reader> result = new Dictionary<Book, Reader>();
        //    foreach (Guid id in _books.Keys)
        //    {

        //        if (!_rental[id].Equals(Guid.Empty))
        //        {
        //            result.Add(_books[id], _readers[_rental[id]]);
        //        }
        //    }
        //    return result;
             
        //}
     


    }
}
