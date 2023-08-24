using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
