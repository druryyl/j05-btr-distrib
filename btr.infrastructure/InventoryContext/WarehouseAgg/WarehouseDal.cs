using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.WarehouseAgg
{
    public class WarehouseDal : IWarehouseDal
    {
        private readonly DatabaseOptions _opt;

        public WarehouseDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(WarehouseModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Warehouse(
                WarehouseId, WarehouseName)
            VALUES (
                @WarehouseId, @WarehouseName)";

            var dp = new DynamicParameters();
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseName", model.WarehouseName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(WarehouseModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Warehouse
            SET
                WarehouseName = @WarehouseName
            WHERE
                WarehouseId = @WarehouseId ";

            var dp = new DynamicParameters();
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseName", model.WarehouseName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IWarehouseKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Warehouse
            WHERE
                WarehouseId = @WarehouseId ";

            var dp = new DynamicParameters();
            dp.AddParam("@WarehouseId", key.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public WarehouseModel GetData(IWarehouseKey key)
        {
            const string sql = @"
            SELECT
                WarehouseId, WarehouseName
            FROM
                BTR_Warehouse
            WHERE
                WarehouseId = @WarehouseId ";

            var dp = new DynamicParameters();
            dp.AddParam("@WarehouseId", key.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<WarehouseModel>(sql, dp);
            }
        }

        public IEnumerable<WarehouseModel> ListData()
        {
            const string sql = @"
            SELECT
                WarehouseId, WarehouseName
            FROM
                BTR_Warehouse
            WHERE
                IsAktif = 1";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<WarehouseModel>(sql);
            }
        }
    }
}