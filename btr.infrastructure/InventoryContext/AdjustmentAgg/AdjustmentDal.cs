using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.AdjustmentAgg;
using btr.domain.InventoryContext.AdjustmentAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.AdjustmentAgg
{
    public class AdjustmentDal : IAdjustmentDal
    {
        private readonly DatabaseOptions _opt;

        public AdjustmentDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(AdjustmentModel model)
        {
            //  create insert statement
            const string sql = @"
                INSERT INTO BTR_Adjustment(
                    AdjustmentId, AdjustmentDate, WarehouseId, BrgId, Alasan,
                    QtyAwalBesar, QtyAwalKecil, QtyAwalInPcs, 
                    QtyAdjustBesar, QtyAdjustKecil, QtyAdjustInPcs, 
                    QtyAkhirBesar, QtyAkhirKecil, QtyAkhirInPcs)
                VALUES(
                    @AdjustmentId, @AdjustmentDate, @WarehouseId, @BrgId, @Alasan,  
                    @QtyAwalBesar, @QtyAwalKecil, @QtyAwalInPcs, 
                    @QtyAdjustBesar, @QtyAdjustKecil, @QtyAdjustInPcs, 
                    @QtyAkhirBesar, @QtyAkhirKecil, @QtyAkhirInPcs)";
            
            //  create parameter
            var dp = new DynamicParameters();
            dp.AddParam("@AdjustmentId", model.AdjustmentId, SqlDbType.VarChar);
            dp.AddParam("@AdjustmentDate", model.AdjustmentDate, SqlDbType.DateTime);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@Alasan", model.Alasan, SqlDbType.VarChar);
            dp.AddParam("@QtyAwalBesar", model.QtyAwalBesar, SqlDbType.Int);
            dp.AddParam("@QtyAwalKecil", model.QtyAwalKecil, SqlDbType.Int);
            dp.AddParam("@QtyAwalInPcs", model.QtyAwalInPcs, SqlDbType.Int);
            dp.AddParam("@QtyAdjustBesar", model.QtyAdjustBesar, SqlDbType.Int);
            dp.AddParam("@QtyAdjustKecil", model.QtyAdjustKecil, SqlDbType.Int);
            dp.AddParam("@QtyAdjustInPcs", model.QtyAdjustInPcs, SqlDbType.Int);
            dp.AddParam("@QtyAkhirBesar", model.QtyAkhirBesar, SqlDbType.Int);
            dp.AddParam("@QtyAkhirKecil", model.QtyAkhirKecil, SqlDbType.Int);
            dp.AddParam("@QtyAkhirInPcs", model.QtyAkhirInPcs, SqlDbType.Int);
            
            //  execute
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(AdjustmentModel model)
        {
            //  create update statement
            const string sql = @"
                UPDATE 
                    BTR_Adjustment 
                SET
                    AdjustmentDate = @AdjustmentDate, 
                    WarehouseId = @WarehouseId, 
                    BrgId = @BrgId,  
                    Alasan = @Alasan,
                    QtyAwalBesar = @QtyAwalBesar, 
                    QtyAwalKecil = @QtyAwalKecil, 
                    QtyAwalInPcs = @QtyAwalInPcs, 
                    QtyAdjustBesar = @QtyAdjustBesar, 
                    QtyAdjustKecil = @QtyAdjustKecil, 
                    QtyAdjustInPcs = @QtyAdjustInPcs, 
                    QtyAkhirBesar = @QtyAkhirBesar, 
                    QtyAkhirKecil = @QtyAkhirKecil, 
                    QtyAkhirInPcs = @QtyAkhirInPcs
                WHERE 
                    AdjustmentId = @AdjustmentId";
            
            //  create parameter
            var dp = new DynamicParameters();
            dp.AddParam("@AdjustmentId", model.AdjustmentId, SqlDbType.VarChar);
            dp.AddParam("@AdjustmentDate", model.AdjustmentDate, SqlDbType.DateTime);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@Alasan", model.Alasan, SqlDbType.VarChar);
            
            dp.AddParam("@QtyAwalBesar", model.QtyAwalBesar, SqlDbType.Int);
            dp.AddParam("@QtyAwalKecil", model.QtyAwalKecil, SqlDbType.Int);
            dp.AddParam("@QtyAwalInPcs", model.QtyAwalInPcs, SqlDbType.Int);
            dp.AddParam("@QtyAdjustBesar", model.QtyAdjustBesar, SqlDbType.Int);
            dp.AddParam("@QtyAdjustKecil", model.QtyAdjustKecil, SqlDbType.Int);
            dp.AddParam("@QtyAdjustInPcs", model.QtyAdjustInPcs, SqlDbType.Int);
            dp.AddParam("@QtyAkhirBesar", model.QtyAkhirBesar, SqlDbType.Int);
            dp.AddParam("@QtyAkhirKecil", model.QtyAkhirKecil, SqlDbType.Int);
            dp.AddParam("@QtyAkhirInPcs", model.QtyAkhirInPcs, SqlDbType.Int);
            
            //  execute
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IAdjustmentKey key)
        {
            //  create delete statement
            const string sql = @"
                DELETE FROM 
                    BTR_Adjustment 
                WHERE 
                    AdjustmentId = @AdjustmentId";
            
            //  create parameter
            var dp = new DynamicParameters();
            dp.AddParam("@AdjustmentId", key.AdjustmentId, SqlDbType.VarChar);
            
            //  execute
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public AdjustmentModel GetData(IAdjustmentKey key)
        {
            //  create select statement
            const string sql = @"
                SELECT 
                    aa.AdjustmentId, aa.AdjustmentDate, aa.WarehouseId, aa.BrgId, aa.Alasan, 
                    aa.QtyAwalBesar, aa.QtyAwalKecil, aa.QtyAwalInPcs, 
                    aa.QtyAdjustBesar, aa.QtyAdjustKecil, aa.QtyAdjustInPcs, 
                    aa.QtyAkhirBesar, aa.QtyAkhirKecil, aa.QtyAkhirInPcs,
                    ISNULL(bb.BrgCode, '') AS BrgCode, 
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM 
                    BTR_Adjustment aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE 
                    aa.AdjustmentId = @AdjustmentId";
            
            //  create parameter
            var dp = new DynamicParameters();
            dp.AddParam("@AdjustmentId", key.AdjustmentId, SqlDbType.VarChar);
            
            //  execute
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<AdjustmentModel>(sql, dp);
            }
        }

        public IEnumerable<AdjustmentModel> ListData(Periode filter)
        {
            //  create select statement
            const string sql = @"
                SELECT 
                    aa.AdjustmentId, aa.AdjustmentDate, aa.WarehouseId, aa.BrgId, aa.Alasan, 
                    aa.QtyAwalBesar, aa.QtyAwalKecil, aa.QtyAwalInPcs, 
                    aa.QtyAdjustBesar, aa.QtyAdjustKecil, aa.QtyAdjustInPcs, 
                    aa.QtyAkhirBesar, aa.QtyAkhirKecil, aa.QtyAkhirInPcs,
                    ISNULL(bb.BrgCode, '') AS BrgCode, 
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM 
                    BTR_Adjustment aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE 
                    aa.AdjustmentDate BETWEEN @StartDate AND @EndDate";
            
            //  create parameter
            var dp = new DynamicParameters();
            dp.AddParam("@StartDate", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@EndDate", filter.Tgl2, SqlDbType.DateTime);
            
            
            //  execute
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<AdjustmentModel>(sql, dp);
            }
        }
    }
}