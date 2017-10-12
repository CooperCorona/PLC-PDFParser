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
                var submission = EnsureSameLengthLines(RemoveLineNumbers(submissionMatches[i].Groups[1].Value.Trim()));
                var solution = EnsureSameLengthLines(RemoveLineNumbers(solutionMatches[i].Groups[1].Value.Trim()));
                var error = TrimError(errorMatches[i].Groups[1].Value.Trim());
                var maze = EnsureSameLengthLines(RemoveLineNumbers(FormatMaze(mazeMatches[i].Groups[1].Value.Trim())));
                var directions = RemoveLineNumbers(directionsMatches[i].Groups[1].Value.Trim());
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

        /// <summary>
        /// Removes line numbers from a string. The PDF parser sometimes includes
        /// line numbers in the middle of other strings. It appears that they
        /// are always at the beginning or the end of a line. If a number
        /// appears at the beginning or end of a line, it is removed.
        /// </summary>
        /// <returns>A string with the line numbers removed.</returns>
        /// <param name="input">Input.</param>
        private string RemoveLineNumbers(string input) {
            if (input == "") {
                return "";
            }
            var lines = input.Split('\n');
            int mode = LengthMode(input);
            return lines.Select(line => {
                if (line.Length == mode)
                {
                    return line;
                }
                else
                {
                    var result = new Regex(@"^\d+").Replace(line, "");
                    if (line.Length == mode)
                    {
                        return result;
                    }
                    else
                    {
                        return new Regex(@"\d+$").Replace(result, "");
                    }
                }
            }).Aggregate("", (a, b) => {
                return a + "\n" + b;
            }).Substring(1);
        }

        /// <summary>
        /// The mode of the set of lengths of lines in a string. Whatever
        /// number occurs the most as length of characters in a line is returned.
        /// </summary>
        /// <returns>The line length that occurs the most.</returns>
        /// <param name="input">Input.</param>
        private int LengthMode(string input)
        {
            string[] lines = input.Split('\n');
            var modes = new System.Collections.Generic.Dictionary<int, int>();
            foreach (var line in lines)
            {
                if (modes.ContainsKey(line.Length))
                {
                    modes[line.Length] += 1;
                }
                else
                {
                    modes[line.Length] = 1;
                }
            }
            return modes.Aggregate(modes.First(), (a, b) => {
                if (b.Value > a.Value)
                {
                    return b;
                }
                else
                {
                    return a;
                }

            }).Key;
        }

        /// <summary>
        /// Removes any lines in the input string that does not have the same
        /// length as the mode of the lengths.
        /// </summary>
        /// <returns>The same length lines.</returns>
        /// <param name="input">Input.</param>
        private string EnsureSameLengthLines(string input) {
            if (input == "") {
                return "";
            }

            string[] lines = input.Split('\n');
            int maxLength = LengthMode(input);
            return lines.Where(l => { return l.Length == maxLength; }).Aggregate("", (a, b) => {
                return a + "\n" + b;
            }).Substring(1);
        }
    }
}
