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
        Assert.True(result.Result);
        Assert.NotNull(result.Assignment);
        Assert.True(result.Assignment[1]);
        Assert.True(result.Assignment[2]);
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
        Assert.False(result.Result);
        Assert.Null(result.Assignment);
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
        Assert.True(result.Result);
        Assert.NotNull(result.Assignment);
    }

    [Fact]
    public void Solver_Solve_EmptyFormula_ReturnsTrue()
    {
        // Arrange
        var formula = new Formula(0);

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.True(result.Result);
        Assert.NotNull(result.Assignment);
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
        Assert.True(result.Result);
    }

    [Fact]
    public void Solver_Solve_RequiresMultipleBacktracking_ReturnsTrue()
    {
        // Arrange
        // This formula requires multiple levels of backtracking:
        // 1. First assignment of x1=true leads to a conflict
        // 2. Backtrack to x1=false
        // 3. Then x2=true leads to another conflict
        // 4. Backtrack to x2=false
        // 5. Finally find the satisfying assignment
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2)),           // x1 ∨ x2
            new Clause(new Literal(1, true), new Literal(3)),     // ¬x1 ∨ x3
            new Clause(new Literal(2, true), new Literal(3, true)), // ¬x2 ∨ ¬x3
            new Clause(new Literal(1), new Literal(3, true))      // x1 ∨ ¬x3
        );

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.True(result.Result);
        Assert.NotNull(result.Assignment);
    }

    [Fact]
    public void Solver_Solve_GroupedVariables_ReturnsTrue()
    {
        // Arrange
        // This formula has groups of variables where at least one must be true
        // Similar to the structure in the failing case, but with fewer variables
        var formula = new Formula(6,
            // First group: at least one of x1,x2,x3 must be true
            new Clause(new Literal(1), new Literal(2)),           // x1 ∨ x2
            new Clause(new Literal(1), new Literal(3)),           // x1 ∨ x3
            new Clause(new Literal(2), new Literal(3)),           // x2 ∨ x3
            
            // Second group: at least one of x4,x5,x6 must be true
            new Clause(new Literal(4), new Literal(5)),           // x4 ∨ x5
            new Clause(new Literal(4), new Literal(6)),           // x4 ∨ x6
            new Clause(new Literal(5), new Literal(6)),           // x5 ∨ x6
            
            // Additional constraints that force backtracking
            new Clause(new Literal(1, true), new Literal(4)),     // ¬x1 ∨ x4
            new Clause(new Literal(2, true), new Literal(5)),     // ¬x2 ∨ x5
            new Clause(new Literal(3, true), new Literal(6))      // ¬x3 ∨ x6
        );

        // Act
        var result = Solver.Solve(formula);

        // Assert
        Assert.True(result.Result);
        Assert.NotNull(result.Assignment);
    }
} 