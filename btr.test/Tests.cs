using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace btr.test
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            const int x = 5;
            var y = $"{x:D2}";
            
            Assert.True(y == "05");
        }

        [Fact]
        public void DefaultIfEmptyTest()
        {
            var mhs = new MhsModel
            {
                ListNilai = new List<MhsNilaiModel>()
            };
            var y = mhs.ListNilai
                .DefaultIfEmpty(new MhsNilaiModel())
                .Max(x => x.NoUrut+1);
            Assert.True(y == 1);
        }
    }

    public class MhsModel
    {
        public List<MhsNilaiModel> ListNilai { get; set; }
    }

    public class MhsNilaiModel
    {
        public int NoUrut { get; set; }
    }
}