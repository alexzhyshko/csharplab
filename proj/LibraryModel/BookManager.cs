using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Library.Domain;
using MySql.Data.MySqlClient;

namespace Library.Managers
{
    public class BookManager
    {

        public BookManager()
        {

        }

        public MySqlConnection Connection { get; set; }
      
        public List<Book> Find(string text)
        {
            List<Book> res = new List<Book>();
            MySqlCommand com = new MySqlCommand("SELECT * FROM Books WHERE name LIKE @name", Connection);
            com.Parameters.AddWithValue("@name", "%"+text+"%");
            using (MySqlDataReader rdr = com.ExecuteReader())
            {

                while (rdr.Read())
                {
                    res.Add(new Book(Guid.Parse(rdr.GetString("id")), rdr.GetString("name"), GetAuthors(Guid.Parse(rdr.GetString("id")))));
                }
            }
            return res;
        }

        public Book TryPickByName(string name)
        {
            Book result = null;
            MySqlCommand com = new MySqlCommand("SELECT * FROM Books WHERE name=@name LIMIT=1", Connection);
            com.Parameters.AddWithValue("@name", name);
            bool ready = false;
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                ready = rdr.HasRows;
                while (rdr.Read())
                {
                    result = new Book(new Guid(rdr.GetString("id")), rdr.GetString("name"), GetAuthors(Guid.Parse(rdr.GetString("id"))));
                }
            }
            return ready ? result : null;
        }

        public bool TryAdd(Book book)
        {
            MySqlCommand com = new MySqlCommand("INSERT INTO Books VALUES(@id, @name)", Connection);
            com.Parameters.AddWithValue("@id", book.Id.ToString());
            com.Parameters.AddWithValue("@name", book.Name);
            int updated = com.ExecuteNonQuery();
            if (updated>0) {
                foreach (Author author in book.Authors)
                {
                    MySqlCommand com2 = new MySqlCommand("INSERT INTO Authors VALUES(@id, @name)", Connection);
                    com2.Parameters.AddWithValue("@id", author.Id.ToString());
                    com2.Parameters.AddWithValue("@name", author.Name);
                    com2.ExecuteNonQuery();

                    MySqlCommand com3 = new MySqlCommand("INSERT INTO BookAuthor VALUES(@bookid, @authorid)", Connection);
                    com3.Parameters.AddWithValue("@bookid", book.Id.ToString());
                    com3.Parameters.AddWithValue("@authorid", author.Id.ToString());
                    com3.ExecuteNonQuery();
                }
            }
            return updated > 0 ? true : false;

        }

        public bool TryRemove(Guid id)
        {
            MySqlCommand com0 = new MySqlCommand("SELECT readerid FROM Rental WHERE bookid=@bookid", Connection);
            com0.Parameters.AddWithValue("@bookid", id.ToString());
            bool free = false;
            using (MySqlDataReader rdr = com0.ExecuteReader())
            {
                while (rdr.Read())
                {
                    free = Guid.Parse(rdr.GetString("readerid")).Equals(Guid.Empty);
                }
            }

            if (free) {

                MySqlCommand deletecom = new MySqlCommand("DELETE FROM Rental WHERE bookid=@bookid", Connection);
                deletecom.Parameters.AddWithValue("@bookid", id.ToString());
                deletecom.ExecuteNonQuery();


                MySqlCommand com = new MySqlCommand("DELETE FROM BookAuthor WHERE bookid=@bookid", Connection);
                com.Parameters.AddWithValue("@bookid", id.ToString());
                int updated = com.ExecuteNonQuery();
                if (updated > 0)
                {
                    MySqlCommand com2 = new MySqlCommand("DELETE FROM Books WHERE id=@id", Connection);
                    com2.Parameters.AddWithValue("@id", id.ToString());
                    com2.ExecuteNonQuery();
                }
                return updated > 0 ? true : false;
            }
            else
            {
                return false;
            }
            
        }

        public Book TryGet(Guid id)
        {
            Book result = null;
            using (MySqlConnection tempcon = new MySqlConnection("server=localhost;port=3306;uid=root;pwd=root;database=Library")) {
                tempcon.Open();
                MySqlCommand com = new MySqlCommand("SELECT * FROM Books WHERE id=@id", tempcon);
                com.Parameters.AddWithValue("@id", id.ToString());
                using (MySqlDataReader rdr = com.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result = new Book(Guid.Parse(rdr.GetString("id")), rdr.GetString("name"), GetAuthors(Guid.Parse(rdr.GetString("id"))));
                    }
                }
            }


            return result;
        }
        
        public List<Book> TryGetAll()
        {

            List<Book> res = new List<Book>();
            MySqlCommand com = new MySqlCommand("SELECT id FROM Books", Connection);
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                while (rdr.Read())
                {
                    res.Add(TryGet(Guid.Parse(rdr.GetString("id"))));
                }
            }

               
            return res;

        }


        public List<Book> GetNonRentBooks()
        {
            List<Book> res = new List<Book>();

            MySqlCommand com = new MySqlCommand("SELECT * FROM Books WHERE id IN (SELECT bookid FROM Rental WHERE readerid=@emptyid)", Connection);
            com.Parameters.AddWithValue("@emptyid", Guid.Empty.ToString());
            using (MySqlDataReader rdr = com.ExecuteReader())
            {

                while (rdr.Read())
                {
                    res.Add(new Book(Guid.Parse(rdr.GetString("id")), rdr.GetString("name"), GetAuthors(Guid.Parse(rdr.GetString("id")))));

                }
            }
            return res;
        }


        private List<Author> GetAuthors(Guid bookid)
        {
            List<Author> authors = new List<Author>();
            using (MySqlConnection tempcon = new MySqlConnection("server=localhost;port=3306;uid=root;pwd=root;database=Library"))
            {
                tempcon.Open();
                MySqlCommand com = new MySqlCommand("SELECT * FROM Authors WHERE id=(SELECT authorid FROM BookAuthor WHERE bookid=@bookid)", tempcon);
                com.Parameters.AddWithValue("@bookid", bookid.ToString());
                using (MySqlDataReader rdr = com.ExecuteReader())
                {

                    while (rdr.Read())
                    {
                        authors.Add(new Author(Guid.Parse(rdr.GetString("id")), rdr.GetString("name")));
                    }
                }
            }
            return authors;
        }
    }
}
