namespace btr.nuna.Domain
{
    public static class StringExtensions
    {
        public static string FixWidth(this string str, int length)
        {
            return str.Length > length ? str.Substring(0, length) : str.PadRight(length, ' ');
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
    }
}