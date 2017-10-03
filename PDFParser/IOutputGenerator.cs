using System;
namespace PDFParser
{
    public interface IOutputGenerator
    {
        void InitializeOutput();

        void WriteDiff(DiffResult result);
    }
}
