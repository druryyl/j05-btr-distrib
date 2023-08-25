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
    public class PurchaseOrderItemDalTest
    {
        private readonly PurchaseOrderItemDal _sut;

        public PurchaseOrderItemDalTest()
        {
            var opt = Options.Create(AppSettingsHelper.GetDatabaseOptions());
            _sut = new PurchaseOrderItemDal(opt);
        }

        private static PurchaseOrderItemModel Faker() =>
            new PurchaseOrderItemModel
            {
                PurchaseOrderId = "A",
                PurchaseOrderItemId = "B",
                NoUrut = 2,
                BrgId = "C",
                BrgName = string.Empty,
                Qty = 13,
                Satuan = "D",
                Harga = 15,
                SubTotal = 16,
                DiskonProsen = 17,
                DiskonRp = 18,
                TaxProsen = 19,
                TaxRp = 20,
                Total = 21
            };
        
        [Fact]
        public void InsertTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(new List<PurchaseOrderItemModel>{Faker()});
            }
        }

        [Fact]
        public void DeleteTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(new List<PurchaseOrderItemModel>{Faker()});
                _sut.Delete(Faker());
            }
        }

        [Fact]
        public void ListDataTest()
        {
            using (var trans = TransHelper.NewScope())
            {
                _sut.Insert(new List<PurchaseOrderItemModel>{Faker()});
                var actual = _sut.ListData(Faker());
                actual.Should().BeEquivalentTo(new List<PurchaseOrderItemModel> { Faker() });
            }
        }
    }
}