using System;
namespace PDFParser
{
    /// <summary>
    /// Interface to read the contents of a pdf.
    /// </summary>
    public interface IInputAccessor
    {
        /// <summary>
        /// Gets the contents to be parsed.
        /// </summary>
        /// <returns>The text to be parsed.</returns>
        string GetContents();

    }
}
