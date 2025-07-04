# Seamless SAT Solver

Seamless is a modern SAT solver implementation in C# with a command-line interface. It implements the DPLL (Davis-Putnam-Logemann-Loveland) algorithm for solving Boolean satisfiability problems and supports the standard DIMACS CNF format.

## Installation

Clone the repository and build using .NET:

```bash
git clone https://github.com/yourusername/Seamless.git
cd Seamless
dotnet build
```

Make the seamless script executable:
```bash
chmod +x seamless
```

## Usage

The solver provides several commands through its CLI:

### Solve a SAT Problem

```bash
./seamless solve <file>
```

Solves a SAT problem from a DIMACS CNF file. Supports both .cnf and .cnf.xz files.

Example:
```bash
./seamless solve problem.cnf.xz
```

### Get Problem Information

```bash
./seamless info <file>
```

Displays information about a DIMACS CNF file, including:
- Number of variables
- Number of clauses
- Average clause size
- Minimum and maximum clause sizes

Example:
```bash
./seamless info problem.cnf
```

### List DIMACS Files

```bash
./seamless list <folder> [--sort <criteria>]
```

Lists information about all DIMACS files in a folder. The output includes:
- Number of variables
- Number of clauses
- File size in KB
- Filename

The `--sort` option allows sorting by:
- `name` (default): Sort by filename
- `size`: Sort by file size
- `variables`: Sort by number of variables
- `clauses`: Sort by number of clauses

Example:
```bash
# List all files sorted by name
./seamless list /path/to/folder

# List files sorted by number of variables
./seamless list /path/to/folder --sort variables
```

### Run Built-in Example

```bash
./seamless example
```

Runs a built-in example formula to demonstrate the solver's functionality.

### Help

Get help about any command:

```bash
./seamless --help          # General help
./seamless solve --help    # Help for the solve command
./seamless info --help     # Help for the info command
./seamless list --help     # Help for the list command
```


## Sample Files

Easy ones

- SAT: `./seamless solve ./samples/36c342091848d5d6a1a8eeb3a8b49b86-rovers1_ks99i.renamed-as.sat05-3971.cnf.xz`
- UNSAT: `./seamless solve ./samples/211938776d92f11870a687abd11d55a4-iso-icl004.shuffled-as.sat05-3238.cnf.xz`
