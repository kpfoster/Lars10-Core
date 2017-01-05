using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lars10.ZipMgmt
{
    public static class ZipParser
    {
        public static ZipRange CreateZipRange(string zip)
        {
            if (zip.IndexOf('-') >= 0)
            {
                var parts = zip.Split('-');

                var lower = parts[0].Trim();
                var upper = parts[1].Trim();

                if (lower.Length == 3)
                {
                    if (Regex.IsMatch(lower, @"^\d"))
                    {
                        lower += "00";
                        upper += "99";
                    }
                    else if (Regex.IsMatch(lower, @"^([A-Z])"))
                    {
                        lower += "0A0";
                        upper += "9Z9";
                    }
                }
                else
                {
                    lower = Zip.NormalizeLowerZip(lower);
                    upper = Zip.NormalizeUpperZip(lower, upper);
                }

                if (!Zip.IsValid(lower))
                    throw new InvalidOperationException("Invalid Zip (lower): " + lower);

                if (!Zip.IsValid(upper))
                    throw new InvalidOperationException("Invalid Zip (upper): " + upper);

                return new ZipRange(lower, upper);
            }

            if (zip.Length == 3)
            {
                var lower = zip;
                var upper = zip;

                if (Regex.IsMatch(lower, @"^\d"))
                {
                    lower += "00";
                    upper += "99";
                }
                else if (Regex.IsMatch(lower, @"^([A-Z])"))
                {
                    lower += "0A0";
                    upper += "9Z9";
                }

                if (!Zip.IsValid(lower))
                    throw new InvalidOperationException("Invalid Zip (lower): " + lower);

                if (!Zip.IsValid(upper))
                    throw new InvalidOperationException("Invalid Zip (upper): " + upper);

                return new ZipRange(lower, upper);
            }

            if (!Zip.IsValid(zip))
                throw new InvalidOperationException($"Invalid Zip: {zip}");

            return new ZipRange(zip, zip);
        }

        public static string CreateRangeAsString(string zip)
        {
            if (zip.IndexOf('-') >= 0)
            {
                var parts = zip.Split('-');

                var lower = parts[0].Trim();
                var upper = parts[1].Trim();

                if (lower.Length == 3)
                {
                    if (Regex.IsMatch(lower, @"^\d"))
                    {
                        lower += "00";
                        upper += "99";
                    }
                    else if (Regex.IsMatch(lower, @"^([A-Z])"))
                    {
                        lower += "0A0";
                        upper += "9Z9";
                    }
                }
                else
                {
                    lower = Zip.NormalizeLowerZip(lower);
                    upper = Zip.NormalizeUpperZip(lower, upper);
                }

                if (!Zip.IsValid(lower))
                    throw new InvalidOperationException("Invalid Zip (lower): " + lower);

                if (!Zip.IsValid(upper))
                    throw new InvalidOperationException("Invalid Zip (upper): " + upper);

                return $"{lower}-{upper}";
            }

            if (zip.Length == 3)
            {
                var lower = zip;
                var upper = zip;

                if (Regex.IsMatch(lower, @"^\d"))
                {
                    lower += "00";
                    upper += "99";
                }
                else if (Regex.IsMatch(lower, @"^([A-Z])"))
                {
                    lower += "0A0";
                    upper += "9Z9";
                }

                if (!Zip.IsValid(lower))
                    throw new InvalidOperationException("Invalid Zip (lower): " + lower);

                if (!Zip.IsValid(upper))
                    throw new InvalidOperationException("Invalid Zip (upper): " + upper);

                return $"{lower}-{upper}";
            }

            if (!Zip.IsValid(zip))
                throw new InvalidOperationException($"Invalid Zip: {zip}");

            return zip;
        }

        public static IEnumerable<ZipRange> CreateMultipleZipRanges(string zips)
        {
            var results = new List<ZipRange>();

            foreach (var zip in zips.Split(','))
            {
                var trimmed = zip.Trim();

                if (trimmed.Length <= 0) continue;

                if (trimmed.IndexOf('-') >= 0)
                {
                    var parts = trimmed.Split('-');

                    var lower = parts[0].Trim();
                    var upper = parts[1].Trim();

                    if (Zip.IsValid(lower) && Zip.IsValid(upper))
                    {
                        results.Add(new ZipRange(lower, upper));
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid Zip Range: " + trimmed);
                    }
                }
                else
                {
                    if (Zip.IsValid(trimmed))
                    {
                        results.Add(new ZipRange(trimmed, trimmed));
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid Zip: " + trimmed);
                    }
                }
            }

            return results;
        }

        public static IEnumerable<ZipRange> CreateMultipleZipRanges(IEnumerable<string> ranges)
        {
            return (from range in ranges
                    select range.Trim() into trimmed
                    where trimmed.Length > 0
                    select CreateZipRange(trimmed)).ToList();
        }

        public static IEnumerable<ZipRange> CreateMultipleZipRangesAndCondense(IEnumerable<string> ranges)
        {
            var r = CreateMultipleZipRanges(ranges).ToList();
            return ZipCondenser.Condense(r);
        }
    }
}
