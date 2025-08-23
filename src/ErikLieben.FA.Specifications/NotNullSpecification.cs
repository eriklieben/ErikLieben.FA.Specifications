namespace ErikLieben.FA.Specifications;

/// <summary>
/// Validates that a reference type is not null
/// </summary>
public sealed class NotNullSpecification<T> : Specification<T?> where T : class
{
    public override bool IsSatisfiedBy(T? entity)
    {
        return entity is not null;
    }
}
