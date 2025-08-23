# ErikLieben.FA.Specifications

[![NuGet](https://img.shields.io/nuget/v/ErikLieben.FA.Specifications?style=flat-square)](https://www.nuget.org/packages/ErikLieben.FA.Specifications)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue?style=flat-square)](https://dotnet.microsoft.com/download/dotnet/9.0)

> **Lightweight, AOT-friendly Specification pattern for clear, reusable domain rules — integrates seamlessly with ErikLieben.FA.Results and Validations.**

## 👋 A Friendly Note

This is an **opinionated library** built primarily for my own projects and coding style. You're absolutely free to use it (it's MIT licensed!), but please don't expect free support or feature requests. If it works for you, great! If not, there are many other excellent libraries in the .NET ecosystem.

That said, I do welcome bug reports and thoughtful contributions. If you're thinking about a feature or change, please open an issue first to discuss it — this helps avoid disappointment if it doesn't align with the library's direction. 😊

## 🚀 Why This Library?

A minimal, delegate-based implementation of the Specification pattern designed to be:

- **🎯 AOT-friendly** - No expression trees, works with Native AOT
- **🔗 Composable** - And, Or, Not combinators for complex rules
- **✅ Result-ready** - Seamless integration with validation flows
- **⚡ Lightweight** - Simple delegates, no heavy dependencies

Perfect for **domain modeling**, **business rules**, and **validation logic** that needs to be testable and reusable.

Specifications align with Domain-Driven Design principles by using the same business terminology that domain experts use, creating a **ubiquitous language** that bridges the communication gap between developers and business stakeholders. Business rules expressed as specifications can be communicated in terms similar to the domain's ubiquitous language, making them understandable to both technical and non-technical team members.

## ❌ When NOT to Use This Library

This library isn't right for every scenario. Consider alternatives when:

- **Simple validation is sufficient** - Basic boolean checks or built-in validation attributes might be enough
- **Query translation is required** - You need to convert business rules to SQL, Entity Framework expressions, or other query languages
- **Team unfamiliarity with patterns** - Your team isn't comfortable with the Specification pattern or Domain-Driven Design concepts
- **Performance is critical** - The abstraction layer may add overhead for high-throughput scenarios
- **Legacy integration constraints** - Existing codebase heavily relies on different validation approaches
- **Over-engineering risk** - Adding specifications would increase complexity without meaningful benefit

## 📦 Installation

```bash
# Core Specifications
dotnet add package ErikLieben.FA.Specifications

# Optional: Enhanced validation flows
dotnet add package ErikLieben.FA.Results
dotnet add package ErikLieben.FA.Results.Validations
```

**Requirements:** .NET 9.0+

## ⚡ Quick Start

```csharp
using ErikLieben.FA.Specifications;

// Define reusable business rules using DelegateSpecification and primary constructors
public sealed class AdultAgeSpecification() : DelegateSpecification<int>(age => age >= 18) {}

public sealed class RealisticAgeSpecification() : DelegateSpecification<int>(age => age <= 120) {}

// Combine specifications without repeating logic
public sealed class ValidAgeSpecification() : DelegateSpecification<int>(
    new AdultAgeSpecification()
        .And(new RealisticAgeSpecification())
        .ToPredicate()) {}

// Use anywhere
var validAge = new ValidAgeSpecification();
bool isValid = validAge.IsSatisfiedBy(25);   // true
bool tooOld = validAge.IsSatisfiedBy(150);   // false
bool tooYoung = validAge.IsSatisfiedBy(16);  // false
```

## 🏗️ Core Types

### Base Specification

```csharp
public abstract class Specification<T>
{
    // Your domain rule
    public abstract bool IsSatisfiedBy(T entity);
    
    // Combinators
    public Specification<T> And(Specification<T> other);
    public Specification<T> Or(Specification<T> other);
    public Specification<T> Not();
    
    // Conversions
    public Func<T, bool> ToPredicate();
    
    // Result integration (requires ErikLieben.FA.Results)
    public Result<T> Validate(T entity, string message, string? propertyName = null);
}
```

### Built-in Specifications

```csharp
// Wrap any predicate
var hasAtSymbol = new DelegateSpecification<string>(s => s.Contains('@'));

// Null checks for reference types
var notNull = new NotNullSpecification<string>();

// Convert predicates to specifications
Func<string, bool> containsDot = s => s.Contains('.');
var hasDot = containsDot.ToSpecification();
```

## 🔗 Composition Patterns

### And - All Must Pass

```csharp
public sealed class IsEvenSpecification : Specification<int>
{
    public override bool IsSatisfiedBy(int x) => x % 2 == 0;
}

public sealed class GreaterThan10Specification : Specification<int>
{
    public override bool IsSatisfiedBy(int x) => x > 10;
}

var evenAndLarge = new IsEvenSpecification().And(new GreaterThan10Specification());

// Test cases
evenAndLarge.IsSatisfiedBy(8);   // false (even but not > 10)
evenAndLarge.IsSatisfiedBy(12);  // true  (even AND > 10)
evenAndLarge.IsSatisfiedBy(15);  // false (> 10 but not even)
```

### Or - Any Must Pass

```csharp
var evenOrLarge = new IsEvenSpecification().Or(new GreaterThan10Specification());

// Test cases
evenOrLarge.IsSatisfiedBy(8);   // true  (even OR > 10)
evenOrLarge.IsSatisfiedBy(12);  // true  (even OR > 10)
evenOrLarge.IsSatisfiedBy(15);  // true  (even OR > 10)
evenOrLarge.IsSatisfiedBy(7);   // false (not even AND not > 10)
```

### Not - Invert Logic

```csharp
var oddNumbers = new IsEvenSpecification().Not();

oddNumbers.IsSatisfiedBy(3);  // true
oddNumbers.IsSatisfiedBy(4);  // false
```

### Complex Composition

```csharp
var between5and20 = new DelegateSpecification<int>(x => x >= 5 && x <= 20);
var isEven = new IsEvenSpecification();
var greaterThan10 = new GreaterThan10Specification();

// Complex business rule: "Between 5-20 and even, OR not greater than 10"
var complexRule = between5and20.And(isEven).Or(greaterThan10.Not());

// Test the rule
bool result = complexRule.IsSatisfiedBy(8);  // true (5-20 AND even)
```

## 📧 Email Validation Example

```csharp
// Build email validation from smaller specs
public sealed class ContainsAtSpecification : Specification<string>
{
    public override bool IsSatisfiedBy(string email) => email.Contains('@');
}

public sealed class ContainsDotSpecification : Specification<string>
{
    public override bool IsSatisfiedBy(string email) => email.Contains('.');
}

public sealed class MaxLengthSpecification : Specification<string>
{
    private readonly int _maxLength;
    
    public MaxLengthSpecification(int maxLength) => _maxLength = maxLength;
    
    public override bool IsSatisfiedBy(string email) => email.Length <= _maxLength;
}

// Compose into complete email validator
var emailSpecification = new ContainsAtSpecification()
    .And(new ContainsDotSpecification())
    .And(new MaxLengthSpecification(254));

// Use it
bool valid = emailSpecification.IsSatisfiedBy("user@example.com");  // true
bool invalid = emailSpecification.IsSatisfiedBy("not-an-email");    // false
```

## 🔄 Working with Collections

### Filter Collections

```csharp
var ages = new[] { 12, 18, 25, 7, 65, 120 };
var adultSpecification = new AdultAgeSpecification();

// Get only adults
var adults = adultSpecification.Filter(ages);  // [18, 25, 65, 120]

// Use with LINQ for more complex operations
var adultNames = people
    .Where(adultSpecification.ToPredicate())  // Convert to Func<Person, bool>
    .Select(p => p.Name)
    .ToList();
```

### Bulk Validation

```csharp
using ErikLieben.FA.Results;

var emails = new[] { "good@example.com", "bad-email", "also@good.com" };
var emailSpecification = new ContainsAtSpecification().And(new ContainsDotSpecification());

// Validate each item individually
var results = emailSpecification.ValidateAll(emails, "Invalid email format");

foreach (var result in results)
{
    if (result.IsFailure)
    {
        Console.WriteLine($"Error: {result.Errors[0].Message}");
    }
    else
    {
        Console.WriteLine($"Valid: {result.Value}");
    }
}
```

## ✅ Integration with Results

When used with [ErikLieben.FA.Results](../ErikLieben.FA.Results/README.md), specifications can produce validation results:

```csharp
using ErikLieben.FA.Results;
using ErikLieben.FA.Specifications;

var adultSpecification = new AdultAgeSpecification();

// Validate and get Result
var result = adultSpecification.Validate(25, "Must be 18 or older", "Age");
// Returns: Success(25)

var failed = adultSpecification.Validate(16, "Must be 18 or older", "Age");
// Returns: Failure with ValidationError("Must be 18 or older", "Age")

// Use in validation pipelines
if (result.IsSuccess)
{
    ProcessAdult(result.Value);
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.Message}");
    }
}
```

For more advanced validation scenarios with error aggregation, see [ErikLieben.FA.Results.Validations](../ErikLieben.FA.Results.Validations/README.md).

## 🎯 Why Delegates Over Expression Trees?

This library intentionally uses **delegates** instead of expression trees for several key reasons:

### ✅ AOT Compatibility
- Works perfectly with Native AOT compilation
- No runtime code generation required
- Smaller deployment footprint

### ✅ Simplicity
- Easy to understand and debug
- No complex expression tree manipulation
- Straightforward performance characteristics

### ✅ Portability
- Works in any .NET environment
- No dependencies on System.Linq.Expressions
- Compatible with trimming scenarios

### ❌ Trade-offs
- **Cannot translate to SQL** - specifications are not queryable
- **In-memory only** - not suitable for database query translation

## 📋 Complete Domain Example

```csharp
using ErikLieben.FA.Results;
using ErikLieben.FA.Specifications;

// Domain model
public record Product(string Name, decimal Price, string Category, bool IsActive);

// Business rule specifications
public sealed class ActiveProductSpecification : Specification<Product>
{
    public override bool IsSatisfiedBy(Product product) => product.IsActive;
}

public sealed class ReasonablePriceSpecification : Specification<Product>
{
    public override bool IsSatisfiedBy(Product product) 
        => product.Price > 0 && product.Price <= 10000;
}

public sealed class ValidCategorySpecification : Specification<Product>
{
    private static readonly string[] ValidCategories = { "Electronics", "Books", "Clothing" };
    
    public override bool IsSatisfiedBy(Product product) 
        => ValidCategories.Contains(product.Category);
}

// Compose business rules
var sellableProductSpecification = new ActiveProductSpecification()
    .And(new ReasonablePriceSpecification())
    .And(new ValidCategorySpecification());

// Apply to products
var products = new[]
{
    new Product("Laptop", 999.99m, "Electronics", true),
    new Product("Book", 19.99m, "Books", false),        // Not active
    new Product("Shirt", -5.00m, "Clothing", true),     // Invalid price
    new Product("Phone", 599.99m, "Electronics", true)
};

// Filter sellable products
var sellable = sellableProductSpecification.Filter(products);
// Result: [Laptop, Phone]

// Validate all products with detailed feedback
var validationResults = sellableProductSpecification.ValidateAll(products, "Product not sellable");

foreach (var (product, result) in products.Zip(validationResults))
{
    if (result.IsSuccess)
    {
        Console.WriteLine($"✅ {product.Name} is sellable");
    }
    else
    {
        Console.WriteLine($"❌ {product.Name}: {result.Errors[0].Message}");
    }
}
```

## 🔧 API Reference

### Core Types

| Type | Description |
|------|-------------|
| `Specification<T>` | Abstract base class for domain rules |
| `DelegateSpecification<T>` | Wraps a `Func<T, bool>` as a specification |
| `NotNullSpecification<T>` | Built-in null check for reference types |

### Combinators

| Method | Description | Example |
|--------|-------------|---------|
| `And(other)` | Both specifications must pass | `adult.And(realistic)` |
| `Or(other)` | Either specification must pass | `weekend.Or(holiday)` |
| `Not()` | Inverts the specification logic | `notEmpty.Not()` |

### Extension Methods

| Method | Description |
|--------|-------------|
| `ToSpecification<T>(this Func<T, bool>)` | Convert predicate to specification |
| `Filter<T>(this Specification<T>, IEnumerable<T>)` | Filter collection by specification |
| `ValidateAll<T>(this Specification<T>, IEnumerable<T>, string)` | Bulk validation with Results |

## 💡 Best Practices

### Do's ✅

- **Name specifications clearly** - `AdultAgeSpecification`, `ValidEmailSpecification`, `ActiveUserSpecification`
- **Keep specifications focused** - One business rule per specification
- **Compose complex rules** - Build from smaller, testable pieces
- **Test specifications independently** - Each specification should have its own tests
- **Use meaningful error messages** - Clear feedback for validation failures

### Don'ts ❌

- **Don't make specifications too complex** - Break down complex rules into smaller pieces
- **Don't depend on external state** - Specifications should be pure functions
- **Don't use for query translation** - This library is for in-memory evaluation only
- **Don't ignore composition** - Leverage And/Or instead of writing large specifications

### Example: Good vs Poor Specification Design

```csharp
// ❌ Poor: Complex, hard to test, unclear intent
public sealed class ComplexUserSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User user)
    {
        return user.Age >= 18 && 
               user.Age <= 65 && 
               !string.IsNullOrEmpty(user.Email) && 
               user.Email.Contains('@') && 
               user.IsActive && 
               user.LastLoginDate > DateTime.Now.AddMonths(-6);
    }
}

// ✅ Good: Focused, composable, clear intent
public sealed class AdultAgeSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User user) => user.Age >= 18 && user.Age <= 65;
}

public sealed class ValidEmailSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User user) 
        => !string.IsNullOrEmpty(user.Email) && user.Email.Contains('@');
}

public sealed class ActiveUserSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User user) => user.IsActive;
}

public sealed class RecentLoginSpecification : Specification<User>
{
    public override bool IsSatisfiedBy(User user) 
        => user.LastLoginDate > DateTime.Now.AddMonths(-6);
}

// Compose for complex rules
var eligibleUserSpecification = new AdultAgeSpecification()
    .And(new ValidEmailSpecification())
    .And(new ActiveUserSpecification())
    .And(new RecentLoginSpecification());
```

## 🔗 Related Libraries

- **[ErikLieben.FA.Results](../ErikLieben.FA.Results/README.md)** - Core Result types and operations
- **[ErikLieben.FA.Results.Validations](../ErikLieben.FA.Results.Validations/README.md)** - Advanced validation flows with error aggregation

## 📄 License

MIT License - see the [LICENSE](../../LICENSE) file for details.
