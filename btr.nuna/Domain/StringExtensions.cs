using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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

        public static string HashSha256(this string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash to a hexadecimal string
                StringBuilder hashBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashBuilder.Append(hashBytes[i].ToString("x2")); // "x2" formats the byte as a two-digit hexadecimal number
                }

                return hashBuilder.ToString();
            }
        }
    }
}