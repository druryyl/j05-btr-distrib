using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.OpnameAgg
{
    public class OpnameDal : IOpnameDal
    {
        private readonly DatabaseOptions _opt;

        public OpnameDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(OpnameModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Opname(
                    OpnameId, OpnameDate, UserId, 
                    BrgId, BrgCode, WarehouseId, 
                    Qty1Awal, Qty1Opname, Qty1Adjust, Satuan1, 
                    Qty2Awal,  Qty2Opname, Qty2Adjust, Satuan2)
                VALUES(
                    @OpnameId, @OpnameDate, @UserId,
                    @BrgId, @BrgCode, @WarehouseId,
                    @Qty1Awal, @Qty1Opname, @Qty1Adjust, @Satuan1,
                    @Qty2Awal, @ Qty2Opname, @Qty2Adjust, @Satuan2)";

            var dp = new DynamicParameters();
            dp.AddParam("@OpnameId", model.OpnameId, SqlDbType.VarChar); 
            dp.AddParam("@OpnameDate", model.OpnameDate, SqlDbType.VarChar); 
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar); 
            dp.AddParam("@BrgCode", model.BrgCode, SqlDbType.VarChar); 
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@Qty1Awal", model.Qty1Awal, SqlDbType.VarChar); 
            dp.AddParam("@Qty1Opname", model.Qty1Opname, SqlDbType.VarChar); 
            dp.AddParam("@Qty1Adjust", model.Qty1Adjust, SqlDbType.VarChar); 
            dp.AddParam("@Satuan1", model.Satuan1, SqlDbType.VarChar);
            dp.AddParam("@Qty2Awal", model.Qty2Awal, SqlDbType.VarChar); 
            dp.AddParam("@Qty2Opname", model.Qty2Opname, SqlDbType.VarChar); 
            dp.AddParam("@Qty2Adjust", model.Qty2Adjust, SqlDbType.VarChar); 
            dp.AddParam("@Satuan2", model.Satuan2, SqlDbType.VarChar);
            
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(OpnameModel model)
        {
            const string sql = @"
                UPDATE
                    BTR_Opname
                SET
                    OpnameId = @OpnameId, 
                    OpnameDate = @OpnameDate, 
                    UserId = @UserId, 
                    BrgId = @BrgId, 
                    BrgCode = @BrgCode, 
                    WarehouseId = @WarehouseId, 
                    Qty1Awal = @Qty1Awal, 
                    Qty1Opname = @Qty1Opname,
                    Qty1Adjust = @Qty1Adjust, 
                    Satuan1 = @Satuan1, 
                    Qty2Awal = @Qty2Awal,  
                    Qty2Opname = @Qty2Opname, 
                    Qty2Adjust = @Qty2Adjust, 
                    Satuan2 = @Satuan2,
                WHERE
                    OpnameId = @OpnameId";

            var dp = new DynamicParameters();
            dp.AddParam("@OpnameId", model.OpnameId, SqlDbType.VarChar); 
            dp.AddParam("@OpnameDate", model.OpnameDate, SqlDbType.VarChar); 
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar); 
            dp.AddParam("@BrgCode", model.BrgCode, SqlDbType.VarChar); 
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@Qty1Awal", model.Qty1Awal, SqlDbType.VarChar); 
            dp.AddParam("@Qty1Opname", model.Qty1Opname, SqlDbType.VarChar); 
            dp.AddParam("@Qty1Adjust", model.Qty1Adjust, SqlDbType.VarChar); 
            dp.AddParam("@Satuan1", model.Satuan1, SqlDbType.VarChar);
            dp.AddParam("@Qty2Awal", model.Qty2Awal, SqlDbType.VarChar); 
            dp.AddParam("@Qty2Opname", model.Qty2Opname, SqlDbType.VarChar); 
            dp.AddParam("@Qty2Adjust", model.Qty2Adjust, SqlDbType.VarChar); 
            dp.AddParam("@Satuan2", model.Satuan2, SqlDbType.VarChar);
            
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IOpnameKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_Opname
                WHERE
                    OpnameId = @OpnameId";

            var dp = new DynamicParameters();
            dp.AddParam("@OpnameId", key.OpnameId, SqlDbType.VarChar); 
            
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public OpnameModel GetData(IOpnameKey key)
        {
            const string sql = @"
                SELECT
                    aa.OpnameId, aa.OpnameDate, aa.UserId, 
                    aa.BrgId, aa.BrgCode, aa.WarehouseId, 
                    aa.Qty1Awal, aa.Qty1Opname, aa.Qty1Adjust, aa.Satuan1, 
                    aa.Qty2Awal,  aa.Qty2Opname, aa.Qty2Adjust, aa.Satuan2,
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseNameS
                FROM
                    BTR_Opname aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    aa.OpnameId = @OpnameId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@OpnameId", key.OpnameId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<OpnameModel>(sql, dp);
            }
        }

        public IEnumerable<OpnameModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.OpnameId, aa.OpnameDate, aa.UserId, 
                    aa.BrgId, aa.BrgCode, aa.WarehouseId, 
                    aa.Qty1Awal, aa.Qty1Opname, aa.Qty1Adjust, aa.Satuan1, 
                    aa.Qty2Awal,  aa.Qty2Opname, aa.Qty2Adjust, aa.Satuan2,
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseNameS
                FROM
                    BTR_Opname aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    aa.OpnameDate BETWEEN @Tgl1 AND @Tgl2 ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<OpnameModel>(sql, dp);
            }
        }
    }
}