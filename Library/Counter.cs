using System;

namespace Library
{
    public interface ICounter
    {
        long Get();
        long Increment(long by);
        void Set(long value);
    }

    public class Counter : ICounter
    {
        private readonly Guid _id;
        private readonly IValueRepository _repository;

        internal Counter(Guid id, IValueRepository repository)
        {
            _id = id;
            _repository = repository;
        }

        public long Get() => _repository.Get(_id);
        public long Increment(long by) => _repository.Add(_id, by);
        public void Set(long value) => _repository.Set(_id, value);
    }
}