using Seamless.Solver;

namespace Seamless.Solver.Tests;

public class SimplifyTests
{
    [Fact]
    public void Simplify_SingleClause_AssignmentSatisfiesClause_ReturnsEmptyFormula()
    {
        // Arrange
        var formula = new Formula(1, new Clause(new Literal(1)));
        
        // Act
        var result = Solver.Simplify(formula, 1, true);
        
        // Assert
        Assert.Equal("", result.ToString());
    }

    [Fact]
    public void Simplify_SingleClause_AssignmentDoesNotSatisfyClause_ReturnsSameClause()
    {
        // Arrange
        var formula = new Formula(1, new Clause(new Literal(1)));
        
        // Act
        var result = Solver.Simplify(formula, 1, false);
        
        // Assert
        Assert.Equal("()", result.ToString());
    }

    [Fact]
    public void Simplify_MultipleClauses_AssignmentSatisfiesOneClause_ReturnsRemainingClauses()
    {
        // Arrange
        var formula = new Formula(2,
            new Clause(new Literal(1)),
            new Clause(new Literal(2))
        );
        
        // Act
        var result = Solver.Simplify(formula, 1, true);
        
        // Assert
        Assert.Equal("(2)", result.ToString());
    }

    [Fact]
    public void Simplify_MultipleClauses_AssignmentDoesNotAffectClause_ReturnsSameClause()
    {
        // Arrange
        var formula = new Formula(2,
            new Clause(new Literal(1)),
            new Clause(new Literal(2))
        );
        
        // Act
        var result = Solver.Simplify(formula, 2, true);
        
        // Assert
        Assert.Equal("(1)", result.ToString());
    }

    [Fact]
    public void Simplify_NegatedLiteral_AssignmentSatisfiesClause_ReturnsEmptyFormula()
    {
        // Arrange
        var formula = new Formula(1, new Clause(new Literal(1, true)));
        
        // Act
        var result = Solver.Simplify(formula, 1, false);
        
        // Assert
        Assert.Equal("", result.ToString());
    }

    [Fact]
    public void Simplify_ComplexFormula_MultipleClausesWithNegatedLiterals_ReturnsCorrectSimplifiedFormula()
    {
        // Arrange
        var formula = new Formula(3,
            new Clause(new Literal(1), new Literal(2, true)),
            new Clause(new Literal(2), new Literal(3)),
            new Clause(new Literal(1, true), new Literal(3, true))
        );
        
        // Act
        var result = Solver.Simplify(formula, 1, true);
        
        // Assert
        Assert.Equal("(2 ∨ 3) ∧ (¬3)", result.ToString());
    }
} 