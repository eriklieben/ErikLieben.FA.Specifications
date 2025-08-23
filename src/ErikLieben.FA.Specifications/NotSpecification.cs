namespace ErikLieben.FA.Specifications;

/// <summary>
/// Internal implementation of NOT specification combinator
/// </summary>
internal sealed class NotSpecification<T>(Specification<T> specification)
    : Specification<T>
{
    public override bool IsSatisfiedBy(T entity) => !specification.IsSatisfiedBy(entity);
}
