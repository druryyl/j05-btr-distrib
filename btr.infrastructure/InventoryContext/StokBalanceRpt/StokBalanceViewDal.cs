using System.Collections.Generic;
using System.Data.SqlClient;
using btr.application.InventoryContext.StokBalanceInfo;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.StokBalanceRpt
{
    public class StokBalanceViewDal : IStokBalanceViewDal
    {
        public readonly DatabaseOptions _opt;

        public StokBalanceViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }
        public IEnumerable<StokBalanceView> ListData()
        {
            const string sql = @"
                SELECT      aa.BrgId, aa.BrgName, aa.BrgCode, 
                            aa.SupplierId, aa.KategoriId, aa.Hpp,
                            ISNULL(bb.WarehouseId, '') WarehouseId, 
                            ISNULL(bb.Qty, 0) Qty,
                            ISNULL(cc1.Satuan, '') AS SatKecil,
                            ISNULL(cc2.Satuan, '') AS SatBesar,
                            ISNULL(cc2.Conversion, 1) AS Conversion,
                            ISNULL(dd.WarehouseName, '') AS WarehouseName,
                            ISNULL(ee.KategoriName, '') AS KategoriName,
                            ISNULL(ff.SupplierName, '') AS SupplierName
                FROM        BTR_Brg aa 
                LEFT JOIN   BTR_StokBalanceWarehouse bb ON bb.BrgId = aa.BrgId
                LEFT JOIN   (
                            SELECT  DISTINCT BrgId, Satuan, 1 AS Conversion
                            FROM    BTR_BrgSatuan
                            WHERE   Conversion = 1) cc1 ON bb.BrgId = cc1.BrgId
                LEFT JOIN   (
                            SELECT  DISTINCT BrgId, Satuan, Conversion
                            FROM    BTR_BrgSatuan
                            WHERE   Conversion > 1) cc2 ON bb.BrgId = cc2.BrgId
                LEFT JOIN   BTR_Warehouse dd ON bb.WarehouseId = dd.WarehouseId
                LEFT JOIN   BTR_Kategori ee ON aa.KategoriId = ee.KategoriId
                LEFT JOIN   BTR_Supplier ff ON aa.SupplierId = ff.SupplierId ";
            
            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokBalanceView>(sql);
            }
          }
    }
}
