using System.CommandLine;
using Seamless.Solver;

namespace Seamless.Cli.Commands;

public class InfoCommandHandler : ICommandHandler
{
    private readonly FileInfo _file;

    public InfoCommandHandler(FileInfo file)
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
        Console.WriteLine($"DIMACS CNF File: {_file.Name}");
        Console.WriteLine($"Variables: {formula.VariableCount}");
        Console.WriteLine($"Clauses: {formula.Clauses.Count}");
        Console.WriteLine($"Average clause size: {formula.Clauses.Average(c => c.Literals.Count):F2}");
        Console.WriteLine($"Minimum clause size: {formula.Clauses.Min(c => c.Literals.Count)}");
        Console.WriteLine($"Maximum clause size: {formula.Clauses.Max(c => c.Literals.Count)}");
    }
} 