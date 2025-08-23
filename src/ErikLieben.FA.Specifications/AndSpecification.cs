namespace ErikLieben.FA.Specifications;

/// <summary>
/// Internal implementation of AND specification combinator
/// </summary>
internal sealed class AndSpecification<T>(Specification<T> left, Specification<T> right)
    : Specification<T>
{
    public override bool IsSatisfiedBy(T entity) =>
        left.IsSatisfiedBy(entity) && right.IsSatisfiedBy(entity);
}
