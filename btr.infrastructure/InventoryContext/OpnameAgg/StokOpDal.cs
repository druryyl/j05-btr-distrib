using btr.application.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.OpnameAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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
                    UserId
                )
                VALUES
                (
                    @StokOpId, @StokOpDate, @PeriodeOp, @BrgId, @WarehouseId,
                    @QtyBesarAwal, @QtyKecilAwal, @QtyPcsAwal, 
                    @QtyBesarOpname, @QtyKecilOpname, @QtyPcsOpname,
                    @QtyBesarAdjust, @QtyKecilAdjust, @QtyPcsAdjust, @UserId)";

            //  parameter
            var dp = new DynamicParameters();
            dp.Add("@StokOpId", model.StokOpId);
            dp.Add("@StokOpDate", model.StokOpDate);
            dp.Add("@PeriodeOp", model.PeriodeOp);
            dp.Add("@BrgId", model.BrgId);
            dp.Add("@WarehouseId", model.WarehouseId);
            dp.Add("@QtyBesarAwal", model.QtyBesarAwal);
            dp.Add("@QtyKecilAwal", model.QtyKecilAwal);
            dp.Add("@QtyPcsAwal", model.QtyPcsAwal);
            dp.Add("@QtyBesarOpname", model.QtyBesarOpname);
            dp.Add("@QtyKecilOpname", model.QtyKecilOpname);
            dp.Add("@QtyPcsOpname", model.QtyPcsOpname);
            dp.Add("@QtyBesarAdjust", model.QtyBesarAdjust);
            dp.Add("@QtyKecilAdjust", model.QtyKecilAdjust);
            dp.Add("@QtyPcsAdjust", model.QtyPcsAdjust);
            dp.Add("@UserId", model.UserId);

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
                    UserId = @UserId
                WHERE StokOpId = @StokOpId";

            //  parameter
            var dp = new DynamicParameters();
            dp.Add("@StokOpId", model.StokOpId);
            dp.Add("@StokOpDate", model.StokOpDate);
            dp.Add("@PeriodeOp", model.PeriodeOp);
            dp.Add("@BrgId", model.BrgId);
            dp.Add("@WarehouseId", model.WarehouseId);
            dp.Add("@QtyBesarAwal", model.QtyBesarAwal);
            dp.Add("@QtyKecilAwal", model.QtyKecilAwal);
            dp.Add("@QtyPcsAwal", model.QtyPcsAwal);
            dp.Add("@QtyBesarOpname", model.QtyBesarOpname);
            dp.Add("@QtyKecilOpname", model.QtyKecilOpname);
            dp.Add("@QtyPcsOpname", model.QtyPcsOpname);
            dp.Add("@QtyBesarAdjust", model.QtyBesarAdjust);
            dp.Add("@QtyKecilAdjust", model.QtyKecilAdjust);
            dp.Add("@QtyPcsAdjust", model.QtyPcsAdjust);
            dp.Add("@UserId", model.UserId);

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
                    StokOpId, StokOpDate, PeriodeOp, BrgId, WarehouseId,
                    QtyBesarAwal, QtyKecilAwal, QtyPcsAwal,
                    QtyBesarOpname, QtyKecilOpname, QtyPcsOpname,
                    QtyBesarAdjust, QtyKecilAdjust, QtyPcsAdjust,
                    UserId,
                    ISNULL(BrgName, '') BrgName, 
                    ISNULL(WarehouseName ,'') WarehouseName
                FROM 
                    BTR_StokOp aa
                    LEFT JOIN BTR_Barang bb ON aa.BrgId = bb.BrgId
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
                    StokOpId, StokOpDate, PeriodeOp, BrgId, WarehouseId,
                    QtyBesarAwal, QtyKecilAwal, QtyPcsAwal,
                    QtyBesarOpname, QtyKecilOpname, QtyPcsOpname,
                    QtyBesarAdjust, QtyKecilAdjust, QtyPcsAdjust,
                    UserId,
                    ISNULL(BrgName, '') BrgName, 
                    ISNULL(WarehouseName ,'') WarehouseName
                FROM 
                    BTR_StokOp aa
                    LEFT JOIN BTR_Barang bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE 
                    PeriodeOp BETWEEN @Tgl1 AND @Tgl2
                    WarehouseId = @WarehouseId";

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
