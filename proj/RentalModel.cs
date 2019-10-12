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
            return _rental.ContainsKey(book.Id);
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

            _rental[bookid] = readerid;
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
                if (_rental[guid].Equals(reader.Id))
                {
                    res.Add(guid);

                }
            }
            return res;
        }

        public Dictionary<Guid, Guid> GetRentBooks()
        {
            Dictionary <Guid, Guid> res= new Dictionary<Guid, Guid>();
            foreach (Guid Id in _rental.Keys)
            {
                if (!_rental[Id].Equals(Guid.Empty))
                {
                    res.Add(Id, _rental[Id]);

                }
            }
            return res;
        }


        public Dictionary<Guid, Guid> TryGetAll()
        {
            Dictionary<Guid, Guid> res = new Dictionary<Guid, Guid>();
            foreach (Guid key in _rental.Keys)
            {
                res.Add(key, _rental[key]);
            }
            return res;
        }
    }
}
