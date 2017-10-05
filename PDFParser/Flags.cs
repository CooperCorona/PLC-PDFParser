using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PDFParser
{
    /// <summary>
    /// Groups together command line flags.
    /// </summary>
    public struct Flags
    {
        /// <summary>
        /// The command line flag corresponding to horizontal printing.
        /// </summary>
        public static string HORIZONTAL = "-h";
        /// <summary>
        /// The command line flag corresponding to the diff buffer amount.
        /// </summary>
        public static string BUFFER = "-b";
        /// <summary>
        /// The command line flag corresponding to the character filling
        /// in non
        /// </summary>
        public static string FILLER = "-f";

        /// <summary>
        /// Flag controlling whether submission, solution, and initial maze
        /// adjacent to each other horizontally or sequentially vertically.
        /// </summary>
        /// <value><c>true</c> if horizontal; otherwise, <c>false</c>.</value>
        public bool Horizontal { get; set; }
        /// <summary>
        /// How many non-diff characters (characters that match) to print
        /// around the diff characters (characters that don't match) in the
        /// two dimensional diff.
        /// </summary>
        /// <value>The buffer.</value>
        public int Buffer { get; set; }
        /// <summary>
        /// The character to print for non-diff characters in the two dimensional
        /// diff.
        /// </summary>
        /// <value>The filler.</value>
        public char Filler { get; set; }
        /// <summary>
        /// The reamining command line arguments after all flags have been
        /// parsed and removed from the list.
        /// </summary>
        public List<string> Arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.Flags"/> struct.
        /// </summary>
        /// <param name="horizontal">If set to <c>true</c> horizontal.</param>
        /// <param name="buffer">The number of characters to print around a two dimensional diff.</param>
        /// <param name="filler">The character to print instead of diff characters.</param>
        /// <param name="arguments">Arguments.</param>
        public Flags(bool horizontal, int buffer, char filler, List<String> arguments) {
            Horizontal = horizontal;
            Buffer = buffer;
            Filler = filler;
            Arguments = arguments;
        }

        /// <summary>
        /// Parses command line arguments into a Flags instance.
        /// </summary>
        /// <returns>The parse.</returns>
        /// <param name="args">An array of strings corresponding to command line arguments.</param>
        public static Flags Parse(string[] args) {
            var arguments = new List<string>(args);
            var flags = new Flags(false, 0, '#', arguments);
            flags = ParseHorizontal(flags, arguments);
            flags = ParseBuffer(flags, arguments);
            flags = ParseFiller(flags, arguments);
            return flags;
        }

        /// <summary>
        /// Parses the horizontal command line flag.
        /// </summary>
        /// <returns>The horizontal.</returns>
        /// <param name="flags">Flags.</param>
        /// <param name="arguments">Arguments.</param>
        private static Flags ParseHorizontal(Flags flags, List<string> arguments) {
			var horizontalFlag = arguments.IndexOf(Flags.HORIZONTAL);
			if (horizontalFlag != -1) {
				flags.Horizontal = true;
				arguments.RemoveAt(horizontalFlag);
			}
            return flags;
        }

        /// <summary>
        /// Parses the buffer command line flag. The buffer flag must be followed
        /// by an nonnegative integer.
        /// </summary>
        /// <returns>The buffer.</returns>
        /// <param name="flags">Flags.</param>
        /// <param name="arguments">Arguments.</param>
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

        /// <summary>
        /// Parses the filler command line flag. The filler flag must be followed
        /// by a string containing a single character.
        /// </summary>
        /// <returns>The filler.</returns>
        /// <param name="flags">Flags.</param>
        /// <param name="arguments">Arguments.</param>
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
