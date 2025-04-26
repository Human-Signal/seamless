using System.CommandLine;
using Seamless.Solver;

namespace Seamless.Cli.Commands;

public class SolveCommandHandler : ICommandHandler
{
    private readonly FileInfo _file;

    public SolveCommandHandler(FileInfo file)
    {
        _file = file;
    }

    public void Handle()
    {
        if (!_file.Exists)
        {
            throw new FileNotFoundException($"File not found: {_file.FullName}");
        }

        var formula = DimacsReader.ReadFromFile(_file.FullName);
        Console.WriteLine($"Solving formula with {formula.VariableCount} variables and {formula.Clauses.Count} clauses...\n");

        var result = Solver.Solver.Solve(formula);

        if (result.Result == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SATISFIABLE");
            Console.ResetColor();
            var assignment = result.Assignment;
            if (assignment == null)
            {
                Console.WriteLine("No assignment found");
            }
            else
            {
                foreach (var kvp in assignment.OrderBy(k => k.Key))
                {
                    Console.WriteLine($"x{kvp.Key} = {kvp.Value}");
                }
            }
        }
        else if (result.Result == false)
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