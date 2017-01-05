using System.Collections.Generic;
using System.Linq;

namespace Lars10.ZipMgmt
{
    public class ZoneDifference
    {
        public ZoneDifference(IEnumerable<ZipRange> ranges, IEnumerable<ZipRange> rangesToExclude)
        {
            Region = ranges.ToList();
            RegionToExclude = rangesToExclude.ToList();
        }

        public ZoneDifference(string range, string zipRangesToExclude, bool rangeIsListOfRanges = false)
        {
            Region = rangeIsListOfRanges ?
                ZipParser.CreateMultipleZipRanges(range).ToList() :
                Zip.FindAll(range).ToList();

            RegionToExclude = ZipParser.CreateMultipleZipRanges(zipRangesToExclude).ToList();
        }

        public List<ZipRange> Region { get; }

        public List<ZipRange> RegionToExclude { get; }

        public List<ZipRange> Difference => GetDifference();

        private List<ZipRange> GetDifference()
        {
            var result = new List<ZipRange>();

            foreach (var range in Region)
            {
                var rangeRemainder = range.RemoveRanges(RegionToExclude);
                result.AddRange(rangeRemainder);
            }

            return result;
        }
    }
}
