---
applyTo: "**/*Tests.cs,**/*Test.cs"
---

# Test File Instructions

When working with test files in PropertyTools:

## Test Framework
- Use NUnit framework with constraint syntax
- Use `Assert.That(actual, Is.EqualTo(expected))` instead of `Assert.AreEqual()`

## Naming Convention
**CRITICAL**: Follow the pattern `MethodName_StateUnderTest_ExpectedBehavior`

Examples:
- `Parse_Days_ReturnsCorrectValue`
- `ChangeAlpha_ValidColor_ReturnsCorrectValue`
- `HexToColor_InvalidColors_ReturnsUndefined`

Reference: [Roy Osherove's naming standards](https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html)

## Test Structure
Use the AAA pattern:
```csharp
[Test]
public void MethodName_StateUnderTest_ExpectedBehavior()
{
    // Arrange
    var input = ...;
    
    // Act
    var result = ...;
    
    // Assert
    Assert.That(result, Is.EqualTo(expected));
}
```

## Coverage Goals
- Target 80% or higher coverage for new code
- Test edge cases (null, empty, boundaries)
- Test error conditions

## Test Organization
- Use `[TestFixture]` for test classes
- Group related tests in the same class
- Name test classes as `{ClassUnderTest}Tests`
