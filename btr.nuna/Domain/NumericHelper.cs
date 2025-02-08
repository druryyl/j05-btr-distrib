using System;

namespace btr.nuna.Domain
{
    public static class NumericHelper
    {
        public static string Eja(this decimal bilangan)
        {
            var result = "";
            var bilString = bilangan.ToString().Trim();
            var dummy = "";
            var hundredCounter = 0;
            var result1 = "";

            bool isFirst = true;

            for (int i = bilString.Length - 1; i >= 0; i--)
            {
                var c = bilString[i];
                dummy = c + dummy;
                if (dummy.Length == 3)
                {
                    result1 = ConvertHundred(dummy, isFirst);
                    if (result1 == "satu") result1 = "se";
                    if (hundredCounter == 1) result1 += " ribu ";
                    if (hundredCounter == 2) result1 += " juta ";
                    if (hundredCounter == 3) result1 += " milyar ";
                    if (hundredCounter == 4) result1 += " trilyun ";
                    hundredCounter++;
                    dummy = "";
                    result = result1 + result;
                    isFirst = false;
                }
            }
            if (dummy != "")
            {
                result1 = ConvertHundred(dummy, isFirst);
                if (result1 == "satu") result1 = "se";
                if (hundredCounter == 1) result1 += " ribu ";
                if (hundredCounter == 2) result1 += " juta ";
                if (hundredCounter == 3) result1 += " milyar ";
                if (hundredCounter == 4) result1 += " trilyun ";

                if (result1 == "se juta ") result1 = " satu juta ";
                result = result1 + result;
            }
            return result;
        }

        private static string ConvertHundred(string bilangan, bool isFirst)
        {
            string[] eja = { "", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan" };

            var sBilangan = bilangan.PadLeft(3, '0');

            var iBilangan0 = Convert.ToInt16(sBilangan[0].ToString().Trim());
            var iBilangan1 = Convert.ToInt16(sBilangan[1].ToString().Trim());
            var iBilangan2 = Convert.ToInt16(sBilangan[2].ToString().Trim());

            var angkaRatusan = eja[iBilangan0];
            var angkaPuluhan = eja[iBilangan1];
            var angkaSatuan = eja[iBilangan2];

            if (angkaRatusan == "satu") angkaRatusan = "se";

            var isBelas = false;
            if (angkaPuluhan == "satu")
            {
                isBelas = true;
                if (angkaSatuan == "satu") angkaPuluhan = "se";
                else angkaPuluhan = angkaSatuan;
                angkaSatuan = "belas";
            }

            var result = "";
            if (angkaRatusan != "")
            {
                if (angkaRatusan == "se")
                    result = angkaRatusan + "ratus ";
                else
                    result = angkaRatusan + " ratus ";
            }

            if (angkaPuluhan != "")
                if (!isBelas)
                    if (angkaPuluhan == "se")
                        result += angkaPuluhan + "puluh ";
                    else
                        result += angkaPuluhan + " puluh ";
                else 
                    result += angkaPuluhan;

            result += angkaSatuan;

            if ((result == "satu") && (!isFirst)) return "se";

            return result;
        }
    }
}
