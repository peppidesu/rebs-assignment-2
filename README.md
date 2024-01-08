# REBS Assignment 2

**Group 9** - Max Kowalchuk (*cnl490*), Pepijn Bakker (*zmx912*), Hjalte Tromborg (*vfr574*) 

## How to run
### Build from source
Requires **.NET SDK 8.0** or later.

1. Download the source code (using `git clone` or from one of the [releases](https://github.com/peppidesu/rebs-assignment-2/releases).
2. Build the project with `dotnet build`
3. The executable can be found in the created subfolder in `./bin/`.

### Prebuild binary
Alternatively, download the prebuild binary for your system from the [latest release page](https://github.com/peppidesu/rebs-assignment-2/releases/latest).

## Usage
```
DcrConformanceChecker <log-file> <graph-file> [-v] [-r <run-name>]
```
See `DcrConformanceChecker --help` for more details.

### Graph and log files
- The graphs for the patterns of assignment 1 as well as the overarching DCR graph are located in the `./graphs` folder.
- The Dreyers log file is located in the `./logs` folder.

When building, these folders are automatically copied to the output folder.
