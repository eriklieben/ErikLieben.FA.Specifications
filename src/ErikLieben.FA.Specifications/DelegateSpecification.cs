namespace ErikLieben.FA.Specifications;

/// <summary>
/// Creates specifications from delegates - useful for simple cases
/// </summary>
public class DelegateSpecification<T>(
    Func<T, bool> predicate) : Specification<T>
{
    public override bool IsSatisfiedBy(T entity) => predicate(entity);
}
