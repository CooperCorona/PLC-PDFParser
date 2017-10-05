using System;
namespace PDFParser
{
    /// <summary>
    /// Reads the text of a pdf from an IInputAccessor and outputs all failing
    /// tests to an IOutputGenerator.
    /// </summary>
    public class PDFReader
    {
        /// <summary>
        /// Used to access the text to parse.
        /// </summary>
        /// <value>The input accessor.</value>
        private IInputAccessor InputAccessor { get; }
        /// <summary>
        /// Used to output the failing tests.
        /// </summary>
        /// <value>The output generator.</value>
        private IOutputGenerator OutputGenerator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.PDFReader"/> class.
        /// </summary>
        /// <param name="inputAccessor">Input accessor.</param>
        /// <param name="outputGenerator">Output generator.</param>
        public PDFReader(IInputAccessor inputAccessor, IOutputGenerator outputGenerator)
        {
            InputAccessor = inputAccessor;
            OutputGenerator = outputGenerator;
        }

        /// <summary>
        /// Parses the text received from the IInputAccessor and outputs failing
        /// tests to OutputGenerator.
        /// </summary>
        public void Parse() {
            OutputGenerator.InitializeOutput();
            var contents = InputAccessor.GetContents();
			var parser = new OutputParser();
			var results = parser.Parse(contents);
			var passed = 0;
			foreach (var result in results)
			{
				if (result.IsEmpty)
				{
					Console.WriteLine(String.Format("{0,2} Test {1,2} Passed.", result.SectionNumber, result.TestNumber));
					passed++;
				}
				else
				{
					Console.WriteLine(String.Format("{0,2} Test {1,2} Failed: XXXX", result.SectionNumber, result.TestNumber));
                    OutputGenerator.WriteDiff(result);
                }
			}
			var percent = (double)passed / (double)results.Count;
			Console.WriteLine("Passed {0} / {1} = {2} %", passed, results.Count, percent * 100);
        }
    }
}
