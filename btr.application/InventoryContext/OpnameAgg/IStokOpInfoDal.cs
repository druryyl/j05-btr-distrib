using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.InventoryContext.OpnameAgg
{
    public interface IStokOpInfoDal :
        IListData<StokOpInfoView, Periode>
    {
    }

    public class StokOpInfoView
    {
        public string WarehouseName { get; set; }
        public DateTime PeriodeOp { get; set; }
        public string SupplierName { get; set; }
        public string KategoriName { get; set; }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public int QtyBesarAwal { get; set; }
        public int QtyKecilAwal { get; set; }
        public decimal QtyPcsAwal { get; set; }

        public int QtyBesarAdjust { get; set; }
        public int QtyKecilAdjust { get; set; }
        public decimal QtyPcsAdjust { get; set; }

        public int QtyBesarOpname { get; set; }
        public int QtyKecilOpname { get; set; }
        public decimal QtyPcsOpname { get; set; }

        public string UserId { get; set; }
        public string StokOpId { get; set; }

        public decimal HppSatuan { get; set; }


        public decimal NilaiAwal { get => HppSatuan * QtyPcsAwal; }
        public decimal NilaiAdjust { get => HppSatuan * QtyPcsAdjust; }
        public decimal NilaiOpname { get => HppSatuan * QtyPcsOpname; }
    }
}
