using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain
{
    public class Admin : Reader
    {
        public Admin(Guid id, string name)
            : base(id, name)
        {
        }
    }
}
