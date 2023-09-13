using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.PackingAgg
{
    public class PackingDal : IPackingDal
    {
        private readonly DatabaseOptions _opt;

        public PackingDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(PackingModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Packing(
                PackingId, PackingDate, DriverId, Route)
            VALUES (
                @PackingId, @PackingDate, @DriverId, @Route)";

            var @dp = new DynamicParameters();
            dp.AddParam("@PackingId", model.PackingId, SqlDbType.VarChar);
            dp.AddParam("@PackingDate", model.PackingDate, SqlDbType.DateTime);
            dp.AddParam("@DriveId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@Route", model.Route, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(PackingModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Packing
            SET
                PackingDate = @PackingDate,
                DriverId = @DriverId,
                Route = @Route
            WHERE
                PackingId = @PackingId ";

            var @dp = new DynamicParameters();
            dp.AddParam("@PackingId", model.PackingId, SqlDbType.VarChar);
            dp.AddParam("@PackingDate", model.PackingDate, SqlDbType.DateTime);
            dp.AddParam("@DriveId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@Route", model.Route, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IPackingKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Packing
            WHERE
                PackingId = @PackingId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", key.PackingId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public PackingModel GetData(IPackingKey key)
        {
            const string sql = @"
            SELECT
                aa.PackingId, aa.PackingDate, aa.DriverId, aa.Route
                ISNULL(bb.DriverName, '') AS DriverName
            FROM
                BTR_Packing aa
                LEFT JOIN BTR_Driver bb ON aa.DriverId = bb.DriverId
            WHERE
                PackingId = @PackingId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", key.PackingId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<PackingModel>(sql, dp);
            }
        }

        public IEnumerable<PackingModel> ListData(Periode periode)
        {
            const string sql = @"
            SELECT
                aa.PackingId, aa.PackingDate, aa.ReffId,
                aa.BrgId,  aa.WarehouseId, aa.QtyIn, aa.Qty,
                aa.NilaiPersediaan,
                ISNULL(bb.BrgName, '') AS BrgName,
                ISNULL(cc.WarehouseName, '') AS WarehouseName
            FROM
                BTR_Packing aa
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
                return conn.Read<PackingModel>(sql, dp);
            }
        }
    }
}