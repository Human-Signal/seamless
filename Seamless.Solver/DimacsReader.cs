using SharpCompress.Compressors.Xz;

namespace Seamless.Solver;

public class DimacsReader
{
    public static Formula ReadFromFile(string filePath)
    {
        // Handle .xz compressed files
        if (filePath.EndsWith(".xz"))
        {
            using var xzStream = new FileStream(filePath, FileMode.Open);
            using var decompressor = new XZStream(xzStream);
            using var reader = new StreamReader(decompressor);
            return ParseDimacs(reader);
        }
        else
        {
            using var reader = new StreamReader(filePath);
            return ParseDimacs(reader);
        }
    }

    private static Formula ParseDimacs(StreamReader reader)
    {
        int variableCount = 0;
        int clauseCount = 0;
        var clauses = new List<Clause>();

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            // Skip empty lines and comments
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("c"))
                continue;

            // Parse problem line
            if (line.StartsWith("p cnf"))
            {
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 4)
                    throw new FormatException("Invalid problem line format");

                variableCount = int.Parse(parts[2]);
                clauseCount = int.Parse(parts[3]);
                continue;
            }

            // Parse clause line
            var literals = new List<Literal>();
            var numbers = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var numStr in numbers)
            {
                var num = int.Parse(numStr);
                if (num == 0) break; // End of clause marker
                
                var variable = Math.Abs(num);
                var isNegated = num < 0;
                literals.Add(new Literal(variable, isNegated));
            }

            if (literals.Count > 0)
                clauses.Add(new Clause(literals));
        }

        if (variableCount == 0)
            throw new FormatException("No problem line found in DIMACS file");

        if (clauses.Count != clauseCount)
            throw new FormatException($"Expected {clauseCount} clauses but found {clauses.Count}");

        return new Formula(variableCount, clauses);
    }
} 