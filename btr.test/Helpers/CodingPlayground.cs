using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
