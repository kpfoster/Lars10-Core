using System.Collections.Generic;

namespace Lars10.ZipMgmt
{
    internal class CanadaRanges : List<ZipRange>
    {
        public CanadaRanges()
        {
            Add(new ZipRange("AB", "T0A0A0", "T9Z9Z9"));   // Alberta
            Add(new ZipRange("BC", "V0A0A0", "V9Z9Z9"));   // British Colombia
            Add(new ZipRange("MB", "R0A0A0", "R9Z9Z9"));   // Manitoba
            Add(new ZipRange("SK", "S0A0A0", "S9Z9Z9"));   // Saskatchewan
            Add(new ZipRange("NB", "E0A0A0", "E9Z9Z9"));   // New Brunswick
            Add(new ZipRange("ON", "K0A0A0", "K9Z9Z9"));   // Ontario
            Add(new ZipRange("ON", "L0A0A0", "L9Z9Z9"));   // Ontario
            Add(new ZipRange("ON", "M0A0A0", "M9Z9Z9"));   // Ontario
            Add(new ZipRange("ON", "N0A0A0", "N9Z9Z9"));   // Ontario
            Add(new ZipRange("ON", "P0A0A0", "P9Z9Z9"));   // Ontario

            Add(new ZipRange("PQ", "G0A0A0", "G9Z9Z9"));   // Quebec
            Add(new ZipRange("PQ", "H0A0A0", "H9Z9Z9"));   // Quebec
            Add(new ZipRange("PQ", "J0A0A0", "J9Z9Z9"));   // Quebec

            Add(new ZipRange("QC", "G0A0A0", "G9Z9Z9"));   // Quebec
            Add(new ZipRange("QC", "H0A0A0", "H9Z9Z9"));   // Quebec
            Add(new ZipRange("QC", "J0A0A0", "J9Z9Z9"));   // Quebec
            Add(new ZipRange("NS", "B0A0A0", "B9Z9Z9"));   // Nova Scotia
            Add(new ZipRange("NF", "A0A0A0", "A9Z9Z9"));   // Newfoundland

            Add(new ZipRange("NL", "A0A0A0", "A9Z9Z9"));   // Newfoundland (Secondary Abbreviation) aka "Newfoundland and Labrador"

            Add(new ZipRange("NT", "X0A0A0", "X9Z9Z9"));   // Northwest Territories (per Wikipedia)
            Add(new ZipRange("YT", "Y0A0A0", "Y9Z9Z9"));   // Yukon (per Wikipedia)
            Add(new ZipRange("PE", "C0A0A0", "C9Z9Z9"));   // Price Edward Island
        }
    }
}
