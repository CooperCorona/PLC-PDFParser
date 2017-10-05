using System;
namespace PDFParser
{
    /// <summary>
    /// Represents a two dimensional diff. A two dimensional diff is the same
    /// size as the original values, but the only characters that are the same
    /// as the original string are diff characters (characters that do not match).
    /// Other characters are "filler" characters.
    /// </summary>
    public struct Diff2D
    {
        public string Expected { get; private set; }
        public string Actual { get; private set; }
        public string Input { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.Diff2D"/> struct.
        /// </summary>
        /// <param name="expected">Expected.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="input">Input.</param>
        public Diff2D(string expected, string actual, string input) {
            Expected = expected;
            Actual = actual;
            Input = input;
        }
    }
}
