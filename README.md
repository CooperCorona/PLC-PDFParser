# PLC Assignment 2 Parser
Parses a PDF generated by the [CSE 322 WebGrader](https://cse.unl.edu/~cse322/grade).
Outputs whether each test was passed or failed and the percentage of tests passed.
Outputs of failing tests are printed to plain text files containing the expected output,
the actual output, the original output, and the differences between them.

## Cloning
* Open your Git client of choice.
* Navigate to your preferred directory.
* Clone ```https://github.com/CooperCorona/PLC-PDFParser.git```

## Usage
```mono PDFParser.exe [-h] [-b num] [-f char] <file path>```

Once you clone the project, you can either modify and build it yourself, or you can
use the pre-built exe.

If building yourself, the debug product is most likely in ```PDFParser/bin/Debug```, and
the release product is most likely in ```PDFParser/bin/Release```. Otherwise, there is
a pre-built executable in the Executable directory.

The executable is named ```PDFParser.exe```. To run it, open a command line shell,
navigate to the directory containing the executable, and run the above command.
This will parse the pdf, outputting the percentage of passed tests and routing
the results of failed tests to the ```out/``` directory.

### Options

* ```-h``` (Horizontal): If specified, the submission, solution, and original input
are printed horizontally adjacent. If not, they are printed vertically adjacent.
* ```-b num``` (Buffer): Specifies the number of non-diff (matching) characters to
print _around_ the diff (non-matching) characters in the two dimensional diff.
Defaults to 0 (meaning only the non-matching characters will be printed).
* ```-f char``` (Filler): Specifies what character to print instead of non-diff
characters in the two dimensional diff. All non-diff characters are replaced
with the given filler character. Defaults to '#'.
