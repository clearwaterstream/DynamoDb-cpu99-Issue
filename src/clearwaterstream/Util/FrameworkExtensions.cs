using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class SystemExtensions
    {
        public static bool IsEither(this string str, params string[] values)
        {
            return IsEither(str, values.AsEnumerable());
        }

        public static bool IsEither(this string str, IEnumerable<string> values)
        {
            if (str == null && values == null)
                return true;

            bool result = false;

            foreach (string val in values)
            {
                if (str.Eq(val))
                {
                    result = true;

                    break;  // BREAK !!!
                }
            }

            return result;
        }

        public static bool Eq(this string str, string otherStr)
        {
            if (str == null)
            {
                return otherStr == null;
            }
            else
            {
                return str.Equals(otherStr, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return new TimeSpan(0, 0, 0, 0, milliseconds);
        }

        public static TimeSpan Seconds(this int seconds)
        {
            return new TimeSpan(0, 0, seconds);
        }

        public static TimeSpan Minutes(this int minutes)
        {
            return new TimeSpan(0, minutes, 0);
        }

        public static TimeSpan Hours(this int hours)
        {
            return new TimeSpan(hours, 0, 0);
        }

        public static TimeSpan Days(this int days)
        {
            return new TimeSpan(days, 0, 0, 0);
        }

        public static string Find(this string str, string charsToFind)
        {
            string result = null;

            var match = Regex.Match(str, charsToFind);

            if (match.Success)
            {
                result = string.Join(null, match.Value.Distinct());
            }

            return result;
        }

        public static string FindExtraneous(this string str, IEnumerable<char> validChars)
        {
            string result = null;

            var extraneous = str.Except(validChars).Distinct();

            if (extraneous.Any())
            {
                result = string.Join(null, extraneous);
            }

            return result;
        }

        public static string RemoveExtraneous(this string str, IEnumerable<char> charsToRemove)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (charsToRemove == null || !charsToRemove.Any())
                return str;

            var withCharsRemoved = str.ToCharArray().Where(c => !charsToRemove.Contains(c)).ToArray();

            var result = new string(withCharsRemoved);

            return result;
        }

        public static string DigitsOnly(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var result = string.Join(null, str.ToCharArray().Where(c => char.IsDigit(c)));

            return result;
        }

        public static int[] GetDigits(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return new int[] { };

            return str.ToCharArray().Where(x => char.IsDigit(x)).Select(x => CharUnicodeInfo.GetDigitValue(x)).ToArray();
        }

        public static string GetDigitsOr(this string str, char or)
        {
            if (string.IsNullOrEmpty(str))
                return String.Empty;

            string result = "";
            char[] strArray = str.ToArray();

            foreach (var chr in strArray)
            {
                if (chr.Equals(or))
                    result += chr;
                if (Char.IsDigit(chr))
                    result += chr;
            }

            return result;
        }

        public static decimal k(this int num)
        {
            return num * 1000m;
        }

        public static bool IsNullOrZero(this decimal? val)
        {
            if (!val.HasValue)
                return false;

            return val.Value == decimal.Zero;
        }

        public static string Capitalize(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var arr = text.ToCharArray();

            arr[0] = char.ToUpper(arr[0]);

            return new string(arr);
        }

        public static bool ContainsAll(this string str, params string[] values)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            if (values == null || values.Length == 0)
                return false;

            var result = values.All(str.Contains);

            return result;
        }
    }
}

namespace System.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        public static string Environment(this NameValueCollection appSettings)
        {
            var env = appSettings["environment"];

            return env?.ToLowerInvariant();
        }

        public static string EnvironmentName(this NameValueCollection appSettings)
        {
            var envName = appSettings["environment_name"];

            return envName?.ToLowerInvariant();
        }
    }
}
