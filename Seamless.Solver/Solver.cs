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
        return DPLL(_formula, _assignment);
    }

    private bool? DPLL(Formula formula, Dictionary<int, bool?> assignment)
    {
        // Unit propagation
        while (true)
        {
            var unitClause = formula.Clauses.FirstOrDefault(c => c.IsUnit);
            if (unitClause == null) break;

            var unitLiteral = unitClause.Literals.First();
            assignment[unitLiteral.Variable] = !unitLiteral.IsNegated;
            
            formula = Simplify(formula, unitLiteral.Variable, !unitLiteral.IsNegated);
            if (formula.Clauses.Any(c => c.IsEmpty))
                return false;
        }

        // Pure literal elimination
        var pureLiterals = FindPureLiterals(formula);
        foreach (var literal in pureLiterals)
        {
            assignment[literal.Variable] = !literal.IsNegated;
            formula = Simplify(formula, literal.Variable, !literal.IsNegated);
        }

        if (formula.Clauses.Count == 0)
            return true;

        if (formula.Clauses.Any(c => c.IsEmpty))
            return false;

        // Choose a variable to branch on
        var variable = ChooseVariable(formula);
        if (variable == null)
            return true;

        // Try assigning true
        var trueAssignment = new Dictionary<int, bool?>(assignment);
        trueAssignment[variable.Value] = true;
        var trueResult = DPLL(Simplify(formula, variable.Value, true), trueAssignment);
        if (trueResult == true)
        {
            foreach (var kvp in trueAssignment)
                assignment[kvp.Key] = kvp.Value;
            return true;
        }

        // Try assigning false
        var falseAssignment = new Dictionary<int, bool?>(assignment);
        falseAssignment[variable.Value] = false;
        var falseResult = DPLL(Simplify(formula, variable.Value, false), falseAssignment);
        if (falseResult == true)
        {
            foreach (var kvp in falseAssignment)
                assignment[kvp.Key] = kvp.Value;
            return true;
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

            if (!clauseSatisfied && newLiterals.Count > 0)
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