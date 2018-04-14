using System;
using System.Collections.Generic;

namespace Library
{
    public interface IValueRepository
    {
        long Get(Guid guid);
        bool Set(Guid guid, long value);
        long Add(Guid guid, long by);
    }

    public class ValueRepository : IValueRepository
    {
        private readonly IDictionary<Guid, long> _storage = new Dictionary<Guid, long>();
        private readonly object _sync = new object();

        public long Get(Guid guid)
        {
            lock (_sync)
            {
                _storage.TryGetValue(guid, out var value);
                return value;
            }
        }


        public bool Set(Guid guid, long value)
        {
            lock (_sync)
            {
                _storage[guid] = value;
            }

            return true;
        }

        public long Add(Guid guid, long by)
        {
            long value;
            lock (_sync)
            {
                _storage.TryGetValue(guid, out value);
                value += by;
                _storage[guid] = value;
            }

            return value;
        }
    }
}
