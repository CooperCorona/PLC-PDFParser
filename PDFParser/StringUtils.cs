using System;
namespace PDFParser
{
    public static class StringUtils
    {

        public static string[] Split(this string s, string separator, StringSplitOptions options = StringSplitOptions.None) {
            return s.Split(new string[] { separator }, options);
        }

        public static string LPad(this string s, char with, int length) {
            string result = s;
            while (result.Length < length) {
                result = with + result;
            }
            return result;
        }

		public static string LPadPlus(this string s, char with, int length)
		{
            return s.LPad(with, s.Length + length);
		}

		public static string RPad(this string s, char with, int length)
		{
			string result = s;
			while (result.Length < length)
			{
				result = result + with;
			}
			return result;
		}

		public static string RPadPlus(this string s, char with, int length)
		{
			return s.RPad(with, s.Length + length);
		}
    }
}
