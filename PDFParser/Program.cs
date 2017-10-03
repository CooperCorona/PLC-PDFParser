using System;

namespace PDFParser
{
    class MainClass
    {

        public static void Main(string[] args)
		{
            if (args.Length == 0) {
                Console.WriteLine("Usage:");
                Console.WriteLine("mono PDFParser.exe <file path>");
                Console.WriteLine("mono PDFParser.exe <username> <cse password>");
                return;
            }
            IInputAccessor inputAccessor = null;
			if (args.Length == 1)
			{
				string fileName = args[0];
				string path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
				inputAccessor = new FileInputAccessor(path);
            } else {
                throw new NotImplementedException("Automatic grading not yet implemented.");
            }
            var outputGenerator = new IndividualFileOutputGenerator();
            var reader = new PDFReader(inputAccessor, outputGenerator);
            reader.Parse();
        }

    }
}
