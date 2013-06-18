using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Globalization;

namespace BotDetect
{
    /// <summary>
    /// A helper class used for common string operations used by BotDetect code.
    /// </summary>
    public class StringHelper
    {
        private StringHelper()
        {
            // constructor omitted & forbidden, static methods only
        }

        /// <summary>
        /// Checks that the suppliedstring is not null, empty, or consists 
        /// of only whitespace characters
        /// </summary>
        public static bool HasValue(string input)
        {
            if (null == input)
            {
                return false;
            }

            if (0 == input.Length)
            {
                return false;
            }

            if (0 == input.Trim().Length)
            {
                return false;
            }

            return true;
        }

        // unicode codepoints from chars
        internal static Set<Int32> GetCodePoints(string csvChars)
        {
            return GetCodePoints(ParseCsv(csvChars));
        }

        /// <summary>
        /// Converts the given collection of characters into Unicode code points
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static Set<Int32> GetCodePoints(StringCollection chars)
        {
            Set<Int32> charSet = new Set<Int32>();
            if (null == chars || 0 == chars.Count)
            {
                return charSet;
            }

            foreach (string item in chars)
            {
                Int32 codePoint = Char.ConvertToUtf32(item, 0);
                charSet.Add(codePoint);
            }

            return charSet;
        }

        /// <summary>
        /// Splits the supplied comma-separated input into a character collection
        /// </summary>
        /// <param name="csvInput"></param>
        /// <returns></returns>
        public static StringCollection ParseCsv(string csvInput)
        {
            if (!StringHelper.HasValue(csvInput))
            {
                return null;
            }

            StringCollection collection = new StringCollection(); 
            collection.AddRange(CsvToArray(csvInput));
            return collection;
        }

        /// <summary>
        /// Splits the supplied comma-separated input into a string array
        /// </summary>
        /// <param name="csvInput"></param>
        /// <returns></returns>
        public static string[] CsvToArray(string csvInput)
        {
            string[] items = csvInput.Split(',');

            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];
                item = item.Trim();
                items[i] = item;
            }

            return items;
        }

        /// <summary>
        /// Writes the given input to a log-friendly string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToString(object input)
        {
            if (null == input)
            {
                return "null";
            }

            return input.ToString();
        }

        /// <summary>
        /// Writes the given input to a log-friendly string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToString(DateTime input)
        {
            if (DateTime.MinValue == input)
            {
                return "null";
            }

            return input.ToString("yyyy-MM-dd HH:mm:ss,fffZ", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Turns null or empty strings into log-friendly representations
        /// </summary>
        public static string LogFriendly(string input)
        {
            if (null == input)
            {
                return "null";
            }

            if (0 == input.Length)
            {
               return "empty string";
            }

            if (0 == input.Trim().Length)
            {
                return "empty (blank) string";
            }

            return input;
        }

        /// <summary>
        /// Get a log-friendly representation of the params
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string LogFriendly(params object[] values)
        {
            string[] stringValues = GetStringArray(values);
            return LogFriendly(stringValues);
        }

        public static string[] GetStringArray(params object[] values)
        {
            ArrayList stringValues = new ArrayList();
            foreach (object value in values)
            {
                string text = value as string;
                if (null != text)
                {
                    stringValues.Add(text);
                }
                else
                {
                    stringValues.Add(StringHelper.ToString(value));
                }
            }

            string[] stringArray = new string[stringValues.Count];
            stringValues.CopyTo(0, stringArray, 0, stringValues.Count);
            return stringArray;
        }

        /// <summary>
        /// Get a log-friendly representation of the params
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string LogFriendly(params string[] values)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            StringHelper.LogFriendlyWrite(sb, values);
            return sb.ToString();
        }

        /// <summary>
        /// Writes params to StringBuilder, comma-separated
        /// </summary>
        public static StringBuilder LogFriendlyWrite(StringBuilder sb, params string[] values)
        {
            if (null == sb)
            {
                throw new TextException("Invalid string builder sb", sb);
            }

            if ((null == values) || (0 == values.Length))
            {
                throw new TextException("Invalid values", values);
            }

            // write all values, comma-separated
            int end = values.Length - 1;
            for (int i = 0; i < end; i++)
            {
                sb.Append(LogFriendly(values[i]));
                sb.AppendLine();
            }
            sb.Append(LogFriendly(values[end]));

            return sb;
        }
    }
}
