using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace btr.test.SalesContext
{
    public class NumericHelperTest
    {
        [Fact]
        public void Eja_1467347()
        {
            decimal x = 1467347;
            var eja = x.Eja();
            Debug.WriteLine(eja);
            Assert.True(eja == "satu juta empat ratus enam puluh tujuh ribu tiga ratus empat puluh tujuh rupiah");
        }
        [Fact]
        public void Eja_1111111()
        {
            decimal x = 1111111;
            var eja = x.Eja();
            Debug.WriteLine(eja);
            Assert.True(eja == "satu juta seratus sebelas ribu seratus sebelas rupiah");
        }
        [Fact]
        public void Eja_11000000()
        {
            decimal x = 11000000;
            var eja = x.Eja();
            Debug.WriteLine(eja);
            Assert.True(eja == "sebelas juta");
        }

    }
}
