using System.CommandLine;
using Seamless.Solver;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Seamless SAT Solver - A modern SAT solver implementation");

        // solve command
        var solveCommand = new Command("solve", "Solve a SAT problem from a DIMACS CNF file");
        var solveFileArg = new Argument<FileInfo>(
            name: "file",
            description: "The DIMACS CNF file to solve (supports .cnf and .cnf.xz files)"
        );
        solveCommand.AddArgument(solveFileArg);
        solveCommand.SetHandler(HandleSolve, solveFileArg);

        // info command
        var infoCommand = new Command("info", "Display information about a DIMACS CNF file");
        var infoFileArg = new Argument<FileInfo>(
            name: "file",
            description: "The DIMACS CNF file to analyze"
        );
        infoCommand.AddArgument(infoFileArg);
        infoCommand.SetHandler(HandleInfo, infoFileArg);

        // list command
        var listCommand = new Command("list", "List information about all DIMACS files in a folder");
        var listFolderArg = new Argument<DirectoryInfo>(
            name: "folder",
            description: "The folder containing DIMACS files to analyze"
        );
        var sortOption = new Option<string>(
            name: "--sort",
            description: "Sort order: name, size, variables, or clauses",
            getDefaultValue: () => "name"
        );
        listCommand.AddArgument(listFolderArg);
        listCommand.AddOption(sortOption);
        listCommand.SetHandler(HandleList, listFolderArg, sortOption);

        // example command
        var exampleCommand = new Command("example", "Run the built-in example formula");
        exampleCommand.SetHandler(HandleExample);

        rootCommand.AddCommand(solveCommand);
        rootCommand.AddCommand(infoCommand);
        rootCommand.AddCommand(listCommand);
        rootCommand.AddCommand(exampleCommand);

        return await rootCommand.InvokeAsync(args);
    }

    static void HandleSolve(FileInfo file)
    {
        if (!file.Exists)
        {
            throw new FileNotFoundException($"File not found: {file.FullName}");
        }

        var formula = DimacsReader.ReadFromFile(file.FullName);
        Console.WriteLine($"Solving formula with {formula.VariableCount} variables and {formula.Clauses.Count} clauses...\n");

        var solver = new Solver(formula);
        var result = solver.Solve();

        if (result == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SATISFIABLE");
            Console.ResetColor();
            var assignment = solver.GetAssignment();
            foreach (var (variable, value) in assignment.OrderBy(kvp => kvp.Key))
            {
                Console.WriteLine($"x{variable} = {value}");
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

    static void HandleInfo(FileInfo file)
    {
        if (!file.Exists)
        {
            throw new FileNotFoundException($"File not found: {file.FullName}");
        }

        var formula = DimacsReader.ReadFromFile(file.FullName);
        Console.WriteLine($"DIMACS CNF File: {file.Name}");
        Console.WriteLine($"Variables: {formula.VariableCount}");
        Console.WriteLine($"Clauses: {formula.Clauses.Count}");
        Console.WriteLine($"Average clause size: {formula.Clauses.Average(c => c.Literals.Count):F2}");
        Console.WriteLine($"Minimum clause size: {formula.Clauses.Min(c => c.Literals.Count)}");
        Console.WriteLine($"Maximum clause size: {formula.Clauses.Max(c => c.Literals.Count)}");
    }

    static void HandleList(DirectoryInfo folder, string sortBy)
    {
        if (!folder.Exists)
        {
            throw new DirectoryNotFoundException($"Folder not found: {folder.FullName}");
        }

        var files = folder.GetFiles("*.cnf*");
        var fileInfos = new List<(FileInfo File, int VariableCount, int ClauseCount)>();

        foreach (var file in files)
        {
            try
            {
                var (variableCount, clauseCount) = DimacsReader.ReadHeader(file.FullName);
                fileInfos.Add((file, variableCount, clauseCount));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading {file.Name}: {ex.Message}");
            }
        }

        var sortedFiles = sortBy.ToLower() switch
        {
            "name" => fileInfos.OrderBy(f => f.File.Name),
            "size" => fileInfos.OrderBy(f => f.File.Length),
            "variables" => fileInfos.OrderBy(f => f.VariableCount),
            "clauses" => fileInfos.OrderBy(f => f.ClauseCount),
            _ => throw new ArgumentException($"Invalid sort option: {sortBy}")
        };

        // Calculate column widths based on maximum values
        var maxVariables = fileInfos.Max(f => f.VariableCount);
        var maxClauses = fileInfos.Max(f => f.ClauseCount);
        var varWidth = Math.Max(10, maxVariables.ToString("N0").Length);
        var clauseWidth = Math.Max(8, maxClauses.ToString("N0").Length);

        Console.WriteLine($"Found {fileInfos.Count} DIMACS files in {folder.FullName}\n");
        
        // Print header
        Console.WriteLine("Variables".PadLeft(varWidth) + "  " +
                         "Clauses".PadLeft(clauseWidth) + "  " +
                         "Size (KB)".PadLeft(13) + "  " +
                         "Filename");
        Console.WriteLine(new string('-', varWidth + clauseWidth + 13 + 40 + 6));

        foreach (var (file, variableCount, clauseCount) in sortedFiles)
        {
            var sizeKB = (int)Math.Ceiling(file.Length / 1024.0);
            Console.WriteLine(variableCount.ToString("N0").PadLeft(varWidth) + "  " +
                            clauseCount.ToString("N0").PadLeft(clauseWidth) + "  " +
                            sizeKB.ToString("N0").PadLeft(10) + " KB  " +
                            file.Name);
        }
    }

    static void HandleExample()
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

        var solver = new Solver(formula);
        var result = solver.Solve();

        if (result == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SATISFIABLE");
            Console.ResetColor();
            var assignment = solver.GetAssignment();
            foreach (var (variable, value) in assignment.OrderBy(kvp => kvp.Key))
            {
                Console.WriteLine($"x{variable} = {value}");
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
