using System;
using System.Globalization;

namespace btr.nuna.Domain
{
    public enum DateFormatEnum
    {
        YMD,
        DMY,
        YMD_HMS,
        YMD_HM,
        HMS,
        HM
    }

    public static class DateTimeConversion
    {
        private const string YMD = "yyyy-MM-dd";
        private const string DMY = "dd-MM-yyyy";
        private const string YMD_HMS = "yyyy-MM-dd HH:mm:ss";
        private const string YMD_HM = "yyyy-MM-dd HH:mm";
        private const string HMS = "HH:mm:ss";
        private const string HM = "HH:mm";



        public static DateTime ToDate(this string stringTgl)
        {
            DateTime dummyDate;
            //  coba parsing sebagai DMY
            bool isValid = DateTime.TryParseExact(stringTgl, DMY,
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out dummyDate);

            //  jika tidak berhasil, parsing sebagai YMD
            if (!isValid)
            {
                isValid = DateTime.TryParseExact(stringTgl, YMD,
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out dummyDate);
            }

            if (isValid)
            {
                return dummyDate;
            }
            else
            {
                throw new InvalidOperationException("Invalid string date");
            }
        }

        private static string GetStringDateTimeFormat(DateFormatEnum format)
        {
            var formatStr = YMD;
            switch (format)
            {
                case DateFormatEnum.YMD:
                    formatStr = YMD;
                    break;
                case DateFormatEnum.DMY:
                    formatStr = DMY;
                    break;
                case DateFormatEnum.YMD_HMS:
                    formatStr = YMD_HMS;
                    break;
                case DateFormatEnum.YMD_HM:
                    formatStr = YMD_HM;
                    break;
                case DateFormatEnum.HMS:
                    formatStr = HMS;
                    break;
                case DateFormatEnum.HM:
                    formatStr = HM;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }

            return formatStr;
        }

        public static DateTime ToDate(this string stringTgl, DateFormatEnum format)
        {

            var formatStr = GetStringDateTimeFormat(format);
            return stringTgl.ToDate(formatStr);

        }

        public static DateTime ToDate(this string stringTgl, string format)
        {
            DateTime dummyDate;
            var isValid = DateTime.TryParseExact(stringTgl, format,
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out dummyDate);

            if (isValid)
            {
                return dummyDate;
            }
            else
            {
                throw new InvalidOperationException("Invalid string date");
            }
        }

        public static string ToString(this DateTime dtTgl, DateFormatEnum format)
        {
            var formatStr = GetStringDateTimeFormat(format);
            var result = dtTgl.ToString(formatStr, CultureInfo.InvariantCulture);
            return result;
        }

        public static string GetHariName(this string stringTgl)
        {
            var dtTgl = ToDate(stringTgl);
            var culture = new System.Globalization.CultureInfo("id-ID");
            var hariName = culture.DateTimeFormat.GetDayName(dtTgl.DayOfWeek);
            return hariName;
        }
    }
}