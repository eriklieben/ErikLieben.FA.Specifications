namespace ErikLieben.FA.Specifications;

/// <summary>
/// Base class for all specifications - AOT compatible using delegates instead of expressions
/// </summary>
/// <typeparam name="T">The type being validated</typeparam>
public abstract class Specification<T>
{
    /// <summary>
    /// Determines if the entity satisfies the specification
    /// </summary>
    /// <param name="entity">The entity to validate</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public abstract bool IsSatisfiedBy(T entity);

    /// <summary>
    /// Combines this specification with another using logical AND
    /// </summary>
    /// <param name="other">The other specification</param>
    /// <returns>A new specification that is satisfied only when both specifications are satisfied</returns>
    public Specification<T> And(Specification<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return new AndSpecification<T>(this, other);
    }

    /// <summary>
    /// Combines this specification with another using logical OR
    /// </summary>
    /// <param name="other">The other specification</param>
    /// <returns>A new specification that is satisfied when either specification is satisfied</returns>
    public Specification<T> Or(Specification<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return new OrSpecification<T>(this, other);
    }

    /// <summary>
    /// Creates a specification that is the logical negation of this specification
    /// </summary>
    /// <returns>A new specification that is satisfied when this specification is not satisfied</returns>
    public Specification<T> Not() => new NotSpecification<T>(this);

    /// <summary>
    /// Converts the specification to a predicate function for use with LINQ (where supported)
    /// </summary>
    /// <returns>A function that can be used as a predicate</returns>
    public virtual Func<T, bool> ToPredicate() => IsSatisfiedBy;
}
