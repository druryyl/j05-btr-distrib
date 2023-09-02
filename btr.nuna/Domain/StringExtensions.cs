using System.Collections.Generic;

namespace btr.nuna.Domain
{
    public static class StringExtensions
    {
        public static string FixWidth(this string str, int length)
        {
            return str.Length > length ? str.Substring(0, length) : str.PadRight(length, ' ');
        }
        public static string FixWidthRight(this string str, int length)
        {
            return str.Length > length ? str.Substring(0, length) : str.PadLeft(length, ' ');
        }
        public static bool ContainMultiWord(this string source, string words)
        {
            bool result = true;
            if (words == null) return true;
            string[] listWord = words.ToLower().Split(' ');
            source = source.ToLower();
            //return listWord.Any(source.ToLower().Contains);
            foreach (var item in listWord)
            {
                if (!source.Contains(item.ToLower()))
                    return false;
            }
            return result;
        }

        public static string[] WrapText(this string str, int length)
        {
            if (str is null)
                return new List<string>().ToArray();

            var listWord = str.Split(' ');
            var result = new List<string>();
            var subResult = string.Empty;
            foreach(var item in listWord)
            {
                if (subResult.Trim().Length + item.Length > length)
                {
                    result.Add(subResult);
                    subResult = string.Empty;
                }
                subResult = subResult + item + ' ';
            }
            if (subResult.Length > 0)
                result.Add(subResult);

            return result.ToArray();
        }
    }
}