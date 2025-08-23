using ErikLieben.FA.Specifications;
using Xunit;

namespace ErikLieben.FA.Specifications.Tests;

public class NotNullSpecificationTests
{
    public class IsSatisfiedBy
    {
        private sealed class MyRefType { public int X { get; set; } }

        [Fact]
        public void Should_return_true_when_reference_is_not_null()
        {
            // Arrange
            var sut = new NotNullSpecification<MyRefType>();
            var value = new MyRefType();

            // Act
            var result = sut.IsSatisfiedBy(value);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_reference_is_null()
        {
            // Arrange
            var sut = new NotNullSpecification<MyRefType>();

            // Act
            var result = sut.IsSatisfiedBy(null);

            // Assert
            Assert.False(result);
        }
    }
}
