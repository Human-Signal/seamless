# SAT Benchmark Problems

This directory contains a collection of SAT benchmark problems in DIMACS CNF format, compressed with XZ.

## Downloading the Benchmarks

You can download problems from either the 2023 or 2024 competition tracks:

```bash
# Download 2023 track problems
wget --content-disposition -i track_main_2023.uri

# Download 2024 track problems
wget --content-disposition -i track_main_2024.uri
```

## Download Sizes

| Track File | Number of Problems | Total Size |
|------------|-------------------|------------|
| track_main_2023.uri | 400 files | 3.37 GB |
| track_main_2024.uri | 400 files | 3.63 GB |

Make sure you have sufficient disk space for both the compressed files and working space for when they are decompressed during solving.

## Usage Example

To solve a problem:

```bash
seamless solve samples/03e9d1abe418a1727bbf2ead77d69d02-php15-mixed-15percent-blocked.cnf.xz
```

To get information about a problem:

```bash
seamless info samples/03e9d1abe418a1727bbf2ead77d69d02-php15-mixed-15percent-blocked.cnf.xz
```

## Note on File Sizes

- Smallest problems: ~3-4KB (chess puzzles)
- Largest problems: Several hundred MB
- Wide range of sizes to test solver performance
