using btr.application.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.InventoryContext.OpnameAgg
{
    public class StokOpDal : IStokOpDal
    {
        private readonly DatabaseOptions _opt;

        public StokOpDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(StokOpModel model)
        {
            //  query insert to table BTR_StokOp
            const string sql = @"
                INSERT INTO BTR_StokOp
                (
                    StokOpId, StokOpDate, PeriodeOp, BrgId, WarehouseId,
                    QtyBesarAwal, QtyKecilAwal, QtyPcsAwal,
                    QtyBesarOpname, QtyKecilOpname, QtyPcsOpname,
                    QtyBesarAdjust, QtyKecilAdjust, QtyPcsAdjust,
                    QtyOpnameInputStr, UserId
                )
                VALUES
                (
                    @StokOpId, @StokOpDate, @PeriodeOp, @BrgId, @WarehouseId,
                    @QtyBesarAwal, @QtyKecilAwal, @QtyPcsAwal, 
                    @QtyBesarOpname, @QtyKecilOpname, @QtyPcsOpname,
                    @QtyBesarAdjust, @QtyKecilAdjust, @QtyPcsAdjust, 
                    @QtyOpnameInputStr, @UserId)";

            //  parameter
            var dp = new DynamicParameters();
            dp.AddParam("@StokOpId", model.StokOpId, SqlDbType.VarChar);
            dp.AddParam("@StokOpDate", model.StokOpDate, SqlDbType.DateTime);
            dp.AddParam("@PeriodeOp", model.PeriodeOp, SqlDbType.DateTime);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@QtyBesarAwal", model.QtyBesarAwal, SqlDbType.Int);
            dp.AddParam("@QtyKecilAwal", model.QtyKecilAwal, SqlDbType.Int);
            dp.AddParam("@QtyPcsAwal", model.QtyPcsAwal, SqlDbType.Int);
            dp.AddParam("@QtyBesarOpname", model.QtyBesarOpname, SqlDbType.Int);
            dp.AddParam("@QtyKecilOpname", model.QtyKecilOpname, SqlDbType.Int);
            dp.AddParam("@QtyPcsOpname", model.QtyPcsOpname, SqlDbType.Int);
            dp.AddParam("@QtyBesarAdjust", model.QtyBesarAdjust, SqlDbType.Int);
            dp.AddParam("@QtyKecilAdjust", model.QtyKecilAdjust, SqlDbType.Int);
            dp.AddParam("@QtyPcsAdjust", model.QtyPcsAdjust, SqlDbType.Int);
            dp.AddParam("@QtyOpnameInputStr", model.QtyOpnameInputStr, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);

            //  execute
            using (var con = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                con.Execute(sql, dp);
            }
        }

        public void Update(StokOpModel model)
        {
            //  query update to table BTR_StokOp    
            const string sql = @"
                UPDATE BTR_StokOp
                SET
                    StokOpDate = @StokOpDate,
                    PeriodeOp = @PeriodeOp,
                    BrgId = @BrgId,
                    WarehouseId = @WarehouseId,
                    QtyBesarAwal = @QtyBesarAwal,
                    QtyKecilAwal = @QtyKecilAwal,
                    QtyPcsAwal = @QtyPcsAwal,
                    QtyBesarOpname = @QtyBesarOpname,
                    QtyKecilOpname = @QtyKecilOpname,
                    QtyPcsOpname = @QtyPcsOpname,
                    QtyBesarAdjust = @QtyBesarAdjust,
                    QtyKecilAdjust = @QtyKecilAdjust,
                    QtyPcsAdjust = @QtyPcsAdjust,
                    QtyOpnameInputStr = @QtyOpnameInputStr,
                    UserId = @UserId
                WHERE 
                    StokOpId = @StokOpId";

            //  parameter
            var dp = new DynamicParameters();
            dp.AddParam("@StokOpId", model.StokOpId, SqlDbType.VarChar);
            dp.AddParam("@StokOpDate", model.StokOpDate, SqlDbType.DateTime);
            dp.AddParam("@PeriodeOp", model.PeriodeOp, SqlDbType.DateTime);
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@QtyBesarAwal", model.QtyBesarAwal, SqlDbType.Int);
            dp.AddParam("@QtyKecilAwal", model.QtyKecilAwal, SqlDbType.Int);
            dp.AddParam("@QtyPcsAwal", model.QtyPcsAwal, SqlDbType.Int);
            dp.AddParam("@QtyBesarOpname", model.QtyBesarOpname, SqlDbType.Int);
            dp.AddParam("@QtyKecilOpname", model.QtyKecilOpname, SqlDbType.Int);
            dp.AddParam("@QtyPcsOpname", model.QtyPcsOpname, SqlDbType.Int);
            dp.AddParam("@QtyBesarAdjust", model.QtyBesarAdjust, SqlDbType.Int);
            dp.AddParam("@QtyKecilAdjust", model.QtyKecilAdjust, SqlDbType.Int);
            dp.AddParam("@QtyPcsAdjust", model.QtyPcsAdjust, SqlDbType.Int);
            dp.AddParam("@QtyOpnameInputStr", model.QtyOpnameInputStr, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);

            //  execute
            using (var con = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                con.Execute(sql, dp);
            }
        }

        public void Delete(IStokOpKey key)
        {
            //  query delete from table BTR_StokOp
            const string sql = @"
                DELETE FROM BTR_StokOp
                WHERE StokOpId = @StokOpId";

            //  parameter
            var dp = new DynamicParameters();
            dp.Add("@StokOpId", key.StokOpId);
            
            //  execute
            using (var con = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                con.Execute(sql, dp);
            }
        }

        public StokOpModel GetData(IStokOpKey key)
        {
            //  query select from table BTR_StokOp
            const string sql = @"
                SELECT
                    aa.StokOpId, aa.StokOpDate, aa.PeriodeOp, aa.BrgId, aa.WarehouseId,
                    aa.QtyBesarAwal, aa.QtyKecilAwal, aa.QtyPcsAwal,
                    aa.QtyBesarOpname, aa.QtyKecilOpname, aa.QtyPcsOpname,
                    aa.QtyBesarAdjust, aa.QtyKecilAdjust, aa.QtyPcsAdjust,
                    aa.QtyOpnameInputStr, aa.UserId,
                    ISNULL(BrgName, '') AS BrgName, 
                    ISNULL(WarehouseName ,'') AS WarehouseName
                FROM 
                    BTR_StokOp aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE 
                    StokOpId = @StokOpId";

            //  parameter
            var dp = new DynamicParameters();
            dp.Add("@StokOpId", key.StokOpId);

            //  execute
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<StokOpModel>(sql, dp);
            }
        }

        public IEnumerable<StokOpModel> ListData(Periode filter1, IWarehouseKey filter2)
        {
            //  query select from table BTR_StokOp
            const string sql = @"
                SELECT
                    aa.StokOpId, aa.StokOpDate, aa.PeriodeOp, aa.BrgId, aa.WarehouseId,
                    aa.QtyBesarAwal, aa.QtyKecilAwal, aa.QtyPcsAwal,
                    aa.QtyBesarOpname, aa.QtyKecilOpname, aa.QtyPcsOpname,
                    aa.QtyBesarAdjust, aa.QtyKecilAdjust, aa.QtyPcsAdjust,
                    aa.QtyOpnameInputStr, aa.UserId,
                    ISNULL(BrgName, '') AS BrgName, 
                    ISNULL(WarehouseName ,'') AS WarehouseName
                FROM 
                    BTR_StokOp aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE 
                    PeriodeOp BETWEEN @Tgl1 AND @Tgl2
                    AND aa.WarehouseId = @WarehouseId";

            //  parameter
            var dp = new DynamicParameters();
            dp.Add("@Tgl1", filter1.Tgl1);
            dp.Add("@Tgl2", filter1.Tgl2);
            dp.Add("@WarehouseId", filter2.WarehouseId);

            //  execute
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokOpModel>(sql, dp);
            }
        }
    }
}
