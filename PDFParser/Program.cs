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
            var flags = Flags.Parse(args);
            IInputAccessor inputAccessor = null;
            Console.WriteLine(String.Join(", ", flags.Arguments));
            if (flags.Arguments.Count == 1)
			{
                string fileName = flags.Arguments[0];
				string path = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
				inputAccessor = new FileInputAccessor(path);
            } else {
                throw new NotImplementedException("Automatic grading not yet implemented.");
            }
            var outputGenerator = new IndividualFileOutputGenerator(flags.Horizontal, flags.Buffer, flags.Filler);
            var reader = new PDFReader(inputAccessor, outputGenerator);
            reader.Parse();
        }

    }
}
