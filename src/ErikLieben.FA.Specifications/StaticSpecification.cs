namespace ErikLieben.FA.Specifications;

/// <summary>
/// Static factory for creating specifications
/// </summary>
public static class Specification
{
    /// <summary>
    /// Creates a specification from a predicate function
    /// </summary>
    /// <typeparam name="T">The type being validated</typeparam>
    /// <param name="predicate">The predicate function</param>
    /// <returns>A new specification</returns>
    public static Specification<T> Create<T>(Func<T, bool> predicate) =>
        new DelegateSpecification<T>(predicate);

    /// <summary>
    /// Creates a specification that is always satisfied
    /// </summary>
    /// <typeparam name="T">The type being validated</typeparam>
    /// <returns>A specification that always returns true</returns>
    public static Specification<T> AlwaysTrue<T>() => Create<T>(_ => true);

    /// <summary>
    /// Creates a specification that is never satisfied
    /// </summary>
    /// <typeparam name="T">The type being validated</typeparam>
    /// <returns>A specification that always returns false</returns>
    public static Specification<T> AlwaysFalse<T>() => Create<T>(_ => false);
}
