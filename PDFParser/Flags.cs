using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PDFParser
{
    public struct Flags
    {

        public static string HORIZONTAL = "-h";
        public static string BUFFER = "-b";
        public static string FILLER = "-f";

        public bool Horizontal { get; set; }
        public int Buffer { get; set; }
        public char Filler { get; set; }
        public List<string> Arguments;

        public Flags(bool horizontal, int buffer, char filler, List<String> arguments) {
            Horizontal = horizontal;
            Buffer = buffer;
            Filler = filler;
            Arguments = arguments;
        }

        public static Flags Parse(string[] args) {
            var arguments = new List<string>(args);
            var flags = new Flags(false, 0, '#', arguments);
            flags = ParseHorizontal(flags, arguments);
            flags = ParseBuffer(flags, arguments);
            flags = ParseFiller(flags, arguments);
            return flags;
        }

        private static Flags ParseHorizontal(Flags flags, List<string> arguments) {
			var horizontalFlag = arguments.IndexOf(Flags.HORIZONTAL);
			if (horizontalFlag != -1) {
				flags.Horizontal = true;
				arguments.RemoveAt(horizontalFlag);
			}
            return flags;
        }

		private static Flags ParseBuffer(Flags flags, List<string> arguments) {
			var bufferFlag = arguments.IndexOf(Flags.BUFFER);
			if (bufferFlag != -1) {
				if (bufferFlag >= arguments.Count - 1) {
					throw new ArgumentException(Flags.BUFFER + " flag must have an argument after it.");
				}
				var bufferArg = arguments[bufferFlag + 1];
				if (!new Regex(@"^\d+$").IsMatch(bufferArg)) {
					throw new ArgumentException(Flags.BUFFER + " flag's argument must be a nonnegative integer.");
				}
				flags.Buffer = int.Parse(bufferArg);
				arguments.RemoveAt(bufferFlag + 1);
				arguments.RemoveAt(bufferFlag);
			}
            return flags;
        }

        private static Flags ParseFiller(Flags flags, List<string> arguments) {
			var fillerFlag = arguments.IndexOf(Flags.FILLER);
			if (fillerFlag != -1) {
				if (fillerFlag >= arguments.Count - 1) {
					throw new ArgumentException(Flags.FILLER + " flag must have an argument after it.");
				}
				var filler = arguments[fillerFlag + 1];
				if (filler.Length > 1) {
					throw new ArgumentException(Flags.FILLER + " flag's argument must be a single character.");
				}
				flags.Filler = filler[0];
				arguments.RemoveAt(fillerFlag + 1);
				arguments.RemoveAt(fillerFlag);
			}
            return flags;
        }
    }
}
