using System;
namespace PDFParser
{

	public class DiffResultWriter
	{
		public DiffResultWriter()
		{
		}

		public bool Write(DiffResult result, System.IO.TextWriter writer)
		{
			if (result.IsEmpty)
			{
				return false;
			}
			writer.Write("{0} Test {1,2}\n\n", result.SectionNumber, result.TestNumber);
            writer.Write("Diff:\n{0}\n\n", result.Diff);
            writer.Write("Submission:\n{0}\n\n", result.Submission);
            writer.Write("Solution:\n{0}\n\n", result.Solution);
            if (result.Error == string.Empty) {
                writer.Write("No error.");
            } else {
                writer.Write("Error:\n{0}\n", result.Error);
            }
            return true;
		}
	}
}
