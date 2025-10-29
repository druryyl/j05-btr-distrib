using System;
using System.Collections.Generic;

namespace btr.nuna.Domain
{
    public static class NumericHelper
    {
        public static string Eja(this decimal bilangan)
        {
            if (bilangan == 0) return "nol";

            var result = "";
            var bilString = Math.Floor(bilangan).ToString(); // Bagian bulat saja
            var length = bilString.Length;

            // Kelompokkan angka per 3 digit dari kanan
            var groups = new List<string>();
            for (int i = length; i > 0; i -= 3)
            {
                int start = Math.Max(0, i - 3);
                int count = Math.Min(3, i - start);
                groups.Insert(0, bilString.Substring(start, count));
            }

            // Process each group
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var groupValue = int.Parse(group);

                if (groupValue == 0) continue;

                var groupText = ConvertHundred(group, i == groups.Count - 1);
                var multiplier = GetMultiplier(groups.Count - i - 1);

                result += groupText + multiplier + " ";
            }

            return result.Trim(); // + " rupiah";
        }

        private static string ConvertHundred(string bilangan, bool isLastGroup)
        {
            string[] eja = { "", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan" };

            var sBilangan = bilangan.PadLeft(3, '0');
            var iBilangan0 = int.Parse(sBilangan[0].ToString());
            var iBilangan1 = int.Parse(sBilangan[1].ToString());
            var iBilangan2 = int.Parse(sBilangan[2].ToString());

            var result = "";

            // Ratusan
            if (iBilangan0 > 0)
            {
                if (iBilangan0 == 1)
                    result += "seratus ";
                else
                    result += eja[iBilangan0] + " ratus ";
            }

            // Puluhan dan Satuan
            if (iBilangan1 > 0)
            {
                if (iBilangan1 == 1)
                {
                    if (iBilangan2 == 0)
                        result += "sepuluh";
                    else if (iBilangan2 == 1)
                        result += "sebelas";
                    else
                        result += eja[iBilangan2] + " belas";
                }
                else
                {
                    result += eja[iBilangan1] + " puluh";
                    if (iBilangan2 > 0)
                        result += " " + eja[iBilangan2];
                }
            }
            else
            {
                // Hanya satuan
                if (iBilangan2 > 0)
                {
                    // Khusus untuk angka 1 di posisi terakhir kelompok pertama
                    //if (iBilangan2 == 1 && isLastGroup && bilangan.Length == 1)
                    //    result += "satu";
                    //else if (iBilangan2 == 1)
                    //    result += "se";
                    if (iBilangan2 == 1)
                        result += "satu";
                    else
                        result += eja[iBilangan2];
                }
            }

            return result.Trim();
        }

        private static string GetMultiplier(int level)
        {
            switch(level)
            {
                case 1:  return " ribu";
                case 2:  return " juta";
                case 3:
                    return " milyar";
                case 4:
                    return " trilyun";
            };
            return string.Empty;
        }
    }
}
