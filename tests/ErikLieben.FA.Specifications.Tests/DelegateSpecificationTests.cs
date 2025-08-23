using ErikLieben.FA.Specifications;
using Xunit;

namespace ErikLieben.FA.Specifications.Tests;

public class DelegateSpecificationTests
{
    public class IsSatisfiedBy
    {
        [Fact]
        public void Should_return_true_when_predicate_returns_true()
        {
            // Arrange
            var sut = new DelegateSpecification<int>(x => x % 2 == 0);

            // Act
            var result = sut.IsSatisfiedBy(4);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_predicate_returns_false()
        {
            // Arrange
            var sut = new DelegateSpecification<int>(x => x > 10);

            // Act
            var result = sut.IsSatisfiedBy(3);

            // Assert
            Assert.False(result);
        }
    }
}
