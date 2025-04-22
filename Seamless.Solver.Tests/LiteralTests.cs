using Seamless.Solver;

namespace Seamless.Solver.Tests;

public class LiteralTests
{
    [Fact]
    public void Literal_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var literal = new Literal(1, true);

        // Assert
        Assert.Equal(1, literal.Variable);
        Assert.True(literal.IsNegated);
    }

    [Fact]
    public void Literal_DefaultConstructor_SetsIsNegatedToFalse()
    {
        // Arrange & Act
        var literal = new Literal(1);

        // Assert
        Assert.Equal(1, literal.Variable);
        Assert.False(literal.IsNegated);
    }

    [Fact]
    public void Literal_Negate_ReturnsNegatedLiteral()
    {
        // Arrange
        var literal = new Literal(1, true);

        // Act
        var negated = literal.Negate();

        // Assert
        Assert.Equal(1, negated.Variable);
        Assert.False(negated.IsNegated);
    }

    [Fact]
    public void Literal_ToString_ReturnsCorrectFormat()
    {
        // Arrange
        var positive = new Literal(1);
        var negative = new Literal(1, true);

        // Act & Assert
        Assert.Equal("1", positive.ToString());
        Assert.Equal("¬1", negative.ToString());
    }
} 