using System;
using System.Collections.Generic;
using System.Text;
using Library.Domain;

namespace Library.Managers
{
    public class ReaderManager
    {
        private Dictionary<Guid, Reader> _readers = new Dictionary<Guid, Reader>();

        public ReaderManager()
        {

        }

        public bool ReaderExists(Reader reader)
        {
            return _readers.ContainsKey(reader.Id);
        }

        public List<Reader> TryGetAll()
        {

            List<Reader> result = new List<Reader>();
            foreach (Reader reader in _readers.Values)
            {
                result.Add(new Reader(reader.Id, reader.Name));
            }
            return result;
        }

        public Reader TryPickByName(string name)
        {
            foreach (Reader reader in _readers.Values)
            {
                if (reader.Name.Equals(name))
                {
                    return reader;
                }
            }
            return null;
        }

        public bool TryAdd(Reader reader)
        {
            if (_readers.ContainsKey(reader.Id))
            {
                return false;
            }
            foreach (Reader r in _readers.Values)
            {
                if (r.Name.ToLower().Equals(reader.Name.ToLower()))
                {
                    return false;
                }
            }
            long startCount = _readers.Count;
            _readers.Add(reader.Id, reader);
            return startCount != _readers.Count;
        }

        public bool TryRemove(Guid readerid)
        {
            long startCount = _readers.Count;
            if (!_readers.ContainsKey(readerid))
            {
                return false;
            }
            _readers.Remove(readerid);
            return startCount - _readers.Count == 1;
        }

        public Reader TryGet(Guid id)
        {
            if (!_readers.ContainsKey(id))
            {
                return null;
            }
            return _readers[id];
        }
    }
}
