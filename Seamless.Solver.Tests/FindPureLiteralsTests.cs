using Seamless.Solver;

namespace Seamless.Solver.Tests;

public class FindPureLiteralsTests
{
    [Fact]
    public void FindPureLiterals_SimplePureLiteral_ReturnsCorrectLiteral()
    {
        // Arrange
        var formula = new Formula(2,
            new Clause(new Literal(1)),
            new Clause(new Literal(2))
        );

        // Act
        var pureLiterals = Solver.FindPureLiterals(formula).ToList();

        // Assert
        Assert.Equal(2, pureLiterals.Count);
        Assert.Contains(new Literal(1), pureLiterals);
        Assert.Contains(new Literal(2), pureLiterals);
    }

    [Fact]
    public void FindPureLiterals_NoPureLiterals_ReturnsEmpty()
    {
        // Arrange
        var formula = new Formula(2,
            new Clause(new Literal(1), new Literal(2)),
            new Clause(new Literal(1, true), new Literal(2, true))
        );

        // Act
        var pureLiterals = Solver.FindPureLiterals(formula).ToList();

        // Assert
        Assert.Empty(pureLiterals);
    }

    [Fact]
    public void FindPureLiterals_MixedLiterals_ReturnsOnlyPureOnes()
    {
        // Arrange
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2)),
            new Clause(new Literal(1, true), new Literal(3)),
            new Clause(new Literal(2), new Literal(3))
        );

        // Act
        var pureLiterals = Solver.FindPureLiterals(formula).ToList();

        // Assert
        Assert.Equal(2, pureLiterals.Count);
        Assert.Contains(new Literal(2), pureLiterals);
        Assert.Contains(new Literal(3), pureLiterals);
    }

    [Fact]
    public void FindPureLiterals_EmptyFormula_ReturnsEmpty()
    {
        // Arrange
        var formula = new Formula(0);

        // Act
        var pureLiterals = Solver.FindPureLiterals(formula).ToList();

        // Assert
        Assert.Empty(pureLiterals);
    }

    [Fact]
    public void FindPureLiterals_SingleClause_ReturnsAllLiterals()
    {
        // Arrange
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2), new Literal(3))
        );

        // Act
        var pureLiterals = Solver.FindPureLiterals(formula).ToList();

        // Assert
        Assert.Equal(3, pureLiterals.Count);
        Assert.Contains(new Literal(1), pureLiterals);
        Assert.Contains(new Literal(2), pureLiterals);
        Assert.Contains(new Literal(3), pureLiterals);
    }
} 