using System;
using System.Collections.Generic;
using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.infrastructure.PurchaseContext.PurchaseOrderAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using btr.test.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace btr.test.PurchaseContext.PurchaseOrderAgg
{
    public class PurchaseOrderDalTest
    {
        private readonly PurchaseOrderDal _sut;

        public PurchaseOrderDalTest()
        {
            var opt = Options.Create(AppSettingsHelper.GetDatabaseOptions());
            _sut = new PurchaseOrderDal(opt);
        }

        private static PurchaseOrderModel Faker() =>
            new PurchaseOrderModel
            {
                PurchaseOrderId = "A",
                PurchaseOrderDate = new DateTime(2023,8,15),
                UserId = "B",
                SupplierId = "C",
                SupplierName = string.Empty,
                WarehouseId = "D",
                WarehouseName = string.Empty,
            };
        
        [Fact]
        public void InsertTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
            }
        }

        [Fact]
        public void UpdateTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Update(Faker());
            }
        }

        [Fact]
        public void DeleteTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
                _sut.Delete(Faker());
            }
        }

        [Fact]
        public void GetDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
                var actual = _sut.GetData(Faker());
                actual.Should().BeEquivalentTo(Faker());
            }        
        }

        [Fact]
        public void ListDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(Faker());
                var actual = _sut.ListData(new Periode(new DateTime(2023,8,15)));
                actual.Should().BeEquivalentTo(new List<PurchaseOrderModel> { Faker() });
            }
        }
    }
}