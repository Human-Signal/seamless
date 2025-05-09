namespace Seamless.Solver;

public static class Solver
{
    private record VariableAssignment(int Variable, bool Value, bool Flipped, bool IsDecisionVariable);

    public static (bool? Result, Dictionary<int, bool?>? Assignment) Solve(Formula formula, CancellationToken cancellationToken = default)
    {
        var watchedFormula = new WatchedFormula(formula.VariableCount, formula.Clauses);
        var assignments = new Dictionary<int, bool?>(formula.VariableCount);
        var stack = new Stack<VariableAssignment>(formula.VariableCount);
        long iterations = 0;

        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Cancelled after {iterations} iterations");
                return (null, null);
            }

            iterations++;
            var result = Propagate(watchedFormula, assignments, stack);
            if (result == PropagationResult.Satisfiable)
            {
                Console.WriteLine($"Determined satisfiable after {iterations} iterations");
                return (true, assignments);
            }
            else if (result == PropagationResult.Unsatisfiable)
            {
                var assignment = Backtrack(assignments, stack);
                if (assignment == null)
                {
                    break;
                }
                stack.Push(assignment);
            }
            else
            {
                var assignment = ChooseVariable(watchedFormula, assignments);
                stack.Push(assignment);
            }
        }
        Console.WriteLine($"Determined unsatisfiable after {iterations} iterations");
        return (false, null);
    }

    private enum PropagationResult
    {
        Unsatisfiable,
        Satisfiable,
        Unknown,
    }

    private static PropagationResult Propagate(
        WatchedFormula formula, 
        Dictionary<int, bool?> assignment,
        Stack<VariableAssignment> stack)
    {
        throw new NotImplementedException();
    }

    private static VariableAssignment? Backtrack(
        Dictionary<int, bool?> assignments,
        Stack<VariableAssignment> stack)
    {
        while (stack.Count > 0)
        {
            var assignment = stack.Pop();
            assignments[assignment.Variable] = null;
            if (assignment.IsDecisionVariable)
            {
                if (!assignment.Flipped)
                {
                    return new VariableAssignment(assignment.Variable, !assignment.Value, true, true);
                }
            }
        }
        return null;
    }

    private static VariableAssignment ChooseVariable(WatchedFormula formula, Dictionary<int, bool?> assignments)
    {
        // Simple heuristic: choose the first unassigned variable
        for (int i = 1; i <= formula.VariableCount; i++)
        {
            if (!assignments.TryGetValue(i, out var value) || value != null)
            {
                return new VariableAssignment(i, true, false, true);
            }
        }
        throw new InvalidOperationException("No variables left to choose");
    }
} 