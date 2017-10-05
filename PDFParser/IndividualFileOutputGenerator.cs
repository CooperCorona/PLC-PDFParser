using System;
using System.Text.RegularExpressions;
            
namespace PDFParser
{
    /// <summary>
    /// Implements the IOutputGenerator interface to output all failing tests
    /// to plain text files.
    /// </summary>
    public class IndividualFileOutputGenerator: IOutputGenerator
	{
        /// <summary>
        /// The directory the output files are placed in.
        /// </summary>
		private static string OUT_DIRECTORY = "out";
        /// <summary>
        /// Determines whether the submission, solution, and maze strings are
        /// horizontally adjacent or vertically adjacent.
        /// </summary>
        /// <value><c>true</c> if horizontal; otherwise, <c>false</c>.</value>
		public bool Horizontal { get; private set; }
        /// <summary>
        /// How many non-diff characters to print next to the diff characters
        /// in two dimensional diffs.
        /// </summary>
        /// <value>The buffer.</value>
		public int Buffer { get; private set; }
        /// <summary>
        /// The character to print instead of non-diff characters.
        /// </summary>
        /// <value>The filler.</value>
		public char Filler { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.IndividualFileOutputGenerator"/> class.
        /// </summary>
        /// <param name="horizontal">If set to <c>true</c> horizontal.</param>
        /// <param name="buffer">Buffer.</param>
        /// <param name="filler">Filler.</param>
		public IndividualFileOutputGenerator(bool horizontal, int buffer, char filler)
		{
			Horizontal = horizontal;
			Buffer = buffer;
			Filler = filler;
		}

        /// <summary>
        /// Creates the output directory and removes all old output files. Any
        /// file matching the regex /part\d+test\d+\.txt/ is removed.
        /// </summary>
        public void InitializeOutput() {
			System.IO.Directory.CreateDirectory(OUT_DIRECTORY);
			var fileRegex = new Regex(@"part\d+test\d+\.txt");
			foreach (var filePath in System.IO.Directory.EnumerateFiles(OUT_DIRECTORY))
			{
				if (fileRegex.IsMatch(filePath))
				{
					System.IO.File.Delete(filePath);
				}
			}
        }

        /// <summary>
        /// Pads the start of a string with 0 until the string is z characters
        /// long.
        /// </summary>
        /// <returns>A string of length at least z.</returns>
        /// <param name="n">N.</param>
        /// <param name="z">The desired length of the string.</param>
        private string LPad(int n, int z) {
            string s = string.Format("{0}", n);
            while (s.Length < z) {
                s = string.Format("0{0}", s);
            }
            return s;
        }

        /// <summary>
        /// Gets the file path corresponding to a given test.
        /// </summary>
        /// <returns>The file path.</returns>
        /// <param name="sectionNumber">Section number.</param>
        /// <param name="testNumber">Test number.</param>
        private string GetFilePath(int sectionNumber, int testNumber) {
            string sectionString = LPad(sectionNumber, 2);
            string testString = LPad(testNumber, 2);
            return string.Format("{0}/part{1}test{2}.txt", OUT_DIRECTORY, sectionString, testString);
        }

        /// <summary>
        /// Writes the diff result to a file.
        /// </summary>
        /// <param name="result">Result.</param>
		public void WriteDiff(DiffResult result) {
            var outPath = GetFilePath(result.SectionNumber, result.TestNumber);
			using (var streamWriter = new System.IO.StreamWriter(outPath)) {
                var diffWriter = new DiffResultWriter(Horizontal, Buffer, Filler);
				diffWriter.Write(result, streamWriter);
			}
        }
    }
}
