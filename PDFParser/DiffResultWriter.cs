using System;
using System.Linq;

namespace PDFParser
{

	public class DiffResultWriter
	{
        public bool Vertical { get; private set; }

		public DiffResultWriter(bool vertical)
		{
            Vertical = vertical;
		}

		public bool Write(DiffResult result, System.IO.TextWriter writer)
		{
			if (result.IsEmpty)
			{
				return false;
			}
			writer.Write("{0} Test {1,2}\n\n", result.SectionNumber, result.TestNumber);
			if (this.Vertical) {
				writer.Write("Submission:\n{0}\n\n", result.Submission);
				writer.Write("Solution:\n{0}\n\n", result.Solution);
				writer.Write("Initial Maze:\n{0}\n\n", result.Maze);
            } else {
                string horizontalMazes = GetHorizontalMazes(result.Submission, result.Solution, result.Maze);
                writer.Write("{0}\n\n", horizontalMazes);
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

        private string GetHorizontalMazes(string submission, string solution, string maze, int buffer = 2) {
            string[] submissionLines = submission.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            string[] solutionLines = solution.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            string[] mazeLines = maze.Split("\n", StringSplitOptions.RemoveEmptyEntries);
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

            //Sometimes the PDF parser includes a random number (probably page number)
            //at the end of the submission. This causes an incorrect number of lines,
            //which causes a crash when the for loop tries to go beyond the bounds
            //of the other arrays. We filter out all too-small lines (the first line
            //should be the correct length).
            //submissionLines = submissionLines.Where(s => { return s.Length == mazeWidth; }).ToArray();
            //solutionLines = solutionLines.Where(s => { return s.Length == mazeWidth; }).ToArray();
            //mazeLines = mazeLines.Where(s => { return s.Length == mazeWidth; }).ToArray();

            //"Submission" is 10 characters long.
            int columns = Math.Max(mazeWidth, 10);

            var text = new System.Text.StringBuilder();
            text.Append("Submission");
            RPad(text, columns + buffer);
            text.Append("Solution");
            RPad(text, (columns + buffer) * 2);
            text.Append("Maze");
            RPad(text, (columns + buffer) * 3);
            Console.WriteLine("1: {0}", submissionLines.Length);
            Console.WriteLine(submission);
            Console.WriteLine("2: {0}", solutionLines.Length);
            Console.WriteLine(solution);
            Console.WriteLine("3: {0}", mazeLines.Length);
            Console.WriteLine(maze);
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

        private void RPad(System.Text.StringBuilder builder, int length) {
            while (builder.Length < length) {
                builder.Append(' ');
            }
        }

	}
}
