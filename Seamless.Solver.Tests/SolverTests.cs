using Seamless.Solver;

namespace Seamless.Solver.Tests;

public class SolverTests
{
    [Fact]
    public void Solver_Solve_SatisfiableFormula_ReturnsTrue()
    {
        // Arrange
        var formula = new Formula(2,
            new Clause(new Literal(1)),
            new Clause(new Literal(2))
        );

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Solver_Solve_UnsatisfiableFormula_ReturnsFalse()
    {
        // Arrange
        var formula = new Formula(1,
            new Clause(new Literal(1)),
            new Clause(new Literal(1, true))
        );

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Solver_Solve_ComplexFormula_ReturnsTrue()
    {
        // Arrange
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2, true)),
            new Clause(new Literal(2), new Literal(3)),
            new Clause(new Literal(1, true), new Literal(3, true))
        );

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Solver_Solve_EmptyFormula_ReturnsTrue()
    {
        // Arrange
        var formula = new Formula(0);

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Solver_Solve_FormulaWithPureLiterals_ReturnsTrue()
    {
        // Arrange
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2)),
            new Clause(new Literal(1), new Literal(3, true)),
            new Clause(new Literal(2), new Literal(3))
        );

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.True(result);
    }
} 