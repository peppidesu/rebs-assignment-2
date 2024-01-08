# REBS Assignment 2

**Group 9** - Max Kowalchuk (*cnl490*), Pepijn Bakker (*zmx912*), Hjalte Tromborg (*vfr574*) 

## How to run
Run the project with `dotnet run` from the command line, or build with `dotnet build` and run the executable in `./bin`.
Alternatively, download the prebuild binary for your system from the [latest release page](https://github.com/peppidesu/rebs-assignment-2/releases/tag/latest).

## Usage
```
DcrConformanceChecker <log-file> <graph-file> [-v] [-r <run-name>]
```
See `DcrConformanceChecker --help` for more details.

### Graph and log files
- The graphs for the patterns of assignment 1 as well as the overarching DCR graph are located in the `./graphs` folder.
- The Dreyers log file is located in the `./logs` folder.

When building, these folders are automatically copied to the output folder.
