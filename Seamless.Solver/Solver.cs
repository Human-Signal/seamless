namespace Seamless.Solver;

public static class Solver
{
    public static bool? Solve(Formula formula)
    {
        var stack = new Stack<(Formula formula, Dictionary<int, bool?> assignment)>();
        stack.Push((formula, new Dictionary<int, bool?>()));

        while (stack.Count > 0)
        {
            var (currentFormula, assignment) = stack.Pop();

            // Unit propagation
            while (true)
            {
                var unitClause = currentFormula.Clauses.FirstOrDefault(c => c.IsUnit);
                if (unitClause == null) break;

                var unitLiteral = unitClause.Literals.First();
                assignment[unitLiteral.Variable] = !unitLiteral.IsNegated;
                
                currentFormula = Simplify(currentFormula, unitLiteral.Variable, !unitLiteral.IsNegated);
                if (currentFormula.Clauses.Any(c => c.IsEmpty))
                {
                    Console.WriteLine($"Found empty clause during unit propagation. Formula: {currentFormula}");
                    return false;
                }
            }

            // Pure literal elimination
            var pureLiterals = FindPureLiterals(currentFormula);
            foreach (var literal in pureLiterals)
            {
                assignment[literal.Variable] = !literal.IsNegated;
                currentFormula = Simplify(currentFormula, literal.Variable, !literal.IsNegated);
            }

            if (currentFormula.Clauses.Count == 0)
            {
                // Found a satisfying assignment
                return true;
            }

            if (currentFormula.Clauses.Any(c => c.IsEmpty))
            {
                Console.WriteLine($"Found empty clause after pure literal elimination. Formula: {currentFormula}");
                return false;
            }

            // Choose a variable to branch on
            var variable = ChooseVariable(currentFormula);
            if (variable == null)
            {
                // No more variables to branch on
                return true;
            }

            // Try assigning false first (push to stack)
            var falseAssignment = new Dictionary<int, bool?>(assignment);
            falseAssignment[variable.Value] = false;
            stack.Push((Simplify(currentFormula, variable.Value, false), falseAssignment));

            // Try assigning true (push to stack)
            var trueAssignment = new Dictionary<int, bool?>(assignment);
            trueAssignment[variable.Value] = true;
            stack.Push((Simplify(currentFormula, variable.Value, true), trueAssignment));
        }

        return false;
    }

    private static Formula Simplify(Formula formula, int variable, bool value)
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

    public static IEnumerable<Literal> FindPureLiterals(Formula formula)
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

    private static int? ChooseVariable(Formula formula)
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
} 