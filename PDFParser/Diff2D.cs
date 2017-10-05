using System;
namespace PDFParser
{
    public struct Diff2D
    {
        public string Expected { get; private set; }
        public string Actual { get; private set; }
        public string Input { get; private set; }

        public Diff2D(string expected, string actual, string input) {
            Expected = expected;
            Actual = actual;
            Input = input;
        }
    }
}
