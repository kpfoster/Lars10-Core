using System.Collections.Generic;

namespace Lars10.ZipMgmt
{
    public static class States
    {
        #region constructor
        static States()
        {
            StateLookup.Add("AL");
            StateLookup.Add("AK");
            StateLookup.Add("AZ");
            StateLookup.Add("AR");
            StateLookup.Add("CA");
            StateLookup.Add("CO");
            StateLookup.Add("CT");
            StateLookup.Add("DE");
            StateLookup.Add("FL");
            StateLookup.Add("GA");
            StateLookup.Add("HI");
            StateLookup.Add("ID");
            StateLookup.Add("IL");
            StateLookup.Add("IN");
            StateLookup.Add("IA");
            StateLookup.Add("KS");
            StateLookup.Add("KY");
            StateLookup.Add("LA");
            StateLookup.Add("ME");
            StateLookup.Add("MD");
            StateLookup.Add("MA");
            StateLookup.Add("MI");
            StateLookup.Add("MN");
            StateLookup.Add("MS");
            StateLookup.Add("MO");
            StateLookup.Add("MT");
            StateLookup.Add("NE");
            StateLookup.Add("NV");
            StateLookup.Add("NH");
            StateLookup.Add("NJ");
            StateLookup.Add("NM");
            StateLookup.Add("NY");
            StateLookup.Add("NC");
            StateLookup.Add("ND");
            StateLookup.Add("OH");
            StateLookup.Add("OK");
            StateLookup.Add("OR");
            StateLookup.Add("PA");
            StateLookup.Add("RI");
            StateLookup.Add("SC");
            StateLookup.Add("SD");
            StateLookup.Add("TN");
            StateLookup.Add("TX");
            StateLookup.Add("UT");
            StateLookup.Add("VT");
            StateLookup.Add("VA");
            StateLookup.Add("WA");
            StateLookup.Add("WV");
            StateLookup.Add("WI");
            StateLookup.Add("WY");
            StateLookup.Add("DC");
            StateLookup.Add("AA");
            StateLookup.Add("AE");
            StateLookup.Add("AP");
            StateLookup.Add("AB");
            StateLookup.Add("BC");
            StateLookup.Add("MB");
            StateLookup.Add("NB");
            StateLookup.Add("NL");
            StateLookup.Add("NS");
            StateLookup.Add("ON");
            StateLookup.Add("PE");
            StateLookup.Add("QC");
            StateLookup.Add("SK");
            StateLookup.Add("NT");
            StateLookup.Add("NU");
            StateLookup.Add("YT");
            StateLookup.Add("PQ");
            StateLookup.Add("NF");
        }
        #endregion

        public static bool Contains(string abbreviation)
        {
            return StateLookup.Contains(abbreviation);
        }

        private static readonly HashSet<string> StateLookup = new HashSet<string>();
    }
}