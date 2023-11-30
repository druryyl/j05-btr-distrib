using System;
using System.Globalization;
using System.Numerics;

namespace btr.nuna.Application
{
    public interface INunaCounterBL
    {
        string Generate(string anchor, int length);
        string Generate(string anchor, string prefix, int length);
        string Generate(string prefix, IDFormatEnum format);
        string Generate(string prefix, IDFormatEnum format, string sourceFlag);
    }
    public enum IDFormatEnum
    {
        PF_YYM_nnnnnC, //  format-A1 (13 digit) : PF-21A-00001F => Max 1jt per bulan
        PF_YYM_nnnC,   //  format-A2 (11 digit) : PF-21A-002C   => Max 4k per bulan
        PFnnnn,        // format-B1 ( 6 digit) : PF4321         => Max 65k global
        PFnnn,         //  format-B1 ( 5 digit) : PF321         => Max 4k global
        Pnn,           //  format-B2 ( 3 digit) : P7A           => Max 255 global
        PREFYYMnnnnnC, //  format-A3 (13 digit) : PFFF21A00001F => Max 1jt per bulan 
        PYYMnnnn,       //  format-A4 (8 digit)  : P23A0001      => Max 65k per bulan 
        PYYMnnnnD,       //  format-A5 (8 digit)  : P23A0001      => Max 9999 per bulan (decimal)
        Pn7

    }

    public class NunaCounterBL : INunaCounterBL
    {
        private readonly INunaCounterDal _counterDal;
        public NunaCounterBL(INunaCounterDal counter)
        {
            _counterDal = counter;
        }

        private static string GetBulan(DateTime tgl)
        {
            string result;
            switch (tgl.Month)
            {
                case 10: 
                    result = "A";
                    break;
                case 11:
                    result = "B";
                    break;
                case 12:
                    result = "C";
                    break;
                default:
                    result = tgl.Month.ToString();
                    break;
            }
            return result;
        }

        private static string GetAnchor(string prefix, string periode, IDFormatEnum format)
        {
            string  result;
            switch (format)
            {
                case IDFormatEnum.PF_YYM_nnnnnC:
                    result = $"{prefix}-{periode}";
                    break;
                case IDFormatEnum.PF_YYM_nnnC:
                    result = $"{prefix}-{periode}";
                    break;
                case IDFormatEnum.PFnnnn:
                    result = $"{prefix}";
                    break;
                case IDFormatEnum.PFnnn:
                    result = $"{prefix}";
                    break;
                case IDFormatEnum.Pnn:
                    result = $"{prefix}";
                    break;
                case IDFormatEnum.PREFYYMnnnnnC:
                    result = $"{prefix}{periode}";
                    break;
                case IDFormatEnum.PYYMnnnn:
                    result = $"{prefix}{periode}";
                    break;
                case IDFormatEnum.Pn7:
                    result = $"{prefix}";
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }

        public string Generate(string prefix, IDFormatEnum format)
        {
            var result = GenerateA(prefix, format, "");
            return result;
        }

        public string Generate(string prefix, IDFormatEnum format, string sourceFlag)
        {
            var result = GenerateA(prefix, format, sourceFlag);
            return result;
        }

        public string Generate(string anchor, int length)
        {
            var result = GenerateB(anchor, string.Empty, length);
            return result;
        }

        public string Generate(string anchor, string prefix, int length)
        {
            var result = GenerateB(anchor, prefix, length);
            return result;
        }


        private string GenerateA(string prefix, IDFormatEnum format, string sourceFlag)
        {
            var periode = DateTime.Now.ToString("yy");
            periode += GetBulan(DateTime.Now);
            var anchor = GetAnchor(prefix, periode, format);


            var noUrutHex = _counterDal.GetNewHexNumber(anchor);
            if (noUrutHex is null)
            {
                noUrutHex = "0";
                _counterDal.InsertNewHexNumber(anchor, "0");
            }
            _counterDal.UpdateNewHexNumber(anchor, AddHexa(noUrutHex, "1"));

            var random = new Random();
            var checkDigit = random.Next(0, 9);

            string noUrutBlok;
            string result;
            switch (format)
            {
                case IDFormatEnum.PF_YYM_nnnnnC:
                    noUrutBlok = $"{Gen_5(noUrutHex)}{sourceFlag.Trim()}{checkDigit}";
                    result = $"{anchor}-{noUrutBlok}";
                    break;
                case IDFormatEnum.PF_YYM_nnnC:
                    noUrutBlok = $"{Gen_3(noUrutHex)}{sourceFlag.Trim()}{checkDigit}";
                    result = $"{anchor}-{noUrutBlok}";
                    break;
                case IDFormatEnum.PFnnnn:
                    noUrutBlok = $"{Gen_4(noUrutHex)}{sourceFlag.Trim()}";
                    result = $"{anchor}{noUrutBlok}";
                    break;
                case IDFormatEnum.PFnnn:
                    noUrutBlok = $"{Gen_3(noUrutHex)}{sourceFlag.Trim()}";
                    result = $"{anchor}{noUrutBlok}";
                    break;
                case IDFormatEnum.Pnn:
                    noUrutBlok = $"{Gen_2(noUrutHex)}{sourceFlag.Trim()}";
                    result = $"{anchor}{noUrutBlok}";
                    break;
                case IDFormatEnum.PREFYYMnnnnnC:
                    noUrutBlok = $"{Gen_5(noUrutHex)}{sourceFlag.Trim()}{checkDigit}";
                    result = $"{anchor}{noUrutBlok}";
                    break;
                case IDFormatEnum.PYYMnnnn:
                    noUrutBlok = $"{Gen_4(noUrutHex)}{sourceFlag.Trim()}";
                    result = $"{anchor}{noUrutBlok}";
                    break;
                case IDFormatEnum.Pn7:
                    var noUrutDec = Convert.ToInt32(noUrutHex, 16);
                    noUrutBlok = $"{Gen_7_Dec(noUrutDec)}{sourceFlag.Trim()}";
                    result = $"{anchor}{noUrutBlok}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }

            return result;
        }

        private string GenerateB(string anchor, string prefix, int length)
        {
            var noUrut = _counterDal.GetNewHexNumber(anchor);
            if (noUrut is null)
            {
                noUrut = "0";
                _counterDal.InsertNewHexNumber(anchor, "0");
            }
            var noUrutNum = Convert.ToInt64(noUrut);
            noUrutNum++;
            _counterDal.UpdateNewHexNumber(anchor, noUrutNum.ToString());

            var prefixLength = (prefix.Length == 0 ? anchor : prefix).Length;
            var blokNo = noUrutNum.ToString().PadLeft(length-prefixLength, '0');
            var result = $"{(prefix.Length==0?anchor:prefix)}{blokNo}";
            return result;
        }

        private string Gen_7_Dec(int noUrut)
        {
            var result = $"{noUrut:D7}";
            return result;
        }

        private string Gen_5(string noUrutHex)
        {
            var result = $"{noUrutHex.PadLeft(5, '0')}";
            result = $"{result.Substring(0, 5)}";
            return result;
        }
        private string Gen_4(string noUrutHex)
        {
            var result = $"{noUrutHex.PadLeft(4, '0')}";
            result = $"{result.Substring(0, 4)}";
            return result;
        }
        private string Gen_3(string noUrutHex)
        {
            var result = $"{noUrutHex.PadLeft(3, '0')}";
            result = $"{result.Substring(0, 3)}";
            return result;
        }
        private string Gen_2(string noUrutHex)
        {
            var result = $"{noUrutHex.PadLeft(2, '0')}";
            return result;
        }
        private string AddHexa(string h1, string h2)
        {
            BigInteger number1 = BigInteger.Parse(h1, NumberStyles.HexNumber);
            BigInteger number2 = BigInteger.Parse(h2, NumberStyles.HexNumber);
            BigInteger sum = BigInteger.Add(number1, number2);
            return sum.ToString("X");
        }
    }
}