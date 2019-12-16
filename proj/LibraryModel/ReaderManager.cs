using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain;
using MySql.Data.MySqlClient;

namespace Library.Managers
{
    public class ReaderManager
    {
        public MySqlConnection Connection { get; set; }

        public ReaderManager()
        {

        }

        public bool ReaderExists(Reader reader)
        {
            MySqlCommand com = new MySqlCommand("SELECT * FROM Readers WHERE username=@name", Connection);
            com.Parameters.AddWithValue("@name", reader.Name);
            MySqlDataReader rdr = com.ExecuteReader();
            bool ready = rdr.HasRows;
            rdr.Close();
            return ready;
        }

        public List<Reader> TryGetAll()
        {
            List<Reader> res = new List<Reader>();
            MySqlCommand com = new MySqlCommand("SELECT * FROM Readers", Connection);
            MySqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                res.Add(new Reader(Guid.Parse(rdr.GetString(0)), rdr.GetString(1)));
            }
            rdr.Close();
            return res;
        }

        public Reader TryPickByName(string name)
        {
            Reader result = null;
            MySqlCommand com = new MySqlCommand("SELECT * FROM Readers WHERE username=@name", Connection);
            com.Parameters.AddWithValue("@name", name);
            MySqlDataReader rdr = com.ExecuteReader();
            bool ready = rdr.HasRows;
            while (rdr.Read())
            {
                result = new Reader(new Guid(rdr.GetString(0)), rdr.GetString(1));
            }
            rdr.Close();
            return ready ? result : null;
        }

        public bool TryAdd(Reader reader)
        {
            MySqlCommand com = new MySqlCommand("INSERT INTO Readers VALUES(@id, @username)", Connection);
            com.Parameters.AddWithValue("@id", reader.Id.ToString());
            com.Parameters.AddWithValue("@username", reader.Name);
            int updated = com.ExecuteNonQuery();
            return updated > 0 ? true : false;
        }

        public bool TryRemove(Guid readerid)
        {
            MySqlCommand com = new MySqlCommand("DELETE FROM Readers WHERE id=@id", Connection);
            com.Parameters.AddWithValue("@id", readerid.ToString());
            int updated = com.ExecuteNonQuery();
            return updated > 0 ? true : false;
        }

        public Reader TryGet(Guid id)
        {
            bool ready = false;
            Reader result = null;
            MySqlCommand com = new MySqlCommand("SELECT * FROM Readers WHERE id=@id", Connection);
            com.Parameters.AddWithValue("@id", id.ToString());
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                ready = rdr.HasRows;
                while (rdr.Read())
                {
                    result = new Reader(new Guid(rdr.GetString(0)), rdr.GetString(1));
                }
            }
            return ready ? result : null;
        }
    }
}
