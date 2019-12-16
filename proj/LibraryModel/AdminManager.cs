using System;
using System.Collections.Generic;
using Library.Domain;
using MySql.Data.MySqlClient;

namespace Library.Managers
{
    public class AdminManager
    {
        public AdminManager()
        {
            
        }

        public MySqlConnection Connection { get; set; }

        public List<Admin> TryGetAll()
        {
            List<Admin> res = new List<Admin>();
            MySqlCommand com = new MySqlCommand("SELECT * FROM Admins", Connection);
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                while (rdr.Read())
                {
                    res.Add(new Admin(Guid.Parse(rdr.GetString(0)), rdr.GetString(1)));
                }
            }
            return res;

        }

        public Admin GetByName(string username)
        {
            Admin result = null;
            MySqlCommand com = new MySqlCommand("SELECT * FROM Admins WHERE username=@name", Connection);
            com.Parameters.AddWithValue("@name", username);
            bool ready = false;
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                ready = rdr.HasRows;
                while (rdr.Read())
                {
                    result = new Admin(new Guid(rdr.GetString(0)), rdr.GetString(1));
                }
            }
            return ready?result:null;
        }

        public bool TryAdd(Admin newAdmin)
        {
            MySqlCommand com = new MySqlCommand("INSERT INTO Admins VALUES(@id, @username)", Connection);
            com.Parameters.AddWithValue("@id", newAdmin.Id.ToString());
            com.Parameters.AddWithValue("@username", newAdmin.Name);
            int updated = com.ExecuteNonQuery();
            return updated > 0 ? true : false;
        }

        public bool TryRemove(Admin admin)
        {
            MySqlCommand com = new MySqlCommand("DELETE FROM Admins WHERE id=@id", Connection);
            com.Parameters.AddWithValue("@id", admin.Id.ToString());
            int updated = com.ExecuteNonQuery();
            return updated > 0 ? true : false;
        }

        public bool CheckRights(Reader reader)
        {
            MySqlCommand com = new MySqlCommand("SELECT * FROM Admins WHERE username=@name", Connection);
            com.Parameters.AddWithValue("@name", reader.Name);
            bool ready = false;
            using (MySqlDataReader rdr = com.ExecuteReader())
            {
                ready = rdr.HasRows;
            }
            return ready;
        }

    }
}
