using System;
using System.Threading.Tasks;
using Library;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ValueRepositoryTest
    {
        private IValueRepository _valueRepository;
        [SetUp]
        public void SetUp()
        {
            _valueRepository = new ValueRepository();
        }

        [Test]
        public void IncrementOne()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            const long initialValue = 50L;
            const long incrementBy = 20L;
            const long expectedResult = 70L;

            // Act
            var setResult = _valueRepository.Set(uuid, initialValue);
            var incrementResult = _valueRepository.Add(uuid, incrementBy);

            // Assert
            Assert.IsTrue(setResult);
            Assert.AreEqual(incrementResult, expectedResult);
        }

        [Test]
        public void GetOneItem()
        {
            // Arrange
            var uuid = Guid.NewGuid();

            // Act
            var result = _valueRepository.Get(uuid);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void AddOneGetOne()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            const long value = 100L;

            // Act
            var setResult = _valueRepository.Set(uuid, value);
            var getResult = _valueRepository.Get(uuid);

            // Assert
            Assert.IsTrue(setResult);
            Assert.AreEqual(value,getResult);
        }

        [Test]
        public void Threads()
        {
            // Arrange
            Parallel.For(0, 10, (i,s) =>
            {
                // Act
                var uuid = Guid.NewGuid();
                var value = i + 100L;
                var setResult = _valueRepository.Set(uuid, value);
                var addResult = _valueRepository.Add(uuid, 500);
                var getResult = _valueRepository.Get(uuid);

                // Assert
                Assert.IsTrue(setResult);
                Assert.AreEqual(value + 500, getResult);
                Assert.AreEqual(addResult,getResult);
            });
        }
    }
}
