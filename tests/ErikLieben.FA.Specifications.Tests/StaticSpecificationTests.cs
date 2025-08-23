using System;
using ErikLieben.FA.Specifications;
using Xunit;

namespace ErikLieben.FA.Specifications.Tests;

public class StaticSpecificationTests
{
    public class Create
    {
        [Fact]
        public void Should_create_delegate_specification_and_evaluate_predicate()
        {
            // Arrange
            var spec = Specification.Create<int>(x => x % 2 == 0);

            // Act
            var trueResult = spec.IsSatisfiedBy(4);
            var falseResult = spec.IsSatisfiedBy(5);

            // Assert
            Assert.IsType<DelegateSpecification<int>>(spec);
            Assert.True(trueResult);
            Assert.False(falseResult);
        }
    }

    public class AlwaysTrue
    {
        [Fact]
        public void Should_always_return_true_for_any_input()
        {
            // Arrange
            var intSpec = Specification.AlwaysTrue<int>();
            var stringSpec = Specification.AlwaysTrue<string>();

            // Act
            var r1 = intSpec.IsSatisfiedBy(0);
            var r2 = intSpec.IsSatisfiedBy(123);
            var r3 = stringSpec.IsSatisfiedBy("anything");
            var r4 = stringSpec.IsSatisfiedBy(string.Empty);

            // Assert
            Assert.True(r1);
            Assert.True(r2);
            Assert.True(r3);
            Assert.True(r4);
        }
    }

    public class AlwaysFalse
    {
        [Fact]
        public void Should_always_return_false_for_any_input()
        {
            // Arrange
            var intSpec = Specification.AlwaysFalse<int>();
            var stringSpec = Specification.AlwaysFalse<string>();

            // Act
            var r1 = intSpec.IsSatisfiedBy(0);
            var r2 = intSpec.IsSatisfiedBy(123);
            var r3 = stringSpec.IsSatisfiedBy("anything");
            var r4 = stringSpec.IsSatisfiedBy(string.Empty);

            // Assert
            Assert.False(r1);
            Assert.False(r2);
            Assert.False(r3);
            Assert.False(r4);
        }
    }
}
