using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain;
using MySql.Data.MySqlClient;

namespace Library.Managers
{
    public class RentalManager
    {

        public MySqlConnection Connection { get; set; }


        public RentalManager()
        {
           
        }

        public bool BookExists(Book book)
        {
            MySqlCommand com = new MySqlCommand("SELECT * FROM Rental WHERE bookid=@id", Connection);
            com.Parameters.AddWithValue("@id", book.Id);
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                return rdr.HasRows;
            }

        }


        //true for rent, false for free
        public bool GetBookRentStatus(Guid bookid)
        {
            bool free = false;
            MySqlCommand com = new MySqlCommand("SELECT readerid FROM Rental WHERE bookid=@id", Connection);
            com.Parameters.AddWithValue("@id", bookid.ToString());
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                while (rdr.Read())
                {
                    free = Guid.Parse(rdr.GetString("readerid")).Equals(Guid.Empty);
                }
            }
            return !free;
        }

        

        //adds a new book without rental
        public bool TryAdd(Guid bookid)
        {
            MySqlCommand com = new MySqlCommand("INSERT INTO Rental VALUES(@bookid, @readerid)", Connection);
            com.Parameters.AddWithValue("@bookid", bookid.ToString());
            com.Parameters.AddWithValue("@readerid", Guid.Empty.ToString());
            int updated = com.ExecuteNonQuery();
            return updated > 0 ? true : false;
        }

        public bool TryRemove(Guid bookid)
        {
            MySqlCommand com = new MySqlCommand("DELETE FROM Rental WHERE bookid=@id", Connection);
            com.Parameters.AddWithValue("@id", bookid.ToString());
            int updated = com.ExecuteNonQuery();
            return updated > 0 ? true : false;
        }

        public bool TryUpdate(Guid bookid, Guid readerid)
        {
            MySqlCommand com = new MySqlCommand("UPDATE Rental SET readerid=@readerid WHERE bookid=@bookid", Connection);
            com.Parameters.AddWithValue("@bookid", bookid.ToString());
            com.Parameters.AddWithValue("@readerid", readerid.ToString());
            int updated = com.ExecuteNonQuery();
            return updated > 0 ? true : false;
        }

        public OptionalGuid TryGet(Guid bookid)
        {
            OptionalGuid result = null;
            MySqlCommand com = new MySqlCommand("SELECT readerid FROM Rental WHERE bookid=@id", Connection);
            com.Parameters.AddWithValue("@id", bookid.ToString());
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result = new OptionalGuid()
                    {
                        Id = Guid.Parse(rdr.GetString("readerid")),
                        ContainsResult = rdr.HasRows
                    };
                }
            }
            return result;

        }


        public List<Guid> GetUserBooks(Reader reader)
        {
            List<Guid> res = new List<Guid>();
            MySqlCommand com = new MySqlCommand("SELECT bookid FROM Rental WHERE readerid=@readerid", Connection);
            com.Parameters.AddWithValue("@readerid", reader.Id.ToString());
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                while (rdr.Read())
                {
                    res.Add(Guid.Parse(rdr.GetString("bookid")));
                }
            }
            return res;
        }

        public Dictionary<Guid, Guid> GetRentBooks()
        {
            Dictionary <Guid, Guid> res= new Dictionary<Guid, Guid>();

            MySqlCommand com = new MySqlCommand("SELECT * FROM Rental WHERE readerid!=@readerid", Connection);
            com.Parameters.AddWithValue("@readerid", Guid.Empty.ToString());
            using (MySqlDataReader rdr = com.ExecuteReader())
            {

                while (rdr.Read())
                {
                    res.Add(Guid.Parse(rdr.GetString("bookid")), Guid.Parse(rdr.GetString("readerid")));

                }
            }
            return res;
        }

        


        public Dictionary<Guid, Guid> TryGetAll()
        {
            Dictionary<Guid, Guid> res = new Dictionary<Guid, Guid>();
            MySqlCommand com = new MySqlCommand("SELECT * FROM Rental", Connection);
            using (MySqlDataReader rdr = com.ExecuteReader())
            {

                while (rdr.Read())
                {
                    res.Add(Guid.Parse(rdr.GetString("bookid")), Guid.Parse(rdr.GetString("readerid")));

                }
            }
            return res;
        }
    }
}
