using Seamless.Solver;

namespace Seamless.Solver.Tests;

public class FormulaTests
{
    [Fact]
    public void Formula_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var clauses = new[]
        {
            new Clause(new Literal(1)),
            new Clause(new Literal(2, true))
        };

        // Act
        var formula = new Formula(2, clauses);

        // Assert
        Assert.Equal(2, formula.VariableCount);
        Assert.Equal(2, formula.Clauses.Count);
        Assert.Contains(clauses[0], formula.Clauses);
        Assert.Contains(clauses[1], formula.Clauses);
    }

    [Fact]
    public void Formula_ParamsConstructor_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var formula = new Formula(2,
            new Clause(new Literal(1)),
            new Clause(new Literal(2, true))
        );

        // Assert
        Assert.Equal(2, formula.VariableCount);
        Assert.Equal(2, formula.Clauses.Count);
    }

    [Fact]
    public void Formula_ToString_ReturnsCorrectFormat()
    {
        // Arrange
        var formula = new Formula(2,
            new Clause(new Literal(1)),
            new Clause(new Literal(2, true))
        );

        // Act & Assert
        Assert.Equal("1 ∧ ¬2", formula.ToString());
    }
} 