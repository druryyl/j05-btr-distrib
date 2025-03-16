using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using System;
using System.Collections.Generic;

namespace btr.domain.InventoryContext.MutasiAgg
{
    public class MutasiModel : IMutasiKey, IWarehouseKey, IUserKey
    {
        public MutasiModel()
        {
        }
        public MutasiModel(string id) => MutasiId = id;

        public string MutasiId { get; set; }
        public JenisMutasiEnum JenisMutasi { get; set; }
        
        public string Keterangan { get; set; }
        public DateTime MutasiDate { get; set; }
        public DateTime KlaimDate { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        
        public string UserId { get; set; }
        public decimal NilaiSediaan { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime VoidDate { get; set; }
        public string UserIdVoid { get; set; }
        public bool IsVoid { get => VoidDate != new DateTime(3000, 1, 1); }

        public List<MutasiItemModel> ListItem { get; set; }
    }

    public enum JenisMutasiEnum
    {
        MutasiKeluar, MutasiMasuk, KlaimSupplier
    }
}