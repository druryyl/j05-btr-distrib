using btr.application.InventoryContext.StokBalanceReport;
using btr.domain.InventoryContext.StokBalanceReport;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace btr.infrastructure.InventoryContext.StokBalanceReport
{
    public class StokBalanceReportDal : IStokBalanceReportDal
    {
        public readonly DatabaseOptions _opt;

        public StokBalanceReportDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }
        public IEnumerable<StokBalanceView> ListData()
        {
            const string sql = @"
                SELECT      aa.BrgId, aa.WarehouseId, aa.Qty,
                            ISNULL(bb.BrgName, '') AS BrgName,
                            ISNULL(bb.BrgCode, '') AS BrgCode,
                            ISNULL(bb.SupplierId, '') AS SupplierId,
                            ISNULL(bb.KategoriId, '') AS KategoriId,
                            ISNULL(bb.Hpp, 0) AS Hpp,
                            ISNULL(cc1.Satuan, '') AS SatKecil,
                            ISNULL(cc2.Satuan, '') AS SatBesar,
                            ISNULL(cc2.Conversion, 1) AS Conversion,
                            ISNULL(dd.WarehouseName, '') AS WarehouseName,
                            ISNULL(ee.KategoriName, '') AS KategoriName,
                            ISNULL(ff.SupplierName, '') AS SupplierName
                FROM        BTR_StokBalanceWarehouse aa
                LEFT JOIN   BTR_Brg bb ON aa.BrgId = bb.BrgId
                LEFT JOIN   (
                            SELECT  DISTINCT BrgId, Satuan, 1 AS Conversion
                            FROM    BTR_BrgSatuan
                            WHERE   Conversion = 1) cc1 ON aa.BrgId = cc1.BrgId
                LEFT JOIN   (
                            SELECT  DISTINCT BrgId, Satuan, Conversion
                            FROM    BTR_BrgSatuan
                            WHERE   Conversion > 1) cc2 ON aa.BrgId = cc2.BrgId
                LEFT JOIN   BTR_Warehouse dd ON aa.WarehouseId = dd.WarehouseId
                LEFT JOIN   BTR_Kategori ee ON bb.KategoriId = ee.KategoriId
                LEFT JOIN   BTR_Supplier ff ON bb.SupplierId = ff.SupplierId ";
            
            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokBalanceView>(sql);
            }
          }
    }
}
