using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain;

namespace Library.Model
{
    public class AdminModel
    {
        private List<Admin> _admins = new List<Admin>();

        public AdminModel()
        {
            
        }

        public bool TryAdd(Admin newAdmin)
        {
            if (!_admins.Contains(newAdmin))
            {
                _admins.Add(newAdmin);
                return true;
            }
            return false;
        }

        public bool TryRemove(Admin admin)
        {
            if (_admins.Contains(admin))
            {
                _admins.Remove(admin);
                return true;
            }
            return false;
        }

        public bool CheckRights(Reader reader)
        {
            foreach (Admin admin in _admins)
            {
                if (admin.id.Equals(reader.id))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
