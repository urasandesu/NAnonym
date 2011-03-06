/* 
 * File: StringUtility.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Urasandesu.NAnonym
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

        public static string Dump<TSource>(this IEnumerable<TSource> source)
        {
            return CsvEncode(source.Select(item => string.Format("{0}", item)));
        }

        public static string CsvEncode(this string s)
        {
            return CsvEncode(s, Csv.Default.Quote);
        }

        public static string CsvEncode(this string s, string quote)
        {
            if (quote == null) throw new ArgumentNullException("quote");
            return s.Replace(quote, quote + quote);
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

        public static string ToLocalPath(this string uriString)
        {
            return new Uri(uriString).LocalPath;
        }

        public static string WithoutExtension(this string fileFullPathOrUri)
        {
            int extensionStartIndex = 0;
            if (-1 < (extensionStartIndex = fileFullPathOrUri.LastIndexOf('.')))
            {
                return fileFullPathOrUri.Substring(0, extensionStartIndex);
            }
            else
            {
                return fileFullPathOrUri;
            }
        }

        public static string ToCamel(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : s.Substring(0, 1).ToLower() + (s.Length < 2 ? string.Empty : s.Substring(1));
        }
    }
}

