using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
// ReSharper disable StringCompareToIsCultureSpecific

namespace Lars10.ZipMgmt
{
    public static class Zip
    {
        public static IEnumerable<ZipRange> FindAll(string name)
        {
            var usa = new UsRanges().FindAll(f => string.Compare(f.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
            var canada = new CanadaRanges().FindAll(f => string.Compare(f.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

            return usa.Any() ?
                DeepCopy(usa) :
                DeepCopy(canada);
        }

        public static bool IsFirst(string zip)
        {
            return zip == "A0A0A0"
                || zip == "00000";
        }

        public static bool IsLast(string zip)
        {
            return zip == "Z9Z9Z9"
                || zip == "99999";
        }

        public static bool IsGreaterThan(string left, string right)
        {
            return left.CompareTo(right) == 1;
        }

        public static bool IsGreaterThanOrEqualTo(string left, string right)
        {
            var compare = left.CompareTo(right);
            return compare == 0 || compare == 1;
        }

        public static bool IsLessThan(string left, string right)
        {
            return left.CompareTo(right) == -1;
        }

        public static bool IsLessThanOrEqualTo(string left, string right)
        {
            var compare = left.CompareTo(right);
            return compare == 0 || compare == -1;
        }

        public static bool IsUnitedStates(string zip)
        {
            return IsValid(zip) && Regex.IsMatch(zip, "^[0-9]{5}$");
        }

        public static bool IsCanada(string zip)
        {
            return IsValid(zip) && Regex.IsMatch(zip, "^([A-Z]{1}[0-9]{1}){3}$");
        }

        public static bool IsValid(string zip)
        {
            if (zip == null)
                return false;

            switch (zip.Length)
            {
                case 5:
                    return Regex.IsMatch(zip, "^[0-9]{5}$");
                case 6:
                    return Regex.IsMatch(zip, "^([A-Z]{1}[0-9]{1}){3}$");
                default:
                    return false;
            }
        }

        public static string Next(string zip)
        {
            if (!IsValid(zip))
                throw new ArgumentException("Invalid zip", nameof(zip));

            var chars = zip.ToCharArray();

            for (var i = chars.Length - 1; i >= 0; i--)
            {
                var c = chars[i];

                if (c == '9')
                {
                    chars[i] = '0';
                }
                else if (c == 'Z')
                {
                    chars[i] = 'A';
                }
                else
                {
                    chars[i]++;
                    break;
                }
            }

            return new string(chars);
        }

        public static string Previous(string zip)
        {
            if (!IsValid(zip))
                throw new ArgumentException("Invalid zip", nameof(zip));

            var chars = zip.ToCharArray();

            for (var i = chars.Length - 1; i >= 0; i--)
            {
                var c = chars[i];

                if (c == '0')
                {
                    chars[i] = '9';
                }
                else if (c == 'A')
                {
                    chars[i] = 'Z';
                }
                else
                {
                    chars[i]--;
                    break;
                }
            }

            return new string(chars);
        }

        internal static string NormalizeLowerZip(string lower)
        {
            if (lower.Length != 3) return lower;

            if (Regex.IsMatch(lower, @"^\d"))
            {
                return $"{lower}00";
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (Regex.IsMatch(lower, @"^([A-Z])"))
            {
                return $"{lower}0A0";
            }

            return null;
        }

        internal static string NormalizeUpperZip(string lower, string upper)
        {
            if (upper.Length == 1)
                upper = lower.Substring(0, 4) + upper;

            if (upper.Length == 2)
                upper = lower.Substring(0, 3) + upper;

            if (upper.Length == 3)
                upper = lower.Substring(0, 2) + upper;

            if (upper.Length == 4)
                upper = lower.Substring(0, 1) + upper;

            return upper;
        }

        private static IEnumerable<ZipRange> DeepCopy(IEnumerable<ZipRange> ranges)
        {
            var serializer = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, ranges);
                ms.Position = 0;
                return serializer.Deserialize(ms) as IEnumerable<ZipRange>;
            }
        }
        private static Dictionary<char, int> Letters => new Dictionary<char, int>
        {
            { 'A', 1 },
            { 'B', 2 },
            { 'C', 3 },
            { 'D', 4 },
            { 'E', 5 },
            { 'F', 6 },
            { 'G', 7 },
            { 'H', 8 },
            { 'I', 9 },
            { 'J', 10 },
            { 'K', 11 },
            { 'L', 12 },
            { 'M', 13 },
            { 'N', 14 },
            { 'O', 15 },
            { 'P', 16 },
            { 'Q', 17 },
            { 'R', 18 },
            { 'S', 19 },
            { 'T', 20 },
            { 'U', 21 },
            { 'V', 22 },
            { 'W', 23 },
            { 'X', 24 },
            { 'Y', 25 },
            { 'Z', 26 }
        };

    }
}
