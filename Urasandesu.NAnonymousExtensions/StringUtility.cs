using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Urasandesu.NAnonymousExtensions
{
    public static class StringUtility
    {
        public static class Csv
        {
            public static class Default
            {
                public const string Quote = "\"";
                public const string Delimiter = ",";
            }
        }

        public static class CommandPrompt
        {
            public static class Default
            {
                public const string Quote = "\"";
                public const string Delimiter = " ";
            }
        }

        public static string CsvEncode(this string s)
        {
            return CsvEncode(s, Csv.Default.Quote);
        }

        public static string CsvEncode(this string s, string quote)
        {
            if (quote == null) throw new ArgumentNullException("quote");
            return s == null ? null : s.Replace(quote, quote + quote);
        }

        public static string CsvEncode(this IEnumerable<string> line)
        {
            return CsvEncode(line, Csv.Default.Quote);
        }

        public static string CsvEncode(this IEnumerable<string> line, string quote)
        {
            if (quote == null) throw new ArgumentNullException("quote");
            return CsvEncode(line, quote, Csv.Default.Delimiter);
        }

        public static string CsvEncode(this IEnumerable<string> line, string quote, string delimiter)
        {
            if (delimiter == null) throw new ArgumentNullException("delimiter");
            return line == null ? null : string.Join(delimiter, line.Select(s => quote + s.CsvEncode(quote) + quote).ToArray());
        }

        public static string CommandPromptEncode(this string s)
        {
            return CommandPromptEncode(s, CommandPrompt.Default.Quote);
        }

        public static string CommandPromptEncode(this string s, string quote)
        {
            if (quote == null) throw new ArgumentNullException("quote");
            return s == null ? null : s.Replace(quote, quote + quote + quote);
        }

        public static string CommandPromptEncode(this IEnumerable<string> line)
        {
            return CommandPromptEncode(line, CommandPrompt.Default.Quote);
        }

        public static string CommandPromptEncode(this IEnumerable<string> line, string quote)
        {
            if (quote == null) throw new ArgumentNullException("quote");
            return CommandPromptEncode(line, quote, CommandPrompt.Default.Delimiter);
        }

        public static string CommandPromptEncode(this IEnumerable<string> line, string quote, string delimiter)
        {
            if (delimiter == null) throw new ArgumentNullException("delimiter");
            return line == null ? null : string.Join(delimiter, line.Select(s => quote + s.CommandPromptEncode(quote) + quote).ToArray());
        }

        public static DateTime FuzzyConvertToDateTime(this string s)
        {
            DateTime dateTime;
            DateTime.TryParseExact(
                s,
                new string[] { "d", "D", "f", "F", "g", "G", "m", "r", "s", "t", "T", "u", "U", "y", "ddd MMM dd HH:mm:ss zzzz yyyy" },
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.AdjustToUniversal,
                out dateTime);
            return dateTime;
        }
    }
}
