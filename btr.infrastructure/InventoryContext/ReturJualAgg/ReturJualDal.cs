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
            // create insert query for ReturJualModel to table BTR_ReturJual
            const string sql = @"
                INSERT INTO 
                    BTR_ReturJual (
                        ReturJualId, ReturJualDate, JenisRetur,
                        CustomerId, WarehouseId, UserId,
                        SalesPersonId, DriverId,
                        Total, DiscRp, PpnRp, GrandTotal,
                        VoidDate, UserIdVoid
                    )
                VALUES (
                        @ReturJualId, @ReturJualDate, @JenisRetur,
                        @CustomerId, @WarehouseId, @UserId,
                        @SalesPersonId, @DriverId,
                        @Total, @DiscRp, @PpnRp, @GrandTotal,
                        @VoidDate, @UserIdVoid
                    )";

            //  assign parameter in query to model using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", model.ReturJualId, SqlDbType.VarChar);
            dp.AddParam("@ReturJualDate", model.ReturJualDate, SqlDbType.DateTime);
            dp.AddParam("@JenisRetur", model.JenisRetur, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@DriverId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@DiscRp", model.DiscRp, SqlDbType.Decimal);
            dp.AddParam("@PpnRp", model.PpnRp, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime);
            dp.AddParam("@UserIdVoid", model.UserIdVoid, SqlDbType.VarChar);

            //  execute query using dapper
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(ReturJualModel model)
        {
            // create table update query for ReturJualModel to table BTR_ReturJual
            const string sql = @"
                UPDATE 
                    BTR_ReturJual 
                SET 
                    ReturJualDate = @ReturJualDate,
                    JenisRetur = @JenisRetur,
                    UserId = @UserId,

                    CustomerId = @CustomerId,
                    WarehouseId = @WarehouseId,
                    SalesPersonId = @SalesPersonId,
                    DriverId = @DriverId,

                    Total = @Total,
                    DiscRp = @DiscRp,
                    PpnRp = @PpnRp,
                    GrandTotal = @GrandTotal,

                    VoidDate = @VoidDate,   
                    UserIdVoid = @UserIdVoid
                WHERE 
                    ReturJualId = @ReturJualId";
            
            //  assign parameter in query to model using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", model.ReturJualId, SqlDbType.VarChar);
            dp.AddParam("@ReturJualDate", model.ReturJualDate, SqlDbType.DateTime);
            dp.AddParam("@JenisRetur", model.JenisRetur, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@DriverId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@DiscRp", model.DiscRp, SqlDbType.Decimal);
            dp.AddParam("@PpnRp", model.PpnRp, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime);
            dp.AddParam("@UserIdVoid", model.UserIdVoid, SqlDbType.VarChar);

            
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
                    aa.ReturJualId, aa.ReturJualDate, aa.UserId, aa.JenisRetur,
                    aa.CustomerId, aa.WarehouseId, 
                    aa.SalesPersonId, aa.DriverId,
                    aa.Total, aa.DiscRp, aa.PpnRp, aa.GrandTotal,
                    aa.VoidDate, aa.UserIdVoid,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName,
                    ISNULL(dd.SalesPersonName, '') AS SalesPersonName,
                    ISNULL(ee.DriverName, '') AS DriverName
                FROM 
                    BTR_ReturJual aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                    LEFT JOIN BTR_SalesPerson dd ON aa.SalesPersonId = dd.SalesPersonId
                    LEFT JOIN BTR_Driver ee ON ee.DriverId = ee.DriverId
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
                    aa.ReturJualId, aa.ReturJualDate, aa.UserId, aa.JenisRetur
                    aa.CustomerId, aa.WarehouseId, 
                    aa.SalesPersonId, aa.DriverId,
                    aa.Total, aa.DiscRp, aa.PpnRp, aa.GrandTotal,
                    aa.VoidDate, aa.UserIdVoid,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName,
                    ISNULL(dd.SalesPersonName, '') AS SalesPersonName,
                    ISNULL(ee.DriverName, '') AS DriverName
                FROM 
                    BTR_ReturJual aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                    LEFT JOIN BTR_SalesPerson dd ON aa.SalesPersonId = dd.SalesPersonId
                    LEFT JOIN BTR_Driver ee ON aa.DriverId = ee.DriverId
                WHERE 
                    ReturJualDate BETWEEN @StartDate AND @EndDate
                ORDER BY 
                    ReturJualDate DESC";
            
            var dp = new DynamicParameters();
            dp.AddParam("@StartDate", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@EndDate", filter.Tgl2, SqlDbType.DateTime);
            
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturJualModel>(sql, dp);
            }

        }
    }
}