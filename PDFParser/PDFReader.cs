using System;
namespace PDFParser
{
    public class PDFReader
    {
        private IInputAccessor InputAccessor { get; }
        private IOutputGenerator OutputGenerator { get; }

        public PDFReader(IInputAccessor inputAccessor, IOutputGenerator outputGenerator)
        {
            InputAccessor = inputAccessor;
            OutputGenerator = outputGenerator;
        }

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
