namespace Seamless.Solver;

public readonly struct Literal
{
    public int Variable { get; }
    public bool IsNegated { get; }

    public Literal(int variable, bool isNegated = false)
    {
        Variable = variable;
        IsNegated = isNegated;
    }

    public Literal Negate() => new(Variable, !IsNegated);

    public override string ToString() => IsNegated ? $"¬{Variable}" : $"{Variable}";
} 