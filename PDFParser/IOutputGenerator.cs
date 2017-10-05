using System;
namespace PDFParser
{
    /// <summary>
    /// Interface to output the results of failing tests.
    /// </summary>
    public interface IOutputGenerator
    {
        /// <summary>
        /// Performs any pre-output initialization.
        /// </summary>
        void InitializeOutput();

        /// <summary>
        /// Output the diff. The diff is guaranteed to represent a failing
        /// test.
        /// </summary>
        /// <param name="result">Result.</param>
        void WriteDiff(DiffResult result);
    }
}
