using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.MutasiAgg
{
    public interface IMutasiBrgViewDal
        : IListData<MutasiBrgView, Periode>
    {
    }

    public class MutasiBrgView
    {
        public string MutasiId { get; set; }
        public DateTime MutasiDate { get; set; }
        public string JenisMutasi { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string SupplierName { get; set; }
        public string KategoriName { get; set; }
        public int QtyBesar { get; set; }
        public string SatBesar { get; set; }
        public int QtyKecil { get; set; }
        public string SatKecil { get; set; }
        public int InPcs { get; set; }
        public decimal HrgSat { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscRp { get; set; }
        public decimal Total { get; set; }
    }
}
