using FluentAssertions;
using btr.nuna.Domain;
using Xunit;

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
            int y = 6;
            var result = (int)x / y;
            result.Should().Be(1);
        }
    }
}
