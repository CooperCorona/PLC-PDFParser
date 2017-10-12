using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDFParser
{
    /// <summary>
    /// Configurable writer to output a parsed test to a stream.
    /// </summary>
    public class DiffResultWriter
    {
        /// <summary>
        /// Whether the submission, solution, and original input should be
        /// horizontally adjacent or vertically adjacent.
        /// </summary>
        /// <value><c>true</c> if horizontal; otherwise, <c>false</c>.</value>
        public bool Horizontal { get; private set; }
        /// <summary>
        /// How many non-diff characters to print around the diff characters
        /// in two dimensional diffs.
        /// </summary>
        /// <value>The buffer.</value>
        public int Buffer { get; private set; }
        /// <summary>
        /// The character to print instead of non-diff characters in two
        /// dimensional diffs.
        /// </summary>
        /// <value>The filler.</value>
        public char Filler { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.DiffResultWriter"/> class.
        /// </summary>
        /// <param name="horizontal">If set to <c>true</c> horizontal.</param>
        /// <param name="buffer">Buffer.</param>
        /// <param name="filler">Filler.</param>
		public DiffResultWriter(bool horizontal, int buffer, char filler)
		{
            Horizontal = horizontal;
            Buffer = buffer;
            Filler = filler;
		}

        /// <summary>
        /// Writes the given parsed diff to the given writer. If the diff is empty
        /// (meaning the test passed), then no output is written.
        /// </summary>
        /// <returns><c>true</c> if output was written; <c>false</c>otherwise.</returns>
        /// <param name="result">Result.</param>
        /// <param name="writer">Writer.</param>
		public bool Write(DiffResult result, System.IO.TextWriter writer)
		{
			if (result.IsEmpty)
			{
				return false;
			}

            var diff2d = GetDiff2D(result);

			writer.Write("{0} Test {1,2}\n\n", result.SectionNumber, result.TestNumber);
			if (!this.Horizontal) {
				writer.Write("Submission:\n{0}\n\n", result.Submission);
				writer.Write("Solution:\n{0}\n\n", result.Solution);
				writer.Write("Initial Maze:\n{0}\n\n", result.Maze);
                if (diff2d.HasValue) {
                    writer.Write("Diff 2D:\n{0}\n\n", diff2d.Value.Actual);
                }
            } else {
                string horizontalMazes = GetHorizontalMazes(result.Submission, result.Solution, result.Maze);
                writer.Write("{0}\n\n", horizontalMazes);
                if (diff2d.HasValue) {
                    string horizontalDiffs = GetHorizontalMazes(diff2d.Value.Actual, diff2d.Value.Expected, diff2d.Value.Input);
                    writer.Write("Diff 2D:\n{0}\n\n", horizontalDiffs);
                }
            }

            writer.Write("Diff:\n{0}\n\n", result.Diff);

            writer.Write("Moves:\n{0}\n\n", result.Moves);
            if (result.Error == string.Empty) {
                writer.Write("No error.");
            } else {
                writer.Write("Error:\n{0}\n", result.Error);
            }
            return true;
		}

        /// <summary>
        /// Combines the rows of the submission, solution, and maze strings
        /// so they can be printed horizontally.
        /// </summary>
        /// <returns>A string containing the submission, solution, and maze
        /// strings arranged horizontally.</returns>
        /// <param name="submission">Submission.</param>
        /// <param name="solution">Solution.</param>
        /// <param name="maze">Maze.</param>
        /// <param name="buffer">How much space to place between each individual string.</param>
        private string GetHorizontalMazes(string submission, string solution, string maze, int buffer = 2) {
            string[] submissionLines = submission.Split('\n').Where(s => { return s.Length > 0;  }).ToArray();
            string[] solutionLines = solution.Split('\n').Where(s => { return s.Length > 0; }).ToArray();
            string[] mazeLines = maze.Split('\n').Where(s => { return s.Length > 0; }).ToArray();
            //If there is an error, submission is the empty string.
            int mazeWidth = 0;
            if (submissionLines.Length > 0) {
                mazeWidth = Math.Max(mazeWidth, submissionLines[0].Length);
            }
			if (solutionLines.Length > 0)
			{
				mazeWidth = Math.Max(mazeWidth, solutionLines[0].Length);
			}
			if (mazeLines.Length > 0)
			{
				mazeWidth = Math.Max(mazeWidth, mazeLines[0].Length);
			}

            //"Submission" is 10 characters long.
            int columns = Math.Max(mazeWidth, 10);

            var text = new System.Text.StringBuilder();
            text.Append("Submission");
            RPad(text, columns + buffer);
            text.Append("Solution");
            RPad(text, (columns + buffer) * 2);
            text.Append("Maze");
            RPad(text, (columns + buffer) * 3);
            for (int i = 0; i < submissionLines.Length; i++)
			{
                var line = new System.Text.StringBuilder();
                if (i < submissionLines.Length) {
                    line.Append(submissionLines[i]);
                }
                RPad(line, columns + buffer);
                if (i < solutionLines.Length) {
                    line.Append(solutionLines[i]);
                }
                RPad(line, (columns + buffer) * 2);
                if (i < mazeLines.Length) {
                    line.Append(mazeLines[i]);
                }
                RPad(line, (columns + buffer) * 3);
                //line.Append(submissionLines[i].RPad(' ', columns + buffer));
                //line.Append(solutionLines[i].RPad(' ', line.Length + columns + buffer));
                //line.Append(mazeLines[i].RPad(' ', line.Length + columns + buffer));
                text.Append("\n");
                text.Append(line);
            }
            return text.ToString();
        }

        /// <summary>
        /// Pads the end of a StringBuilder with spaces until its length equals
        /// the specified length.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <param name="length">Length.</param>
        private void RPad(System.Text.StringBuilder builder, int length) {
            while (builder.Length < length) {
                builder.Append(' ');
            }
        }


        /// <summary>
        /// Calculates the two dimensional diff from a DiffResult instance.
        /// </summary>
        /// <returns>The corresponding two dimensional diff if the submission
        /// string exists, <c>null</c> otherwise.</returns>
        /// <param name="result">Result.</param>
		private Diff2D? GetDiff2D(DiffResult result)
		{
			if (result.Submission != "")
			{
				var diff2d = new DiffGenerator2D(Buffer, Filler).Generate(result.Solution, result.Submission, result.Maze);
                return diff2d;
            } else {
                return null;
            }
        }

	}
}
