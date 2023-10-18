using btr.application.InventoryContext.KartuStokRpt;
using btr.domain.InventoryContext.KartuStokRpt;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace btr.infrastructure.InventoryContext.KartuStokRpt
{
    public class KartuStokDal : IKartuStokDal
    {
        private readonly DatabaseOptions _opt;

        public KartuStokDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<KartuStokView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.WarehouseId, 
                    SUM(aa.QtyIn - QtyOut) QtyAwal,
                    SUM((aa.QtyIn - aa.QtyOut) * aa.NilaiPersediaan) NilaiAwal,
                    cc.BrgCode, cc.BrgName, 
                    dd.WarehouseName
                FROM (
                        SELECT aa.BrgId, aa.WarehouseId, aa.MutasiDate, aa.QtyIn, aa.QtyOut, bb.NilaiPersediaan
                        FROM BTR_StokMutasi aa
                        LEFT JOIN BTR_Stok bb ON aa.StokId = bb.StokId
                    ) aa
                    LEFT JOIN BTR_Brg cc ON aa.BrgId = cc.BrgId
                    LEFT JOIN BTR_Warehouse dd ON aa.WarehouseId = dd.WarehouseId
                WHERE
                    aa.MutasiDate <= '2023-09-18 23:59:59'
                GROUP BY
                    aa.BrgId, aa.WarehouseId, cc.BrgCode, cc.BrgName,
                    dd.WarehouseName ";
            return null;
        }
    }
}
