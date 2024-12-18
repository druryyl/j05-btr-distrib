using btr.application.InventoryContext.KartuStokRpt;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.KartuStokRpt;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.InventoryContext.KartuStokRpt
{
    public class KartuStokDal : IKartuStokDal
    {
        private readonly DatabaseOptions _opt;

        public KartuStokDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<KartuStokView> ListData(Periode filter, IBrgKey brgKey)
        {
            //  query table BTR_StokMutasi and project to KartuStokView
            const string sql = @"
                SELECT
                    aa.WarehouseId, aa.StokId, aa.NoUrut, aa.MutasiDate, aa.JenisMutasi, 
                    aa.ReffId, aa.MutasiDate, aa.QtyIn, aa.QtyOut, aa.HargaJual, aa.Keterangan, 
                    bb.NilaiPersediaan as Hpp
                FROM
                    BTR_StokMutasi aa
                    LEFT JOIN BTR_Stok bb ON aa.StokId = bb.StokId
                WHERE
                    aa.BrgId = @BrgId AND
                    aa.MutasiDate BETWEEN @StartDate AND @EndDate
                ORDER BY
                    aa.StokId, aa.NoUrut ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", brgKey.BrgId, SqlDbType.VarChar);
            dp.AddParam("@StartDate", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@EndDate", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<KartuStokView>(sql, dp);
            }   
        }

        public KartuStokStokAwalView GetSaldoAwal(Periode filter, IBrgKey brgKey, IWarehouseKey warehouseKey)
        {
            //  query table BTR_StokMutasi and project to KartuStokView
            const string sql = @"
                SELECT
                    aa.WarehouseId, 
                    SUM(QtyIn) AS QtyMasuk,
                    SUM(QtyOut) AS QtyKeluar,       
                    SUM(QtyIn - QtyOut) AS QtyAkhir,
                    ISNULL(bb.WarehouseName,'') AS WarehouseName
                FROM
                    BTR_StokMutasi aa
                    LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                WHERE
                    aa.BrgId = @BrgId AND
                    aa.WarehouseId = @WarehouseId AND
                    aa.MutasiDate <= @StartDate
                GROUP BY
                    aa.WarehouseId, bb.WarehouseName";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", brgKey.BrgId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", warehouseKey.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@StartDate", filter.Tgl1.AddSeconds(-1), SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<KartuStokStokAwalView>(sql, dp);
            }
        }
    }
}
