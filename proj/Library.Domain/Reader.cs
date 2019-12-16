using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain
{
    public class Reader : Person
    {
        public bool Blocked { get; private set; } = false;

        public Reader(Guid id, string name)
            : base(id, name)
        {
        }

        public bool TryBlock()
        {
            if (!Blocked)
            {
                Blocked = true;
                return true;
            }

            return false;
        }

        public bool TryUnbock()
        {
            if (Blocked)
            {
                Blocked = false;
                return true;
            }

            return false;
        }
    }
}