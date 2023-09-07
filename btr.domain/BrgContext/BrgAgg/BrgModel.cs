using System;
using System.Collections.Generic;
using btr.domain.BrgContext.KategoriAgg;
using btr.domain.PurchaseContext.SupplierAgg;

namespace btr.domain.BrgContext.BrgAgg
{
    public class BrgModel : IBrgKey, ISupplierKey, IKategoriKey
    {
        public BrgModel()
        {
        }

        public BrgModel(string id) => BrgId = id;

        public string BrgId { get; set; }
        public string BrgName { get; set;}
        public string BrgCode { get; set; }
        public bool IsAktif { get; set; }
        
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        
        public string KategoriId { get; set; }
        public string KategoriName { get; set; }

        public string JenisBrgId { get; set; }
        public string JenisBrgName { get; set; }
        
        public decimal Hpp { get; set; }
        public DateTime HppTimestamp { get; set; }
        public List<BrgSatuanModel> ListSatuan { get; set; }
        public List<BrgHargaModel> ListHarga { get; set; }
    }
}