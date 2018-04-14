using System;

namespace Library
{
    public interface ICounter
    {
        long Get(Guid guid);
        long Increment(Guid guid, long by);
        long Set(Guid guid, long value);
    }

    public class Counter : ICounter
    {
        private readonly IValueRepository _valueRepository;

        public Counter(IValueRepository valueRepository)
        {
            _valueRepository = valueRepository;
        }
        
        public long Set(Guid guid, long value)
        {
            _valueRepository.Set(guid, value);

            return value;
        }

        public long Increment(Guid guid, long by) => _valueRepository.Add(guid, by);
        public long Get(Guid guid) => _valueRepository.Get(guid);
    }
}