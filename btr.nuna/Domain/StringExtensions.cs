namespace btr.nuna.Domain
{
    public static class StringExtensions
    {
        public static string FixWidth(this string str, int length)
        {
            return str.Length > length ? str.Substring(0, length) : str.PadRight(length, ' ');
        } 
    }
}