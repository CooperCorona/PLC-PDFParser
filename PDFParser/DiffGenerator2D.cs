using System;
using System.Linq;
using System.Collections.Generic;

namespace PDFParser
{
    public class DiffGenerator2D
    {
        int Buffer { get; set; }
        char Filler { get; set; }

        public DiffGenerator2D(int buffer = 0, char filler = ' ')
        {
            Buffer = buffer;
            Filler = filler;
        }

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

        private Point ToPoint(int i, int width) {
            return new Point(i % width, i / width);
        }

        private bool NearBy(Point point, IEnumerable<Point> points, int nearness) {
            foreach (var p in points) {
                if (point.NearBy(p, nearness)) {
                    return true;
                }
            }
            return false;
        }

        private char[] Split(string s) {
            return s.Split("\n").SelectMany(str => { return str.ToCharArray(); } ).ToArray();
        }

    }
}
