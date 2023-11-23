using System;
using System.Collections.Generic;
using System.Linq;
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
        
        // [Fact]
        // public void InsertTest()
        // {
        //     using (var trans = TransHelper.NewScope())
        //     {
        //         _sut.Insert(Faker());
        //     }
        // }
        //
        // [Fact]
        // public void UpdateTest()
        // {
        //     using (var trans = TransHelper.NewScope())
        //     {
        //         _sut.Update(Faker());
        //     }
        // }
        //
        // [Fact]
        // public void DeleteTest()
        // {
        //     using (var trans = TransHelper.NewScope())
        //     {
        //         _sut.Insert(Faker());
        //         _sut.Delete(Faker());
        //     }
        // }
        //
        // [Fact]
        // public void GetDataTest()
        // {
        //     using (var trans = TransHelper.NewScope())
        //     {
        //         _sut.Insert(Faker());
        //         var actual = _sut.GetData(Faker());
        //         actual.Should().BeEquivalentTo(Faker());
        //     }        
        // }
        //
        // [Fact]
        // public void ListDataTest()
        // {
        //     using (var trans = TransHelper.NewScope())
        //     {
        //         _sut.Insert(Faker());
        //         var actual = _sut.ListData(new Periode(new DateTime(2023,8,15)));
        //         actual.Should().BeEquivalentTo(new List<PurchaseOrderModel> { Faker() });
        //     }
        // }

        [Fact]
        public void NullCoalessing_Null_Test()
        {
            List<BrgModel> listBrg = null;
            var actual = listBrg?.FirstOrDefault()?.Nilai ?? 9;
            actual.Should().Be(9);
        }

        [Fact]
        public void NullCoalessing_NotNull_Test()
        {
            var listBrg = new List<BrgModel>();
            var actual = listBrg?.FirstOrDefault()?.Nilai ?? 9;
            actual.Should().Be(9);
        }
        [Fact]
        public void NullCoalessing_NotEmpty_Test()
        {
            var listBrg = new List<BrgModel>
            {
                new BrgModel("A","AAA",8)
            };
            var actual = listBrg?.FirstOrDefault()?.Nilai ?? 9;
            actual.Should().Be(8);
        }
        
    }

    public class BrgModel
    {
        public BrgModel(string brgId, string brgName, decimal nilai)
        {
            BrgId = brgId;
            BrgName = brgName;
            Nilai = nilai;
        }
        public string BrgId { get; set; }
        public string BrgName { get; set; }
        public decimal Nilai { get; set; }
    }
}