using btr.domain.SalesContext.KlasifikasiAgg;
using btr.infrastructure.Helpers;
using btr.infrastructure.SalesContext.KlasifikasiAgg;
using btr.nuna.Application;
using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace btr.test.SalesContext
{
    public class SegmentDalTest
    {
        private readonly SegmentDal _sut;

        public SegmentDalTest()
        {
            var databaseOptions = new DatabaseOptions
            {
                ServerName = "JUDE7",
                DbName = "devTest",
                IsTest = true
            };
            _sut = new SegmentDal(Options.Create(databaseOptions));
        }

        private SegmentType Faker() => new SegmentType("A", "B");

        [Fact]
        public void UT1_InsertTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
            }
        }
        [Fact]
        public void UT2_UpdateTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Update(model);
            }
        }
        //  create delete test
        [Fact]
        public void UT3_DeleteTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Delete(SegmentType.Key(model.SegmentId));
            }
        }

        [Fact]
        public void UT4_GetDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Insert(model);
                var actual = _sut.GetData(SegmentType.Key(model.SegmentId));
                actual.Should().BeEquivalentTo(model);
            }
        }
        [Fact]
        public void UT5_GetDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Insert(model);
                var actual = _sut.ListData();
                actual.Should().BeEquivalentTo(new List<SegmentType> { model });
            }
        }
    }
}
