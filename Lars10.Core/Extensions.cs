using System;

namespace Lars10.Core
{
    public static class Extensions
    {
        public static long ToEpoch(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1);

            if (date < epoch)
                return long.MinValue;

            var epochTimeSpan = date - epoch;
            return (long)epochTimeSpan.TotalSeconds;
        }
    }
}
