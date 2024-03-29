using System.Linq;
using DataRetrievers;
using NUnit.Framework;

namespace DataRetrievers.Tests
{
    public partial class SortingTests
    {
        [Test]
        public void DescendingShouldReturnDescendingSortingWithCorrectFieldName()
        {
            //arrange
            //act
            var result = Sorting<FakeProjection>.Descending((p) => p.Id);

            //assert
            Assert.AreEqual(nameof(FakeProjection.Id), result.FieldName);
            Assert.IsTrue(result.IsDescending);
        }

        [Test]
        public void AscendingShouldReturnDescendingSortingWithCorrectFieldName()
        {
            //arrange
            //act
            var result = Sorting<FakeProjection>.Ascending((p) => p.Id);

            //assert
            Assert.AreEqual(nameof(FakeProjection.Id), result.FieldName);
            Assert.IsFalse(result.IsDescending);


        }
    }
}

    