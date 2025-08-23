namespace ErikLieben.FA.Specifications;

/// <summary>
/// Internal implementation of OR specification combinator
/// </summary>
internal sealed class OrSpecification<T>(Specification<T> left, Specification<T> right)
    : Specification<T>
{
    public override bool IsSatisfiedBy(T entity) =>
        left.IsSatisfiedBy(entity) || right.IsSatisfiedBy(entity);
}
