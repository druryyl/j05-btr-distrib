using btr.domain.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.FakturAgg;
using System;
using System.Collections.Generic;
using btr.domain.SupportContext.UserAgg;

namespace btr.domain.InventoryContext.PackingAgg
{
    public class PackingModel : IPackingKey, IDriverKey, IWarehouseKey, IUserKey
    {
        public PackingModel()
        {
        }
        public PackingModel(string id) => PackingId = id;

        public string PackingId { get; set; }
        public DateTime PackingDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }


        public DateTime TglAwalFaktur { get; set; }
        public DateTime TglAkhirFaktur { get; set; }
        public string KeywordSearch { get; set; }
        
        public List<PackingFakturModel> ListFaktur { get; set; }
        public List<PackingBrgModel> ListBrg { get; set; }
    }

    public class PackingFakturModel : IFakturKey
    {
        public string PackingId { get; set; }
        public int NoUrut { get; set; }
        public string PackingFakturId { get; set; }
        public string FakturId { get; set; }
        public string FakturCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Kota { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class PackingBrgModel : IFakturKey
    {
        public string PackingId { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string FakturId { get; set; }

        public string SupplierId { get; set; }
        public int QtyBesar { get;  set; }
        public string SatBesar { get;  set; }
        public int QtyKecil { get;  set; }
        public string SatKecil { get;  set; }
        public decimal HargaJual { get; set; }
    }


    public interface IPackingKey
    {
        string PackingId { get; }
    }
}
