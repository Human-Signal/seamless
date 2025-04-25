namespace Seamless.Solver;

public class Clause
{
    public HashSet<Literal> Literals { get; }

    public Clause(IEnumerable<Literal> literals)
    {
        Literals = new HashSet<Literal>(literals);
    }

    public Clause(params Literal[] literals) : this((IEnumerable<Literal>)literals)
    {
    }

    public bool IsEmpty => Literals.Count == 0;
    public bool IsUnit => Literals.Count == 1;

    public override string ToString() => $"({string.Join(" ∨ ", Literals)})";
} 