# Seamless SAT Solver CLI

A command-line interface for the Seamless SAT Solver, a modern SAT solver implementation.

## Commands

### solve
Solves a SAT problem from a DIMACS CNF file.

```bash
seamless solve <file>
```

**Arguments:**
- `file`: The DIMACS CNF file to solve (supports .cnf and .cnf.xz files)

**Output:**
- If satisfiable: Prints "SATISFIABLE" in green and shows the variable assignments
- If unsatisfiable: Prints "UNSATISFIABLE" in red
- If unknown: Prints "UNKNOWN" in yellow

### info
Displays information about a DIMACS CNF file.

```bash
seamless info <file>
```

**Arguments:**
- `file`: The DIMACS CNF file to analyze

**Output:**
- File name
- Number of variables
- Number of clauses
- Average clause size
- Minimum clause size
- Maximum clause size

### example
Runs the built-in example formula.

```bash
seamless example
```

**Output:**
- The example formula: (x1 ∨ ¬x2) ∧ (x2 ∨ x3) ∧ (¬x1 ∨ ¬x3)
- The solution status (SATISFIABLE/UNSATISFIABLE/UNKNOWN)
- If satisfiable, shows the variable assignments

## Example Usage

```bash
# Solve a SAT problem
seamless solve problem.cnf

# Get information about a CNF file
seamless info problem.cnf

# Run the example formula
seamless example
``` 