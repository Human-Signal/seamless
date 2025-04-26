using System.CommandLine;
using Seamless.Cli.Commands;

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
        var timeLimitOption = new Option<int>(
            name: "--time-limit",
            description: "Maximum time in seconds to run the solver (default: 5000)",
            getDefaultValue: () => 5000
        );
        solveCommand.AddArgument(solveFileArg);
        solveCommand.AddOption(timeLimitOption);
        solveCommand.SetHandler((file, timeLimit) => new SolveCommandHandler(file, timeLimit).Handle(), solveFileArg, timeLimitOption);

        // info command
        var infoCommand = new Command("info", "Display information about a DIMACS CNF file");
        var infoFileArg = new Argument<FileInfo>(
            name: "file",
            description: "The DIMACS CNF file to analyze"
        );
        infoCommand.AddArgument(infoFileArg);
        infoCommand.SetHandler(file => new InfoCommandHandler(file).Handle(), infoFileArg);

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
        listCommand.SetHandler((folder, sortBy) => new ListCommandHandler(folder, sortBy).Handle(), listFolderArg, sortOption);

        // example command
        var exampleCommand = new Command("example", "Run the built-in example formula");
        exampleCommand.SetHandler(() => new ExampleCommandHandler().Handle());

        rootCommand.AddCommand(solveCommand);
        rootCommand.AddCommand(infoCommand);
        rootCommand.AddCommand(listCommand);
        rootCommand.AddCommand(exampleCommand);

        return await rootCommand.InvokeAsync(args);
    }
}
