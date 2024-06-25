using btr.application.InventoryContext.StokBrgSupplierRpt;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace btr.infrastructure.InventoryContext.StokBrgSupplierRpt
{
    public class StokBrgSupplierDal : IStokBrgSupplierDal
    {
        private readonly DatabaseOptions _opt;

        public StokBrgSupplierDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<StokBrgSupplierView> ListData()
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.Qty,
                    ISNULL(bb.WarehouseName, '') WarehouseName,
                    ISNULL(dd.SupplierName, '') SupplierName,
                    ISNULL(ee.KategoriName, '') KategoriName,   
                    ISNULL(cc.BrgCode, '') BrgCode,
                    ISNULL(cc.BrgName, '') BrgName,
                    ISNULL(ff1.Satuan,'') SatuanBesar,
                    ISNULL(ff1. Conversion,0) Conversion,
                    ISNULL(ff2.Satuan, '') SatuanKecil,
                    ISNULL(ggMT.Harga, 0) HargaMT,
                    ISNULL(ggGT.Harga, 0) HargaGT,
                    ISNULL(cc.Hpp, 0) Hpp
                FROM
                    BTR_StokBalanceWarehouse aa
                    LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                    LEFT JOIN BTR_Brg cc ON aa.BrgId = cc.BrgId
                    LEFT JOIN BTR_Supplier dd ON cc.SupplierId = dd.SupplierId
                    LEFT JOIN BTR_Kategori ee ON cc.KategoriId = ee.KategoriId
                    LEFT JOIN (
                        SELECT BrgId, Satuan, Conversion
                        FROM BTR_BrgSatuan
                        WHERE Conversion = 1
                    ) ff2 ON cc.BrgId = ff2.BrgId
                    LEFT JOIN (
                        SELECT BrgId, Satuan, MAX(Conversion) Conversion
                        FROM BTR_BrgSatuan
                        WHERE Conversion > 1
                        GROUP BY BrgId, Satuan
                    ) ff1 ON cc.BrgId = ff1.BrgId
                    LEFT JOIN (
                        SELECT BrgId, HargaTypeId, Harga
                        FROM BTR_BrgHarga
                        WHERE HargaTypeId = 'GT'
                    ) ggGT ON cc.BrgId = ggGT.BrgId
                        LEFT JOIN (
                        SELECT BrgId, HargaTypeId, Harga
                        FROM BTR_BrgHarga
                        WHERE HargaTypeId = 'MT'
                    ) ggMT ON cc.BrgId = ggMT.BrgId";

            //  connect db and execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokBrgSupplierView>(sql);
            }
        }
    }
}
