using System;
using System.Collections.Generic;
using System.Linq;

namespace Lars10.ZipMgmt
{
    internal static class ZipRangeExtensions
    {
        public static List<ZipRange> RemoveRanges(this ZipRange singleRange, List<ZipRange> rangesToRemove)
        {
            rangesToRemove.Sort(delegate (ZipRange a, ZipRange b)
            {
                var result = a.Lower.CompareTo(b.Lower);

                return result == 0 ? a.Upper.CompareTo(b.Upper) : result;
            });

            var rangesRemaining = new List<ZipRange>();

            for (var i = 0; i < rangesToRemove.Count - 1; i++)
            {
                var range = rangesToRemove[i];
                var nextRange = rangesToRemove[i + 1];

                // ReSharper disable once StringCompareIsCultureSpecific.1
                if (string.Compare(range.Upper, nextRange.Lower) > 0)
                {
                    throw new ArgumentException($"RangesToRemove list should be in zip order: {range.Lower}-{range.Upper}");
                }
            }

            var singleRangeAfterRemove = new List<ZipRange> { singleRange };

            for (var i = 0; i < rangesToRemove.Count; i++)
            {
                var rangeToRemove = rangesToRemove[i];

                // all ranges prior to last one have either been removed or placed in remainder list
                if (singleRangeAfterRemove.Count > 0)
                {
                    var lastSourceRange = singleRangeAfterRemove[singleRangeAfterRemove.Count - 1];

                    // remove range, and get list of rangesAfterRemove
                    singleRangeAfterRemove = lastSourceRange.Remove(rangeToRemove).ToList();

                    // if a single range remains
                    if (singleRangeAfterRemove.Count == 1)
                    {
                        var lastRangeToCheck = singleRangeAfterRemove[0];

                        // ReSharper disable once StringCompareIsCultureSpecific.1
                        if (string.Compare(lastRangeToCheck.Upper, lastSourceRange.Upper) >= 0)
                        {
                            if (i == rangesToRemove.Count - 1)
                            {
                                rangesRemaining.Add(lastRangeToCheck);
                            }
                            // no chunk prior
                        }
                        else
                        {
                            rangesRemaining.Add(lastRangeToCheck);
                            break;
                        }
                    }
                    else if (singleRangeAfterRemove.Count > 0)
                    {
                        rangesRemaining.Add(singleRangeAfterRemove[0]);
                    }
                }
                else
                {
                    break;
                }
            }

            if (singleRangeAfterRemove.Count > 1)
            {
                rangesRemaining.Add(singleRangeAfterRemove[1]);
            }

            return rangesRemaining;
        }

        public static bool CanMerge(this ZipRange r, ZipRange range)
        {
            if (range == null || Equals(range, r))
                return false;

            if (r.IsBetweenInclusive(range))
            {
                return true;
            }

            if (Zip.IsLessThan(range.Lower, r.Lower) && r.IsBetweenInclusive(range.Upper))
            {
                return true;
            }

            if (r.IsBetweenInclusive(range.Lower) && Zip.IsGreaterThan(range.Upper, r.Upper))
            {
                return true;
            }

            if (!Zip.IsLast(range.Upper) && Zip.Next(range.Upper) == r.Lower)
            {
                return true;
            }

            return !Zip.IsFirst(range.Upper) && Zip.Previous(range.Lower) == r.Upper;
        }

        public static void Merge(this ZipRange r, ZipRange range)
        {
            if (r.IsBetweenInclusive(range))
            {
                return;
            }

            if (Zip.IsLessThan(range.Lower, r.Lower) && r.IsBetweenInclusive(range.Upper))
            {
                r.Lower = range.Lower;
            }
            else if (r.IsBetweenInclusive(range.Lower) && Zip.IsGreaterThan(range.Upper, r.Upper))
            {
                r.Upper = range.Upper;
            }
            else
            {
                if (!Zip.IsLast(range.Upper) && Zip.Next(range.Upper) == r.Lower)
                {
                    r.Lower = range.Lower;
                }
                else if (!Zip.IsFirst(range.Lower) && Zip.Previous(range.Lower) == r.Upper)
                {
                    r.Upper = range.Upper;
                }
                else
                {
                    throw new InvalidOperationException("Merge scenario not supported");
                }
            }
        }

        private static IEnumerable<ZipRange> Remove(this ZipRange r, ZipRange rangeToRemove)
        {
            if (Zip.IsLessThan(rangeToRemove.Lower, r.Lower) && Zip.IsGreaterThan(rangeToRemove.Upper, r.Upper))
            {
                // Range completely overlaps the current range
                return new ZipRange[] { };
            }

            if (r.IsBetweenInclusive(rangeToRemove))
            {
                // Range is completely within the current range
                return new[]
                {
                    new ZipRange(r.Lower, Zip.Previous(rangeToRemove.Lower)),
                    new ZipRange(Zip.Next(rangeToRemove.Upper), r.Upper)
                };
            }

            if (IsBetweenExclusive(rangeToRemove.Lower, r.Lower, r.Upper))
            {
                return new[]
                {
                    new ZipRange(r.Lower, Zip.Previous(rangeToRemove.Lower))
                };
            }

            if (IsBetweenExclusive(rangeToRemove.Upper, r.Lower, r.Upper))
            {
                return new[]
                {
                    new ZipRange(Zip.Next(rangeToRemove.Upper), r.Upper)
                };
            }

            if (rangeToRemove.Upper == r.Upper)
            {
                if (Zip.IsLessThanOrEqualTo(Zip.Next(rangeToRemove.Upper), r.Upper))
                {
                    return new[]
                    {
                        new ZipRange(Zip.Next(rangeToRemove.Upper), r.Upper)
                    };
                }

                return new ZipRange[] { };
            }

            if (rangeToRemove.Lower != r.Upper) return new[] { r };

            if (Zip.IsLessThanOrEqualTo(r.Lower, Zip.Previous(rangeToRemove.Lower)))
            {
                return new[]
                {
                    new ZipRange(r.Lower, Zip.Previous(rangeToRemove.Lower)),
                };
            }

            return new ZipRange[] { };
        }

        private static bool IsBetweenInclusive(this ZipRange r, string zip)
        {
            return IsBetweenInclusive(zip, r.Lower, r.Upper);
        }

        private static bool IsBetweenInclusive(this ZipRange r, ZipRange range)
        {
            return IsBetweenExclusive(range.Lower, r.Lower, r.Upper)
                && IsBetweenExclusive(range.Upper, r.Lower, r.Upper);
        }

        private static bool IsBetweenExclusive(string zip, string lower, string upper)
        {
            return Zip.IsGreaterThan(zip, lower) && Zip.IsLessThan(zip, upper);
        }

        private static bool IsBetweenInclusive(string zip, string lower, string upper)
        {
            return Zip.IsGreaterThanOrEqualTo(zip, lower) && Zip.IsLessThanOrEqualTo(zip, upper);
        }
    }
}
