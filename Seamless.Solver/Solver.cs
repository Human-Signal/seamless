namespace Seamless.Solver;

public class Solver
{
    private readonly Formula _formula;
    private readonly Dictionary<int, bool?> _assignment;

    public Solver(Formula formula)
    {
        _formula = formula;
        _assignment = new Dictionary<int, bool?>();
    }

    public bool? Solve()
    {
        var stack = new Stack<(Formula formula, Dictionary<int, bool?> assignment)>();
        stack.Push((_formula, new Dictionary<int, bool?>(_assignment)));

        while (stack.Count > 0)
        {
            var (formula, assignment) = stack.Pop();

            // Unit propagation
            while (true)
            {
                var unitClause = formula.Clauses.FirstOrDefault(c => c.IsUnit);
                if (unitClause == null) break;

                var unitLiteral = unitClause.Literals.First();
                assignment[unitLiteral.Variable] = !unitLiteral.IsNegated;
                
                formula = Simplify(formula, unitLiteral.Variable, !unitLiteral.IsNegated);
                if (formula.Clauses.Any(c => c.IsEmpty))
                {
                    Console.WriteLine($"Found empty clause during unit propagation. Formula: {formula}");
                    return false;
                }
            }

            // Pure literal elimination
            var pureLiterals = FindPureLiterals(formula);
            foreach (var literal in pureLiterals)
            {
                assignment[literal.Variable] = !literal.IsNegated;
                formula = Simplify(formula, literal.Variable, !literal.IsNegated);
            }

            if (formula.Clauses.Count == 0)
            {
                // Found a satisfying assignment
                foreach (var kvp in assignment)
                    _assignment[kvp.Key] = kvp.Value;
                return true;
            }

            if (formula.Clauses.Any(c => c.IsEmpty))
            {
                Console.WriteLine($"Found empty clause after pure literal elimination. Formula: {formula}");
                return false;
            }

            // Choose a variable to branch on
            var variable = ChooseVariable(formula);
            if (variable == null)
            {
                // No more variables to branch on
                foreach (var kvp in assignment)
                    _assignment[kvp.Key] = kvp.Value;
                return true;
            }

            // Try assigning false first (push to stack)
            var falseAssignment = new Dictionary<int, bool?>(assignment);
            falseAssignment[variable.Value] = false;
            stack.Push((Simplify(formula, variable.Value, false), falseAssignment));

            // Try assigning true (push to stack)
            var trueAssignment = new Dictionary<int, bool?>(assignment);
            trueAssignment[variable.Value] = true;
            stack.Push((Simplify(formula, variable.Value, true), trueAssignment));
        }

        return false;
    }

    private Formula Simplify(Formula formula, int variable, bool value)
    {
        var newClauses = new HashSet<Clause>();
        foreach (var clause in formula.Clauses)
        {
            var newLiterals = new HashSet<Literal>();
            bool clauseSatisfied = false;

            foreach (var literal in clause.Literals)
            {
                if (literal.Variable == variable)
                {
                    if ((literal.IsNegated && !value) || (!literal.IsNegated && value))
                    {
                        clauseSatisfied = true;
                        break;
                    }
                }
                else
                {
                    newLiterals.Add(literal);
                }
            }

            if (!clauseSatisfied)
            {
                newClauses.Add(new Clause(newLiterals));
            }
        }

        return new Formula(formula.VariableCount, newClauses);
    }

    private IEnumerable<Literal> FindPureLiterals(Formula formula)
    {
        var literalCounts = new Dictionary<Literal, int>();
        foreach (var clause in formula.Clauses)
        {
            foreach (var literal in clause.Literals)
            {
                if (!literalCounts.ContainsKey(literal))
                    literalCounts[literal] = 0;
                literalCounts[literal]++;
            }
        }

        return literalCounts
            .Where(kvp => !literalCounts.ContainsKey(kvp.Key.Negate()))
            .Select(kvp => kvp.Key);
    }

    private int? ChooseVariable(Formula formula)
    {
        // Simple heuristic: choose the variable that appears most frequently
        var variableCounts = new Dictionary<int, int>();
        foreach (var clause in formula.Clauses)
        {
            foreach (var literal in clause.Literals)
            {
                if (!variableCounts.ContainsKey(literal.Variable))
                    variableCounts[literal.Variable] = 0;
                variableCounts[literal.Variable]++;
            }
        }

        return variableCounts.Count > 0
            ? variableCounts.OrderByDescending(kvp => kvp.Value).First().Key
            : null;
    }

    public Dictionary<int, bool> GetAssignment()
    {
        return _assignment
            .Where(kvp => kvp.Value.HasValue)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Value);
    }
} 