﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.InventoryContext.PackingAgg
{
    public class PackingModel : IPackingKey
    {
        public string PackingId { get; }
        public string PackingDate { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string Route { get; set; }
        public List<PackingFakturModel> ListFaktur { get; set; }
        public List<PackingSupplierModel> ListSupplier { get; set; }
    }

    public class PackingFakturModel
    {
        public string PackingId { get; set; }
        public string FakturId { get; set; }
        public string CustomerName { get; set; }
        public string Alamat { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class PackingSupplierModel
    {
        public string PackingId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        
        public List<PackingBrgModel> ListBrg { get; set; }
    }

    public class PackingBrgModel
    {
        public string PackingId { get; set; }

        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int NoUrut {get;set;}
        public string BrgId { get; set; }
        public string BrgName { get; set; }

        public int QtyKecil { get; set; }
        public string SatuanKecil { get; set; }
        public int QtyBesar { get; set; }
        public string SatuanBesar { get; set; }
        public decimal HargaJual { get; set; }
    }

    public interface IPackingKey
    {
        string PackingId { get; }
    }
}