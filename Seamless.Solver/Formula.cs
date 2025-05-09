namespace Seamless.Solver;

public class Formula
{
    public int VariableCount { get; }
    public HashSet<Clause>[] WatchLists { get; }
    public HashSet<Clause> UnitClauses { get; }

    public Formula(int variableCount, IEnumerable<Clause> clauses)
    {
        VariableCount = variableCount;
        UnitClauses = new HashSet<Clause>();
        
        // Initialize watch lists - 2 lists per variable (positive and negative)
        WatchLists = new HashSet<Clause>[variableCount * 2];
        for (int i = 0; i < variableCount * 2; i++)
        {
            WatchLists[i] = new HashSet<Clause>();
        }

        // Populate watch lists and unit clauses
        foreach (var clause in clauses)
        {
            if (clause.Literals.Count == 1)
            {
                UnitClauses.Add(clause);
            }
            else if (clause.Literals.Count >= 2)
            {
                // Add to watch lists for first two literals
                var literals = clause.Literals.ToArray();
                var lit1 = literals[0];
                var lit2 = literals[1];
                
                AddToWatchList(lit1, clause);
                AddToWatchList(lit2, clause);
            }
        }
    }

    private void AddToWatchList(Literal literal, Clause clause)
    {
        int index = literal.IsNegated 
            ? (literal.Variable - 1) * 2 + 1 
            : (literal.Variable - 1) * 2;
        WatchLists[index].Add(clause);
    }

    public Formula(int variableCount, params Clause[] clauses) 
        : this(variableCount, (IEnumerable<Clause>)clauses)
    {
    }

    public override string ToString() 
    {
        var allClauses = UnitClauses.Concat(WatchLists.SelectMany(list => list));
        return string.Join(" ∧ ", allClauses);
    }
} 