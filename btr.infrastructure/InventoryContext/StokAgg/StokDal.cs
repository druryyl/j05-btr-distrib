using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.StokAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.StokAgg
{
    public class StokDal : IStokDal
    {
        private readonly DatabaseOptions _opt;

        public StokDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(StokModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Stok(
                StokId, StokControlId, StokControlDate,
                BrgId,  WarehouseId, QtyIn, Qty,
                NilaiPersediaan)
            VALUES (
                @StokId, @StokControlId, @StokControlDate,
                @BrgId,  @WarehouseId, @QtyIn, @Qty,
                @NilaiPersediaan)";
            var @dp = new DynamicParameters();
            dp.AddParam("@StokId", model.StokId, SqlDbType.VarChar);
            dp.AddParam("@StokControlId", model.StokControlId, SqlDbType.VarChar);
            dp.AddParam("@StokControlDate", model.StokControlDate, SqlDbType.VarChar);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@QtyIn", model.QtyIn, SqlDbType.Int);
            dp.AddParam("@Qty", model.Qty, SqlDbType.Int);
            dp.AddParam("@NilaiPersediaan", model.NilaiPersediaan, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(StokModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Stok
            SET
                StokId = @StokId, 
                StokControlId = @StokControlId, 
                StokControlDate = @StokControlDate,
                BrgId = @BrgId,  
                WarehouseId = @WarehouseId, 
                QtyIn = @QtyIn, 
                Qty = @Qty,
                NilaiPersediaan = @NilaiPersediaan
            WHERE
                StokId = @StokId ";

            var @dp = new DynamicParameters();
            dp.AddParam("@StokId", model.StokId, SqlDbType.VarChar);
            dp.AddParam("@StokControlId", model.StokControlId, SqlDbType.VarChar);
            dp.AddParam("@StokControlDate", model.StokControlDate, SqlDbType.VarChar);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@QtyIn", model.QtyIn, SqlDbType.Int);
            dp.AddParam("@Qty", model.Qty, SqlDbType.Int);
            dp.AddParam("@NilaiPersediaan", model.NilaiPersediaan, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IStokKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Stok
            WHERE
                StokId = @StokId ";

            var dp = new DynamicParameters();
            dp.AddParam("@StokId", key.StokId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public StokModel GetData(IStokKey key)
        {
            const string sql = @"
            SELECT
                aa.StokId, aa.StokControlId, aa.StokControlDate,
                aa.BrgId,  aa.WarehouseId, aa.QtyIn, aa.Qty,
                aa.NilaiPersediaan,
                ISNULL(bb.BrgName, '') AS BrgName,
                ISNULL(cc.WarehouseName, '') AS WarehouseName
            FROM
                BTR_Stok aa
                LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                LEFT JOIN BTR_Warehouse cc on aa.WarehouseId = cc.WarehouseId
            WHERE
                StokId = @StokId ";

            var dp = new DynamicParameters();
            dp.AddParam("@StokId", key.StokId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<StokModel>(sql, dp);
            }
        }

        public IEnumerable<StokModel> ListData(IBrgKey brg, IWarehouseKey warehouse)
        {
            const string sql = @"
            SELECT
                aa.StokId, aa.StokControlId, aa.StokControlDate,
                aa.BrgId,  aa.WarehouseId, aa.QtyIn, aa.Qty,
                aa.NilaiPersediaan,
                ISNULL(bb.BrgName, '') AS BrgName,
                ISNULL(cc.WarehouseName, '') AS WarehouseName
            FROM
                BTR_Stok aa
                LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                LEFT JOIN BTR_Warehouse cc on aa.WarehouseId = cc.WarehouseId
            WHERE
                aa.BrgId = @BrgId
                AND aa.WarehouseId = @WarehouseId
                AND aa.Qty > 0";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", brg.BrgId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", warehouse.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokModel>(sql, dp);
            }
        }

        public IEnumerable<StokModel> ListDataAll(IBrgKey brg, IWarehouseKey warehouse)
        {
            const string sql = @"
            SELECT
                aa.StokId, aa.StokControlId, aa.StokControlDate,
                aa.BrgId,  aa.WarehouseId, aa.QtyIn, aa.Qty,
                aa.NilaiPersediaan,
                ISNULL(bb.BrgName, '') AS BrgName,
                ISNULL(cc.WarehouseName, '') AS WarehouseName
            FROM
                BTR_Stok aa
                LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                LEFT JOIN BTR_Warehouse cc on aa.WarehouseId = cc.WarehouseId
            WHERE
                aa.BrgId = @BrgId
                AND aa.WarehouseId = @WarehouseId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", brg.BrgId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", warehouse.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokModel>(sql, dp);
            }
        }
    }
}