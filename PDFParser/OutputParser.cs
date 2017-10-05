using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace PDFParser
{
    public class OutputParser
    {

        public OutputParser()
        {
        }

        /// <summary>
        /// Parses the contents of a WebGrader result into a list of DiffResults.
        /// </summary>
        /// <returns>A list of DiffResults corresponding to WebGrader tests.</returns>
        /// <param name="contents">Text of the WebGrader results.</param>
		public List<DiffResult> Parse(string contents)
		{
            //Splitting by "Chapter 2" removes the table of contents.
            contents = contents.Split(new string[] { "Chapter 2" }, StringSplitOptions.None)[1];
            var diffs = ParseDiffs(contents);
            return diffs;
        }

		/// <summary>
		/// Parses the contents of a WebGrader result into a list of DiffResults.
		/// </summary>
		/// <returns>A list of DiffResults corresponding to WebGrader tests.</returns>
		/// <param name="contents">Text of the WebGrader results.</param>
		private List<DiffResult> ParseDiffs(string contents) {
            var regex = new Regex(@"part(\d\d)test(\d\d)\.dif?f?(.+?)\d+\.\d+\.\d+ Input File", RegexOptions.Singleline);
            var submissionRegex = new Regex(@"\d\.\d+\.\d Submission Output.*?\d?\d?part\d\dtest\d\d\.output(.+?)\d\.\d+\.\d Solution Output", RegexOptions.Singleline);
            var solutionRegex = new Regex(@"\d\.\d+\.\d Solution Output.*?part\d\dtest\d\d\.solution(.+?)\d\.\d+\.\d stderr", RegexOptions.Singleline);
			var errorRegex = new Regex(@"\d\.\d+\.\d stderr.*?part\d\dtest\d\d\.err(.*?)\d\.\d+", RegexOptions.Singleline);
			var mazeRegex = new Regex(@"part\d\dtest\d\d\.maze\.emf(.+?)\d\.\d+\.\d Submission Output", RegexOptions.Singleline);
            var directionsRegex = new Regex(@"part\d\dtest\d\d\.moves\.emf(.+?)part\d\dtest\d\d\.maze\.emf", RegexOptions.Singleline);
            var matches = regex.Matches(contents);
            var submissionMatches = submissionRegex.Matches(contents);
            var solutionMatches = solutionRegex.Matches(contents);
            var errorMatches = errorRegex.Matches(contents);
            var mazeMatches = mazeRegex.Matches(contents);
            var directionsMatches = directionsRegex.Matches(contents);

            var diffs = new List<DiffResult>();
            for (int i = 0; i < matches.Count; i++) {
                var match = matches[i];
                var sectionNumber = int.Parse(match.Groups[1].Value);
                var testNumber = int.Parse(match.Groups[2].Value);
                var diff = match.Groups[3].Value.Trim();
                var submission = submissionMatches[i].Groups[1].Value.Trim();
                var solution = solutionMatches[i].Groups[1].Value.Trim();
                var error = TrimError(errorMatches[i].Groups[1].Value.Trim());
                var maze = FormatMaze(mazeMatches[i].Groups[1].Value.Trim());
                var directions = directionsMatches[i].Groups[1].Value.Trim();
                var result = new DiffResult(diff == "" && error == "", sectionNumber, testNumber, diff, submission, solution, error, maze, directions);
                diffs.Add(result);
            }
            return diffs;
        }

        /// <summary>
        /// Parses the original input maze (which is a CSV of maze characters)
        /// into the same format as the submission and solution strings.
        /// </summary>
        /// <returns>The maze.</returns>
        /// <param name="maze">Maze.</param>
        private string FormatMaze(string maze) {
            return maze.Replace(",", "");
        }

        /// <summary>
        /// Removes whitespace and exclusively-integer strings.
        /// Because page numbers get randomly included in the
        /// output, sometimes a no-error test will be considered
        /// an error because it contains a number.
        /// </summary>
        /// <returns>The error.</returns>
        private string TrimError(string error) {
            return new Regex(@"^\s*\d+\s*$").Replace(error, "");
        }

    }
}
