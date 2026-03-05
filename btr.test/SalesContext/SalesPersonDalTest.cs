using btr.domain.SalesContext.SalesPersonAgg;
using btr.infrastructure.Helpers;
using btr.infrastructure.SalesContext.SalesPersonAgg;
using btr.nuna.Application;
using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace btr.test.SalesContext
{
    public class SalesPersonDalTest
    {
        private readonly SalesPersonDal _sut;

        public SalesPersonDalTest()
        {
            var databaseOptions = new DatabaseOptions
            {
                ServerName = "JUDE7",
                DbName = "devTest",
                IsTest = true
            };
            _sut = new SalesPersonDal(Options.Create(databaseOptions));
        }

        private SalesPersonModel Faker()
        {
            return new SalesPersonModel
            {
                SalesPersonId = "A",
                SalesPersonCode = "B",
                SalesPersonName = "C",
                WilayahId = "W1",
                WilayahName = "Wilayah1",
                Email = "test@example.com",
                SegmentId = "S1",
                SegmentName = "Segment1"
            };
        }

        [Fact]
        public void UT1_InsertTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Insert(model);
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

        [Fact]
        public void UT3_DeleteTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Delete(new SalesPersonModel(model.SalesPersonId));
            }
        }

        [Fact]
        public void UT4_GetDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Insert(model);
                var actual = _sut.GetData(new SalesPersonModel(model.SalesPersonId));
                actual.Should().BeEquivalentTo(model, 
                    opt => opt
                        .Excluding(x => x.SegmentName)
                        .Excluding(x => x.WilayahName));
            }
        }

        [Fact]
        public void UT5_ListDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                var model = Faker();
                _sut.Insert(model);
                var actual = _sut.ListData();
                actual.Should().BeEquivalentTo(new List<SalesPersonModel> { model },
                    opt => opt
                        .Excluding(x => x.SegmentName)
                        .Excluding(x => x.WilayahName));
            }
        }
    }
}
