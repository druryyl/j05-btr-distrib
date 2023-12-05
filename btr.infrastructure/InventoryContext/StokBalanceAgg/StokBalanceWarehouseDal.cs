using btr.application.InventoryContext.StokBalanceAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.domain.InventoryContext.WarehouseAgg;

namespace btr.infrastructure.InventoryContext.StokBalanceAgg
{
    public class StokBalanceWarehouseDal : IStokBalanceWarehouseDal
    {
        private readonly DatabaseOptions _opt;

        public StokBalanceWarehouseDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<StokBalanceWarehouseModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("WarehouseId", "WarehouseId");
                bcp.AddMap("Qty", "Qty");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_StokBalanceWarehouse";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IBrgKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_StokBalanceWarehouse
                WHERE
                    BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", key.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<StokBalanceWarehouseModel> ListData(IBrgKey filter)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.WarehouseId, aa.Qty,
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM 
                    BTR_StokBalanceWarehouse aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    aa.BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", filter.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokBalanceWarehouseModel>(sql, dp);
            }
        }

        public IEnumerable<StokBalanceWarehouseModel> ListData(IWarehouseKey filter)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.WarehouseId, aa.Qty,
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM 
                    BTR_StokBalanceWarehouse aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    aa.WarehouseId = @WarehouseId ";

            var dp = new DynamicParameters();
            dp.AddParam("@WarehouseId", filter.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokBalanceWarehouseModel>(sql, dp);
            }
        }
    }
}
