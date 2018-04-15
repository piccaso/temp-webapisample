using System;

namespace Library
{
    public interface ICounterFactory
    {
        ICounter Create(Guid id);
    }

    public class CounterFactory : ICounterFactory
    {
        private readonly IValueRepository _valueRepository;

        public CounterFactory(IValueRepository valueRepository)
        {
            _valueRepository = valueRepository;
        }

        public ICounter Create(Guid id)
        {
            return new Counter(id, _valueRepository);
        }
    }
}