using System;

namespace ApplicationProperties {
    /// <summary>
    /// Extension methods for strings.
    /// </summary>
    public static class StringExtensions {

        /// <summary>
        /// Removes characters that are encapsulating the string.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <param name="start">The leading character/string.</param>
        /// <param name="end">The trailing character/string.</param>
        /// <returns>The input string without the leading start string and trailing end string.</returns>
        public static string Uncapsulate(this string s, string start, string end) {
            if (s.StartsWith(start)) { s = s.Substring(start.Length); }
            if(s.EndsWith(end)) { s = s.Substring(0, s.Length - end.Length); }
            return s;
        }

        /// <summary>
        /// Encapsulates the input string with leading and trailing characters/strings.
        /// </summary>
        /// <param name="s">The input string to encapsulate.</param>
        /// <param name="start">The leading characters/string.</param>
        /// <param name="end">The trailing characters/string.</param>
        /// <returns>The encapsulated input string.</returns>
        public static string Encapsultate(this string s, string start, string end) {
            if(!s.StartsWith(start, StringComparison.InvariantCultureIgnoreCase)) { s = $"{start}{s}"; }
            if(!s.EndsWith(end, StringComparison.InvariantCultureIgnoreCase)) { s = $"{s}{end}"; }
            return s;
        }

        /// <summary>
        /// Checks if a string is null or empty.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <returns>True if the string is null or empty.</returns>
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        /// <summary>
        /// Checks if a string is encapsulated.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <param name="start">The leading string.</param>
        /// <param name="end">The tailing string.</param>
        /// <returns>True if the string is encapsulated.</returns>
        public static bool IsEncapsulated(this string s, string start, string end) => s.StartsWith(start, StringComparison.InvariantCultureIgnoreCase)
                                                                                   && s.EndsWith(end, StringComparison.InvariantCultureIgnoreCase);
    }
}
