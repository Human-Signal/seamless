# Seamless SAT Solver

Seamless is a modern SAT solver implementation in C# with a command-line interface. It implements the DPLL (Davis-Putnam-Logemann-Loveland) algorithm for solving Boolean satisfiability problems and supports the standard DIMACS CNF format.

## Installation

Clone the repository and build using .NET:

```bash
git clone https://github.com/yourusername/Seamless.git
cd Seamless
dotnet build
```

## Usage

The solver provides several commands through its CLI:

### Solve a SAT Problem

```bash
seamless solve <file>
```

Solves a SAT problem from a DIMACS CNF file. Supports both .cnf and .cnf.xz files.

Example:
```bash
seamless solve problem.cnf.xz
```

### Get Problem Information

```bash
seamless info <file>
```

Displays information about a DIMACS CNF file, including:
- Number of variables
- Number of clauses
- Average clause size
- Minimum and maximum clause sizes

Example:
```bash
seamless info problem.cnf
```

### Run Built-in Example

```bash
seamless example
```

Runs a built-in example formula to demonstrate the solver's functionality.

### Help

Get help about any command:

```bash
seamless --help          # General help
seamless solve --help    # Help for the solve command
seamless info --help     # Help for the info command
```

## Input Format

The solver accepts DIMACS CNF format files. Here's an example of the format: 