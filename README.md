# Image Processor

## Requirements

dotnet version 7.0:

```sh
dotnet --version
```

## Installation

1. Clone the repo

```sh
git clone https://github.com/TheIthorian/c-sharp-image-processor.git .
```

2. Build

```sh
dotnet build
```

## Running

```sh
dotnet run <input_file_path> <output_file_path>
```

The `input_file_path` and `output_file_path` arguments are optional.

-   `input_file_path` defaults to the first file found in the directory.
-   `output_file_path` defaults to `'./output.jpg'`
