using System;
using System.Linq;
using ErikLieben.FA.Specifications;
using Xunit;

namespace ErikLieben.FA.Specifications.Tests;

public class SpecificationTests
{
    private sealed class EvenSpec : Specification<int>
    {
        public override bool IsSatisfiedBy(int entity) => entity % 2 == 0;
    }

    private sealed class GreaterThanSpec : Specification<int>
    {
        private readonly int threshold;
        public GreaterThanSpec(int threshold) => this.threshold = threshold;
        public override bool IsSatisfiedBy(int entity) => entity > threshold;
    }

    public class And
    {
        [Fact]
        public void Should_return_true_when_both_are_satisfied()
        {
            // Arrange
            var left = new EvenSpec();
            var right = new GreaterThanSpec(10);
            var sut = left.And(right);

            // Act
            var result = sut.IsSatisfiedBy(12);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_left_is_false()
        {
            // Arrange
            var left = new EvenSpec();
            var right = new GreaterThanSpec(10);
            var sut = left.And(right);

            // Act
            var result = sut.IsSatisfiedBy(11); // not even

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_throw_when_other_is_null()
        {
            // Arrange
            var sut = new EvenSpec();

            // Act
            Func<Specification<int>> act = () => sut.And(null!);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }
    }

    public class Or
    {
        [Fact]
        public void Should_return_true_when_either_is_true()
        {
            // Arrange
            var left = new EvenSpec();
            var right = new GreaterThanSpec(10);
            var sut = left.Or(right);

            // Act
            var trueByLeft = sut.IsSatisfiedBy(8);
            var trueByRight = sut.IsSatisfiedBy(11);

            // Assert
            Assert.True(trueByLeft);
            Assert.True(trueByRight);
        }

        [Fact]
        public void Should_throw_when_other_is_null()
        {
            // Arrange
            var sut = new EvenSpec();

            // Act
            Func<Specification<int>> act = () => sut.Or(null!);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }
    }

    public class Not
    {
        [Fact]
        public void Should_invert_the_result()
        {
            // Arrange
            var baseSpec = new EvenSpec();
            var sut = baseSpec.Not();

            // Act
            var whenBaseTrue = sut.IsSatisfiedBy(2);
            var whenBaseFalse = sut.IsSatisfiedBy(3);

            // Assert
            Assert.False(whenBaseTrue);
            Assert.True(whenBaseFalse);
        }
    }

    public class ToPredicate
    {
        [Fact]
        public void Should_convert_to_predicate_that_invokes_IsSatisfiedBy()
        {
            // Arrange
            var sut = new GreaterThanSpec(5);

            // Act
            var predicate = sut.ToPredicate();
            var result = predicate(6);

            // Assert
            Assert.True(result);
        }
    }
}
