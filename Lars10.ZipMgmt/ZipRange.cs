using System;

namespace Lars10.ZipMgmt
{
    [Serializable]
    public sealed class ZipRange
    {
        public ZipRange(string name, string lower, string upper) : this(lower, upper)
        {
            Name = name;
        }

        public ZipRange(string lower, string upper)
        {
            if (string.IsNullOrWhiteSpace(lower))
                throw new ArgumentException(lower);

            if (string.IsNullOrWhiteSpace(upper))
                throw new ArgumentException(upper);

            lower = lower.Trim();
            upper = upper.Trim();

            if (lower.Length == 3)
                lower = lower + "00";

            if (upper.Length == 3)
                upper = upper + "99";

            if (!Zip.IsValid(lower))
                throw new ArgumentException("Invalid zip code", nameof(lower));

            if (!Zip.IsValid(upper))
                throw new ArgumentException("Invalid zip code:", nameof(upper));

            if (Zip.IsUnitedStates(lower) && Zip.IsCanada(upper))
                throw new InvalidOperationException("US and Canada zips are not allowed to be mixed within the same range");

            if (Zip.IsCanada(lower) && Zip.IsUnitedStates(upper))
                throw new InvalidOperationException("US and Canada zips are not allowed to be mixed within the same range");

            if (Zip.IsGreaterThan(lower, upper))
                throw new InvalidOperationException("Lower must be less than or equal to upper: " + lower + "-" + upper);

            Lower = lower;
            Upper = upper;
        }

        public string Name { get; }

        public string Lower
        {
            get { return _lower; }
            internal set
            {
                var lowerZip = Zip.NormalizeLowerZip(value);

                if (!Zip.IsValid(lowerZip))
                    throw new ArgumentNullException($"Invalid lower zip code: {value}");

                _lower = lowerZip;
            }
        }

        public string Upper
        {
            get { return _upper; }
            internal set
            {
                var upperZip = Zip.NormalizeUpperZip(_lower, value);

                if (!Zip.IsValid(upperZip))
                    throw new ArgumentNullException($"Invalid upper zip code: {value}");

                _upper = upperZip;
            }
        }

        public override string ToString()
        {
            return $"{Lower}-{Upper}";
        }

        public override int GetHashCode()
        {
            return (Lower + Upper).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var range = obj as ZipRange;

            if (range != null)
            {
                return range.Lower == Lower && range.Upper == Upper;
            }

            var stringValue = obj as string;

            var parts = stringValue?.Split('-');

            if (parts?.Length == 2)
            {
                return Lower == parts[0].Trim() && Upper == parts[1].Trim();
            }

            return false;
        }

        private string _lower;
        private string _upper;
    }
}
