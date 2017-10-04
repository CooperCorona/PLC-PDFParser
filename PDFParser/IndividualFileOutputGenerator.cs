using System;
using System.Text.RegularExpressions;
            
namespace PDFParser
{
    public class IndividualFileOutputGenerator: IOutputGenerator
	{
		private static string OUT_DIRECTORY = "out";

        public IndividualFileOutputGenerator()
        {
        }

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

        private string LPad(int n, int z) {
            string s = string.Format("{0}", n);
            while (s.Length < z) {
                s = string.Format("0{0}", s);
            }
            return s;
        }

        private string GetFilePath(int sectionNumber, int testNumber) {
            string sectionString = LPad(sectionNumber, 2);
            string testString = LPad(testNumber, 2);
            return string.Format("{0}/part{1}test{2}.txt", OUT_DIRECTORY, sectionString, testString);
        }

		public void WriteDiff(DiffResult result) {
            var outPath = GetFilePath(result.SectionNumber, result.TestNumber);
			using (var streamWriter = new System.IO.StreamWriter(outPath)) {
				var diffWriter = new DiffResultWriter(false);
				diffWriter.Write(result, streamWriter);
			}
        }
    }
}
