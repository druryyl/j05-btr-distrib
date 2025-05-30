﻿using btr.application.InventoryContext.KartuStokRpt;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using btr.nuna.Infrastructure;
using System.Data.SqlClient;
using btr.domain.InventoryContext.WarehouseAgg;

namespace btr.infrastructure.InventoryContext.KartuStokRpt
{
    public class KartuStokSummaryDal : IKartuStokSummaryDal
    {
        private readonly DatabaseOptions _opt;

        public KartuStokSummaryDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<KartuStokSummaryDto> ListData(Periode filter, IWarehouseKey warehouseKey)
        {
            const string sql = @"
                SELECT 
                    aa1.BrgId, 
                    aa3.WarehouseName AS Warehouse, 
                    aa4.SupplierName AS Supplier,
                    aa2.KategoriName AS Kategori, 
                    aa1.BrgName, 
                    aa1.BrgCode,
                    ISNULL(bb.QtyBalance,0) AS Invoice,
                    ISNULL(cc.QtyBalance,0) AS Faktur,
                    ISNULL(dd.QtyBalance,0) AS Retur,
                    ISNULL(ee.QtyBalance,0) AS Mutasi,
                    ISNULL(ff.QtyBalance,0) AS Opname
                FROM( 
                    SELECT  aa.BrgId, bb.WarehouseId
                    FROM    BTR_Brg aa
                    CROSS JOIN BTR_Warehouse bb) aa

                LEFT JOIN BTR_Brg aa1 ON aa.BrgId = aa1.BrgId
                LEFT JOIN BTR_Kategori aa2 ON aa1.KategoriId = aa2.KategoriId
                LEFT JOIN BTR_Warehouse aa3 ON aa.WarehouseId = aa3.WarehouseId
                LEFT JOIN BTR_Supplier aa4 ON aa1.SupplierId = aa4.SupplierId

                LEFT JOIN(
                    SELECT  aa.BrgId, cc.WarehouseId, 'INVOICE' AS JenisMutasi,
                            SUM(aa.QtyIn) - SUM(aa.QtyOut) AS QtyBalance
                    FROM    BTR_StokMutasi aa
                            LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                            LEFT jOIN BTR_Stok cc ON aa.StokId = cc.StokId
                    WHERE   aa.MutasiDate BETWEEN @StartDate AND @EndDate
                            AND aa.JenisMutasi IN ('INVOICE', 'INVOICE-BONUS')
                            AND cc.WarehouseId = @WarehouseId
                    GROUP BY aa.BrgId, cc.WarehouseId) bb ON aa.BrgId = bb.BrgId AND aa.WarehouseId = bb.WarehouseId

                LEFT JOIN(
                    SELECT  aa.BrgId, cc.WarehouseId, 'FAKTUR' AS JenisMutasi,
                            SUM(aa.QtyIn) - SUM(aa.QtyOut) AS QtyBalance
                    FROM    BTR_StokMutasi aa
                            LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                            LEFT jOIN BTR_Stok cc ON aa.StokId = cc.StokId
                    WHERE   aa.MutasiDate BETWEEN @StartDate AND @EndDate
                            AND aa.JenisMutasi IN ('FAKTUR', 'FAKTUR-BONUS')
                            AND cc.WarehouseId = @WarehouseId
                    GROUP BY aa.BrgId, cc.WarehouseId) cc ON aa.BrgId = cc.BrgId AND aa.WarehouseId = cc.WarehouseId

                LEFT JOIN(
                    SELECT  aa.BrgId, cc.WarehouseId, 'RETUR' AS JenisMutasi,
                            SUM(aa.QtyIn) - SUM(aa.QtyOut) AS QtyBalance
                    FROM    BTR_StokMutasi aa
                            LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                            LEFT jOIN BTR_Stok cc ON aa.StokId = cc.StokId
                    WHERE   aa.MutasiDate BETWEEN @StartDate AND @EndDate
                            AND aa.JenisMutasi IN ('RETURJUAL')
                            AND cc.WarehouseId = @WarehouseId
                    GROUP BY aa.BrgId, cc.WarehouseId) dd ON aa.BrgId = dd.BrgId AND aa.WarehouseId = dd.WarehouseId

                LEFT JOIN(
                    SELECT  aa.BrgId, cc.WarehouseId, 'MUTASI' AS JenisMutasi,
                            SUM(aa.QtyIn) - SUM(aa.QtyOut) AS QtyBalance
                    FROM    BTR_StokMutasi aa
                            LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                            LEFT jOIN BTR_Stok cc ON aa.StokId = cc.StokId
                    WHERE   aa.MutasiDate BETWEEN @StartDate AND @EndDate
                            AND aa.JenisMutasi IN ('MUTASI-KLAIM', 'MUTASI-KELUAR', 'MUTASI-MASUK')
                            AND cc.WarehouseId = @WarehouseId
                    GROUP BY aa.BrgId, cc.WarehouseId) ee ON aa.BrgId = ee.BrgId AND aa.WarehouseId = ee.WarehouseId

                LEFT JOIN(
                    SELECT  aa.BrgId, cc.WarehouseId, 'OPNAME' AS JenisMutasi,
                            SUM(aa.QtyIn) - SUM(aa.QtyOut) AS QtyBalance
                    FROM    BTR_StokMutasi aa
                            LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                            LEFT jOIN BTR_Stok cc ON aa.StokId = cc.StokId
                    WHERE   aa.MutasiDate BETWEEN @StartDate AND @EndDate
                            AND aa.JenisMutasi IN ('OPNAME', 'STOKOP', 'ADJUST')
                            AND cc.WarehouseId = @WarehouseId
                    GROUP BY aa.BrgId, cc.WarehouseId) ff ON aa.BrgId = ff.BrgId AND aa.WarehouseId = ff.WarehouseId
                WHERE       
                    aa.WarehouseId = @WarehouseId";

            var dp = new DynamicParameters();
            dp.AddParam("@StartDate", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@EndDate", filter.Tgl2, SqlDbType.DateTime);
            dp.AddParam("@WarehouseId", warehouseKey.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<KartuStokSummaryDto>(sql, dp);
            }
        }
    }
}
