using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.ReturJualAgg
{
    //  Create Data Access Layer for ReturJual
    public class ReturJualDal : IReturJualDal
    {
        private readonly DatabaseOptions _opt;

        public ReturJualDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(ReturJualModel model)
        {
            //  create insert query for ReturJualModel to table BTR_ReturJual
            const string  sql = @"
                INSERT INTO BTR_ReturJual (
                    ReturJualId, ReturJualDate, FakturId, FakturCode,
                    CustomerId, SalesPersonId, WarehouseId, UserId,
                    Total, DiscRp, PpnRp, GrandTotal)
                VALUES (
                    @ReturJualId, @ReturJualDate, @FakturId, @FakturCode,
                    @CustomerId, @SalesPersonId, @WarehouseId, @UserId,
                    @Total, @DiscRp, @PpnRp, @GrandTotal)";
            
            //  assign parameter in query to model using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", model.ReturJualId, SqlDbType.VarChar);
            dp.AddParam("@ReturJualDate", model.ReturJualDate, SqlDbType.DateTime);
            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@DiscRp", model.DiscRp, SqlDbType.Decimal);
            dp.AddParam("@PpnRp", model.PpnRp, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            
            //  execute query using dapper
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(ReturJualModel model)
        {
            //  create update query for ReturJualModel to table BTR_ReturJual
            const string sql = @"
                UPDATE 
                    BTR_ReturJual 
                SET
                    ReturJualDate = @ReturJualDate, 
                    FakturId = @FakturId, 
                    FakturCode = @FakturCode,
                    CustomerId = @CustomerId, 
                    SalesPersonId = @SalesPersonId, 
                    WarehouseId = @WarehouseId, 
                    UserId = @UserId,
                    Total = @Total, 
                    DiscRp = @DiscRp, 
                    PpnRp = @PpnRp, 
                    GrandTotal = @GrandTotal
                WHERE 
                    ReturJualId = @ReturJualId";
            
            //  assign parameter in query to model using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", model.ReturJualId, SqlDbType.VarChar);
            dp.AddParam("@ReturJualDate", model.ReturJualDate, SqlDbType.DateTime);
            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@DiscRp", model.DiscRp, SqlDbType.Decimal);
            dp.AddParam("@PpnRp", model.PpnRp, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            
            //  execute query using dapper
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IReturJualKey key)
        {
            //  create query delete for ReturJualModel from table BTR_ReturJual
            const string sql = @"
                DELETE FROM 
                    BTR_ReturJual 
                WHERE 
                    ReturJualId = @ReturJualId";
            
            //  assign parameter in query to key using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, SqlDbType.VarChar);
            
            //  execute query using dapper
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public ReturJualModel GetData(IReturJualKey key)
        {
            //  create query select for ReturJualModel from table BTR_ReturJual
            const string sql = @"
                SELECT 
                    aa.ReturJualId, aa.ReturJualDate, aa.FakturId, aa.FakturCode,
                    aa.CustomerId, aa.SalesPersonId, aa.WarehouseId, aa.UserId,
                    aa.Total, aa.DiscRp, aa.PpnRp, aa.GrandTotal,
                    ISNULL(cc.CustomerName, '') AS CustomerName,
                    ISNULL(dd.SalesPersonName, '') AS SalesPersonName,
                    ISNULL(ee.WarehouseName, '') AS WarehouseName
                FROM 
                    BTR_ReturJual aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId
                    LEFT JOIN BTR_SalesPerson dd ON aa.SalesPersonId = dd.SalesPersonId
                    LEFT JOIN BTR_Warehouse ee ON aa.WarehouseId = ee.WarehouseId
                WHERE 
                    ReturJualId = @ReturJualId";
            
            //  assign parameter in query to key using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, SqlDbType.VarChar);
            
            //  execute query using dapper
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<ReturJualModel>(sql, dp);
            }
        }

        public IEnumerable<ReturJualModel> ListData(Periode filter)
        {
            //  create query select for ReturJualModel from table BTR_ReturJual with filter ReturJualDate
            const string sql = @"
                SELECT 
                    aa.ReturJualId, aa.ReturJualDate, aa.FakturId, aa.FakturCode,
                    aa.CustomerId, aa.SalesPersonId, aa.WarehouseId, aa.UserId,
                    aa.Total, aa.DiscRp, aa.PpnRp, aa.GrandTotal,
                    ISNULL(cc.CustomerName, '') AS CustomerName,
                    ISNULL(dd.SalesPersonName, '') AS SalesPersonName,
                    ISNULL(ee.WarehouseName, '') AS WarehouseName
                FROM 
                    BTR_ReturJual aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId
                    LEFT JOIN BTR_SalesPerson dd ON aa.SalesPersonId = dd.SalesPersonId
                    LEFT JOIN BTR_Warehouse ee ON aa.WarehouseId = ee.WarehouseId
                WHERE 
                    ReturJualDate BETWEEN @Tgl1 AND @Tgl2
                ORDER BY 
                    ReturJualDate DESC";
            
            //  assign parameter in query to filter using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);
            
            //  execute query using dapper
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturJualModel>(sql, dp);
            }
        }
    }
}