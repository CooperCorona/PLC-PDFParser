using System;
namespace PDFParser
{
    /// <summary>
    /// string extensions.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Pads the start of a string with a given character until its length
        /// is greater than or equal to the given legnth.
        /// </summary>
        /// <returns>The string padded on the left.</returns>
        /// <param name="s">The string to pad.</param>
        /// <param name="with">The character to pad with.</param>
        /// <param name="length">The length to which to pad the string.</param>
        public static string LPad(this string s, char with, int length) {
            string result = s;
            while (result.Length < length) {
                result = with + result;
            }
            return result;
        }

		/// <summary>
		/// Pads the end of a string with a given character until its length
		/// is greater than or equal to the given legnth.
		/// </summary>
		/// <returns>The string padded on the right.</returns>
		/// <param name="s">The string to pad.</param>
		/// <param name="with">The character to pad with.</param>
		/// <param name="length">The length to which to pad the string.</param>
		public static string RPad(this string s, char with, int length)
		{
			string result = s;
			while (result.Length < length)
			{
				result = result + with;
			}
			return result;
		}

    }
}
