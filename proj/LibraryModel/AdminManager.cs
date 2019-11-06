using System;
using System.Collections.Generic;
using Library.Domain;

namespace Library.Managers
{
    public class AdminManager
    {
        private List<Admin> _admins = new List<Admin>();

        public AdminManager()
        {
            
        }


        public List<Admin> TryGetAll()
        {
            List<Admin> res = new List<Admin>();
            foreach (Admin adm in _admins)
            {
                res.Add(new Admin(adm.Id, adm.Name));
            }
            return res;

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
                if (admin.Id.Equals(reader.Id))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
