namespace Seamless.Solver;

public class Formula
{
    public HashSet<Clause> Clauses { get; }
    public int VariableCount { get; }

    public Formula(int variableCount, IEnumerable<Clause> clauses)
    {
        VariableCount = variableCount;
        Clauses = new HashSet<Clause>(clauses);
    }

    public Formula(int variableCount, params Clause[] clauses) 
        : this(variableCount, (IEnumerable<Clause>)clauses)
    {
    }

    public override string ToString() => string.Join(" ∧ ", Clauses);
} 