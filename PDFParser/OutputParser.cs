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

		public List<DiffResult> Parse(string contents)
		{
            //Splitting by "Chapter 2" removes the table of contents.
            contents = contents.Split(new string[] { "Chapter 2" }, StringSplitOptions.None)[1];
            System.IO.File.WriteAllText("./output.txt", contents);
            var diffs = ParseDiffs(contents);
            return diffs;
        }

        private List<DiffResult> ParseDiffs(string contents) {
            var regex = new Regex(@"part(\d\d)test(\d\d)\.dif?f?(.+?)\d+\.\d+\.\d+ Input File", RegexOptions.Singleline);
            var submissionRegex = new Regex(@"\d\.\d+\.\d Submission Output.*?\d?\d?part\d\dtest\d\d\.output(.+?)\d\.\d+\.\d Solution Output", RegexOptions.Singleline);
            var solutionRegex = new Regex(@"\d\.\d+\.\d Solution Output.*?part\d\dtest\d\d\.solution(.+?)\d\.\d+\.\d stderr", RegexOptions.Singleline);
            var errorRegex = new Regex(@"\d\.\d+\.\d stderr.*?part\d\dtest\d\d\.err(.*?)\d\.\d+", RegexOptions.Singleline);
            var matches = regex.Matches(contents);
            var submissionMatches = submissionRegex.Matches(contents);
            var solutionMatches = solutionRegex.Matches(contents);
            var errorMatches = errorRegex.Matches(contents);
            var diffs = new List<DiffResult>();
            //Console.WriteLine(@"Matches: {0}", matches.Count);
            //Console.WriteLine(@"Submissions: {0}", submissionMatches.Count);
            //Console.WriteLine(@"Solutions: {0}", solutionMatches.Count);
            //Console.WriteLine(@"Errors: {0}", errorMatches.Count);
            for (int i = 0; i < matches.Count; i++) {
                var match = matches[i];
                var sectionNumber = int.Parse(match.Groups[1].Value);
                var testNumber = int.Parse(match.Groups[2].Value);
                //Console.WriteLine("({0}, {1})", sectionNumber, testNumber);
                var diff = match.Groups[3].Value.Trim();
                //Console.WriteLine(diff);
                var submission = submissionMatches[i].Groups[1].Value.Trim();
                var solution = solutionMatches[i].Groups[1].Value.Trim();
                var error = errorMatches[i].Groups[1].Value.Trim();
                var result = new DiffResult(diff == "" && error == "", sectionNumber, testNumber, diff, submission, solution, error);
                diffs.Add(result);
            }
            return diffs;
        }
    }
}
