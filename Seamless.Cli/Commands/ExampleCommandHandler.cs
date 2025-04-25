using System.CommandLine;
using Seamless.Solver;

namespace Seamless.Cli.Commands;

public class ExampleCommandHandler : ICommandHandler
{
    public void Handle()
    {
        // Example: (x1 ∨ ¬x2) ∧ (x2 ∨ x3) ∧ (¬x1 ∨ ¬x3)
        var formula = new Formula(3,
            new Clause(
                new Literal(1),
                new Literal(2, true)
            ),
            new Clause(
                new Literal(2),
                new Literal(3)
            ),
            new Clause(
                new Literal(1, true),
                new Literal(3, true)
            )
        );

        Console.WriteLine("Example formula:");
        Console.WriteLine(formula);
        Console.WriteLine();

        var solver = new Solver.Solver(formula);
        var result = solver.Solve();

        if (result == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SATISFIABLE");
            Console.ResetColor();
            var assignment = solver.GetAssignment();
            foreach (var kvp in assignment.OrderBy(k => k.Key))
            {
                Console.WriteLine($"x{kvp.Key} = {kvp.Value}");
            }
        }
        else if (result == false)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("UNSATISFIABLE");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("UNKNOWN");
            Console.ResetColor();
        }
    }
} 