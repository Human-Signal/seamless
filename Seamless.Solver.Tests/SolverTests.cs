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
        var solver = new Solver(formula);

        // Act
        var result = solver.Solve();

        // Assert
        Assert.True(result);
        var assignment = solver.GetAssignment();
        Assert.True(assignment[1]);
        Assert.True(assignment[2]);
    }

    [Fact]
    public void Solver_Solve_UnsatisfiableFormula_ReturnsFalse()
    {
        // Arrange
        var formula = new Formula(1,
            new Clause(new Literal(1)),
            new Clause(new Literal(1, true))
        );
        var solver = new Solver(formula);

        // Act
        var result = solver.Solve();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Solver_Solve_ComplexFormula_ReturnsCorrectAssignment()
    {
        // Arrange
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2, true)),
            new Clause(new Literal(2), new Literal(3)),
            new Clause(new Literal(1, true), new Literal(3, true))
        );
        var solver = new Solver(formula);

        // Act
        var result = solver.Solve();

        // Assert
        Assert.True(result);
        var assignment = solver.GetAssignment();
        Assert.True(assignment[1]);
        Assert.True(assignment[2]);
        Assert.False(assignment[3]);
    }

    [Fact]
    public void Solver_Solve_EmptyFormula_ReturnsTrue()
    {
        // Arrange
        var formula = new Formula(0);
        var solver = new Solver(formula);

        // Act
        var result = solver.Solve();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Solver_Solve_FormulaWithPureLiterals_ReturnsCorrectAssignment()
    {
        // Arrange
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2)),
            new Clause(new Literal(1), new Literal(3, true)),
            new Clause(new Literal(2), new Literal(3))
        );
        var solver = new Solver(formula);

        // Act
        var result = solver.Solve();

        // Assert
        Assert.True(result);
        var assignment = solver.GetAssignment();
        Assert.True(assignment[1]);
        Assert.True(assignment[2]);
        // x3's value doesn't matter as the formula is already satisfied by x1 and x2
    }
} 