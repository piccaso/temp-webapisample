using System;
using System.Threading.Tasks;
using Library;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CounterTests
    {
        private ICounterFactory _counterFactory;
        [SetUp]
        public void SetUp()
        {
            _counterFactory = new CounterFactory(new ValueRepository());
        }

        [Test]
        public void Threads()
        {
            // Arrange
            Parallel.For(0, 10, (i, s) =>
            {
                var counter = _counterFactory.Create(Guid.NewGuid());
                var value = i + 100L;

                // Act
                counter.Set(value);
                var addResult = counter.Increment(500);
                var getResult = counter.Get();

                // Assert
                Assert.AreEqual(value + 500, getResult);
                Assert.AreEqual(addResult, getResult);
            });
        }
    }
}
