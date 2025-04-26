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

    public override bool Equals(object? obj)
    {
        if (obj is Literal other)
        {
            return Variable == other.Variable && IsNegated == other.IsNegated;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Variable, IsNegated);
    }
} 