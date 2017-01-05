using System.Collections.Generic;

namespace Lars10.ZipMgmt
{
    public static class ZipCondenser
    {
        public static List<ZipRange> Condense(List<ZipRange> ranges)
        {
            while (true)
            {
                ranges.Sort(delegate (ZipRange a, ZipRange b)
                {
                    var result = a.Lower.CompareTo(b.Lower);

                    return result == 0 ? a.Upper.CompareTo(b.Upper) : result;
                });

                ZipRange source = null, target = null;

                foreach (var outer in ranges)
                {
                    foreach (var inner in ranges)
                    {
                        if (outer == inner) continue;
                        if (!inner.CanMerge(outer)) continue;

                        source = outer;
                        target = inner;
                        break;
                    }

                    if (source != null && target != null)
                        break;
                }

                if (source != null && target != null)
                {
                    target.Merge(source);

                    ranges.Remove(source);

                    continue;
                }
                break;
            }

            return ranges;
        }

        public static List<string> Condense(List<string> singleZipCodes)
        {
            singleZipCodes.Sort();

            var results = new List<string>();

            string low = null;
            string high = null;

            var i = 0;

            var done = !(singleZipCodes.Count > 0);

            while (!done)
            {
                var current = singleZipCodes[i];

                if (low == null)
                {
                    low = current;
                    high = current;
                }
                else
                {
                    if (Zip.Next(high) == current)
                    {
                        high = current;
                    }
                    else
                    {
                        results.Add(low == high ? low : $"{low}-{high}");

                        low = current;
                        high = current;
                    }
                }
                if (i >= singleZipCodes.Count - 1)
                {
                    results.Add(low == high ? low : $"{low}-{high}");
                    done = true;
                }
                else
                {
                    i++;
                }
            }

            return results;
        }
    }
}
