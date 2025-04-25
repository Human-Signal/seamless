using System.CommandLine;
using Seamless.Solver;

namespace Seamless.Cli.Commands;

public class ListCommandHandler : ICommandHandler
{
    private readonly DirectoryInfo _folder;
    private readonly string _sortBy;

    public ListCommandHandler(DirectoryInfo folder, string sortBy)
    {
        _folder = folder;
        _sortBy = sortBy;
    }

    public void Handle()
    {
        if (!_folder.Exists)
        {
            throw new DirectoryNotFoundException($"Folder not found: {_folder.FullName}");
        }

        var files = _folder.GetFiles("*.cnf*");
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

        var sortedFiles = _sortBy.ToLower() switch
        {
            "name" => fileInfos.OrderBy(f => f.File.Name),
            "size" => fileInfos.OrderBy(f => f.File.Length),
            "variables" => fileInfos.OrderBy(f => f.VariableCount),
            "clauses" => fileInfos.OrderBy(f => f.ClauseCount),
            _ => throw new ArgumentException($"Invalid sort option: {_sortBy}")
        };

        // Calculate column widths based on maximum values
        var maxVariables = fileInfos.Max(f => f.VariableCount);
        var maxClauses = fileInfos.Max(f => f.ClauseCount);
        var varWidth = Math.Max(10, maxVariables.ToString("N0").Length);
        var clauseWidth = Math.Max(8, maxClauses.ToString("N0").Length);

        Console.WriteLine($"Found {fileInfos.Count} DIMACS files in {_folder.FullName}\n");
        
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
} 