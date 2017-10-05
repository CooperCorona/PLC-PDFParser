using System;
using System.Linq;
using System.Collections.Generic;

namespace PDFParser
{
    /// <summary>
    /// Parses an expected and actual output, replacing identical (non-diff)
    /// characters with filler to see the diff in its original structure.
    /// </summary>
    public class DiffGenerator2D
    {
        /// <summary>
        /// The number of non-diff characters to print around diff characters.
        /// </summary>
        /// <value>The buffer.</value>
        int Buffer { get; set; }
        /// <summary>
        /// The character to print instead of non-diff characters. Defaults to
        /// 0 (only diff characters, no non-diff characters).
        /// </summary>
        /// <value>The filler.</value>
        char Filler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.DiffGenerator2D"/> class.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="filler">Filler.</param>
        public DiffGenerator2D(int buffer = 0, char filler = ' ')
        {
            Buffer = buffer;
            Filler = filler;
        }

        /// <summary>
        /// Generates a two dimensional diff by comparing expected and actual
        /// output by replacing non-diff characters with a filler character.
        /// </summary>
        /// <returns>The generate.</returns>
        /// <param name="expected">Expected.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="maze">Maze.</param>
        public Diff2D Generate(string expected, string actual, string maze) {
            char[] expectedChars = Split(expected);
            char[] actualChars = Split(actual);
            char[] mazeChars = Split(maze);
            if (expectedChars.Length != actualChars.Length) {
                throw new ArgumentException("Arguments must have same length.");
            }

            int width = expected.Split('\n')[0].Length;
            var diffPoints = new HashSet<Point>();
            for (int i = 0; i < expectedChars.Length; i++) {
                if (expectedChars[i] != actualChars[i]) {
                    diffPoints.Add(ToPoint(i, width));
                }
            }

            var expectedDiff = new System.Text.StringBuilder();
            var actualDiff = new System.Text.StringBuilder();
            var mazeDiff = new System.Text.StringBuilder();
            for (int i = 0; i < expectedChars.Length; i++) {
                var currentPoint = ToPoint(i, width);
                if (NearBy(currentPoint, diffPoints, Buffer)) {
                    expectedDiff.Append(expectedChars[i]);
                    actualDiff.Append(actualChars[i]);
                    mazeDiff.Append(mazeChars[i]);
				}
				else
				{
					expectedDiff.Append(Filler);
					actualDiff.Append(Filler);
                    mazeDiff.Append(Filler);
                }
                if ((i + 1) % width == 0) {
                    expectedDiff.Append('\n');
                    actualDiff.Append('\n');
                    mazeDiff.Append('\n');
                }
            }
            return new Diff2D(expectedDiff.ToString(), actualDiff.ToString(), mazeDiff.ToString());
        }

        /// <summary>
        /// Converts an index into a character array into a Point.
        /// </summary>
        /// <returns>The point corresponding to the given index.</returns>
        /// <param name="i">The index into the character array.</param>
        /// <param name="width">The width of the rows in the original array.</param>
        private Point ToPoint(int i, int width) {
            return new Point(i % width, i / width);
        }

        /// <summary>
        /// Determines if a point is "near" a point in a set of points.
        /// </summary>
        /// <returns><c>true</c>, if the given point is considered "near" to any
        /// of the passed in points, <c>false</c> otherwise.</returns>
        /// <param name="point">Point.</param>
        /// <param name="points">Points.</param>
        /// <param name="nearness">How near the point must be.</param>
        private bool NearBy(Point point, IEnumerable<Point> points, int nearness) {
            foreach (var p in points) {
                if (point.NearBy(p, nearness)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Splits a string containing rows into a 1d character array (so the
        /// last character in a row is adjacent to the first character in the
        /// next row).
        /// </summary>
        /// <returns>The split.</returns>
        /// <param name="s">S.</param>
        private char[] Split(string s) {
            return s.Split('\n').SelectMany(str => { return str.ToCharArray(); } ).ToArray();
        }

    }
}
