using System;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDFParser
{
    public class FileInputAccessor: IInputAccessor
    {
        public string FilePath { get; private set; }

        public FileInputAccessor(string filePath)
        {
            FilePath = filePath;
        }

        public string GetContents() {
            return ReadEntirePDF();
        }

		/// <summary>
		/// Reads the entire contents of the PDF into memory. The entire PDF
		/// must be read at once because some diffs span multiple pages. It
		/// wouldn't be impossible to do it piecewise, but it's not really
		/// worth the effort. 30 pages is a lot, but not a lot for a computer.
		/// </summary>
		/// <returns>The text of the PDF.</returns>
		private string ReadEntirePDF()
		{
			string text = string.Empty;
			using (PdfReader reader = new PdfReader(FilePath))
			{
				for (int i = 1; i <= reader.NumberOfPages; i++)
				{
					ITextExtractionStrategy extractionStrategy = new LocationTextExtractionStrategy();
					text += PdfTextExtractor.GetTextFromPage(reader, i, extractionStrategy);
				}
			}
			return text;
		}
    }
}
