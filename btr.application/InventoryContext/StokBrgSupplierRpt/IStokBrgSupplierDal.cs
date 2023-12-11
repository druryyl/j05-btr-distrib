using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.StokBrgSupplierRpt
{
    public interface IStokBrgSupplierDal :
        IListData<StokBrgSupplierView>
    {
    }
    public class StokBrgSupplierView
    {
        public string BrgId { get; set; }
        public int Qty { get; set; }
        public string WarehouseName { get; set; }
        public string SupplierName { get; set; }
        public string KategoriName { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string SatuanBesar { get; set; }
        public int Conversion { get; set; }
        public string SatuanKecil { get; set; }
        public decimal HargaMT { get; set; }
        public decimal HargaGT { get; set; }
        public decimal Hpp { get; set; }
    }
}
