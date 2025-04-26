using Seamless.Solver;

namespace Seamless.Solver.Tests;

public class ClauseTests
{
    [Fact]
    public void Clause_Constructor_SetsLiteralsCorrectly()
    {
        // Arrange
        var literals = new[] { new Literal(1), new Literal(2, true) };

        // Act
        var clause = new Clause(literals);

        // Assert
        Assert.Equal(2, clause.Literals.Count);
        Assert.Contains(new Literal(1), clause.Literals);
        Assert.Contains(new Literal(2, true), clause.Literals);
    }

    [Fact]
    public void Clause_ParamsConstructor_SetsLiteralsCorrectly()
    {
        // Arrange & Act
        var clause = new Clause(new Literal(1), new Literal(2, true));

        // Assert
        Assert.Equal(2, clause.Literals.Count);
        Assert.Contains(new Literal(1), clause.Literals);
        Assert.Contains(new Literal(2, true), clause.Literals);
    }

    [Fact]
    public void Clause_IsEmpty_ReturnsTrueForEmptyClause()
    {
        // Arrange & Act
        var clause = new Clause();

        // Assert
        Assert.True(clause.IsEmpty);
    }

    [Fact]
    public void Clause_IsUnit_ReturnsTrueForUnitClause()
    {
        // Arrange & Act
        var clause = new Clause(new Literal(1));

        // Assert
        Assert.True(clause.IsUnit);
    }

    [Fact]
    public void Clause_ToString_ReturnsCorrectFormat()
    {
        // Arrange
        var clause = new Clause(new Literal(1), new Literal(2, true), new Literal(3));

        // Act & Assert
        Assert.Equal("(1 ∨ ¬2 ∨ 3)", clause.ToString());
    }
} 