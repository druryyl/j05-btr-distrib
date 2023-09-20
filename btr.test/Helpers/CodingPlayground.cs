using FluentAssertions;
using btr.nuna.Domain;
using Xunit;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace btr.test.Helpers
{
    public class CodingPlayground
    {
        [Fact]
        public void GetIntegerPartOfDecimal()
        {
            decimal dec = 101.93M;
            var intPart = (int)dec;
            intPart.Should().Be(101);
        }

        [Fact]
        public void ArrayEmptyTest()
        {
            var arr = new string[] {"Agus"};
            var arr1 = arr[0];
            var arr2 = arr.Length > 1 ? arr[1] : string.Empty;
            arr2.Should().Be(string.Empty);
        }

        [Fact]
        public void EjaBilanganTest()
        {
            decimal bilangan = 100;
            var result = bilangan.Eja();
            result.Should().Be("seratus");
        }
    
        [Fact]
        public void FormatNumerikTest()
        {
            var x = new decimal(10.00);
            var y = x.ToString("0.##");
            y.Should().Be("10");

        }

        [Fact]
        public void PembulatanKeBawah()
        {
            int x = 10;
            var  y = 5.1;
            var resul1 = x / y;
            var result = (int)resul1;
            result.Should().Be(1);
        }
        [Fact]
        public void RegexTest()
        {
            const string pattern = @"^\d{3}\.\d{3}-\d{2}\.(\d{8})$";
            const string input = "010.000-10.23456789";

            var matched = Regex.Match(input, pattern);
            long noAwalN =0;
            if (matched.Success)
            {
                noAwalN = long.Parse(matched.Groups[1].Value);
            }

            noAwalN.Should().Be(23456789);

        }
    }
}
