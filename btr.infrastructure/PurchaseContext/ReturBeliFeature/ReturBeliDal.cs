using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.PurchaseContext.ReturBeliFeature;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.ReturBeliFeature
{
    public class ReturBeliDal : IReturBeliDal
    {
        private readonly DatabaseOptions _opt;

        public ReturBeliDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(ReturBeliModel model)
        {
            const string sql = @"
                INSERT INTO BTR_ReturBeli (
                    ReturBeliId, ReturBeliDate, ReturBeliCode, SupplierId,  WarehouseId, 
                    NoFakturPajak, DueDate, Total, Disc, Dpp, Tax, GrandTotal,
                    CreateTime, LastUpdate, UserId, VoidDate, UserIdVoid, IsStokPosted)
                VALUES(
                    @ReturBeliId, @ReturBeliDate, @ReturBeliCode, @SupplierId,  @WarehouseId, 
                    @NoFakturPajak, @DueDate, @Total, @Disc, @Dpp, @Tax, @GrandTotal,
                    @CreateTime, @LastUpdate, @UserId, @VoidDate, @UserIdVoid, @IsStokPosted)";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturBeliId", model.ReturBeliId, SqlDbType.VarChar); 
            dp.AddParam("@ReturBeliDate", model.ReturBeliDate, SqlDbType.DateTime); 
            dp.AddParam("@ReturBeliCode", model.ReturBeliCode, SqlDbType.VarChar); 
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);  
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar); 
            dp.AddParam("@NoFakturPajak", model.NoFakturPajak, SqlDbType.VarChar);
            
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);  
            dp.AddParam("@Disc", model.Disc, SqlDbType.Decimal);
            dp.AddParam("@Dpp", model.Dpp, SqlDbType.Decimal);
            dp.AddParam("@Tax", model.Tax, SqlDbType.Decimal);  
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);

            dp.AddParam("@CreateTime", model.CreateTime, SqlDbType.DateTime); 
            dp.AddParam("@LastUpdate", model.LastUpdate, SqlDbType.DateTime); 
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar); 
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime); 
            dp.AddParam("@UserIdVoid", model.UserIdVoid, SqlDbType.VarChar);
            dp.AddParam("@IsStokPosted", model.IsStokPosted, SqlDbType.Bit);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(ReturBeliModel model)
        {
            const string sql = @"
                UPDATE  
                    BTR_ReturBeli
                SET
                    ReturBeliDate = @ReturBeliDate, 
                    ReturBeliCode = @ReturBeliCode, 
                    SupplierId = @SupplierId,  
                    WarehouseId = @WarehouseId, 
                    NoFakturPajak = @NoFakturPajak, 
                    DueDate = @DueDate, 
                    Total = @Total, 
                    Disc = @Disc, 
                    Dpp = @Dpp,
                    Tax = @Tax, 
                    GrandTotal = @GrandTotal,
                    CreateTime = @CreateTime,
                    LastUpdate = @LastUpdate,
                    UserId = @UserId,
                    VoidDate = @VoidDate,
                    UserIdVoid = @UserIdVoid,
                    IsStokPosted = @IsStokPosted
                WHERE
                    ReturBeliId = @ReturBeliId";

            var dp = new DynamicParameters();

            dp.AddParam("@ReturBeliId", model.ReturBeliId, SqlDbType.VarChar); 
            dp.AddParam("@ReturBeliDate", model.ReturBeliDate, SqlDbType.DateTime); 
            dp.AddParam("@ReturBeliCode", model.ReturBeliCode, SqlDbType.VarChar); 
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);  
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar); 
            dp.AddParam("@NoFakturPajak", model.NoFakturPajak, SqlDbType.VarChar); 
            
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);  
            dp.AddParam("@Disc", model.Disc, SqlDbType.Decimal);  
            dp.AddParam("@Dpp", model.Dpp, SqlDbType.Decimal);
            dp.AddParam("@Tax", model.Tax, SqlDbType.Decimal);  
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);

            dp.AddParam("@CreateTime", model.CreateTime, SqlDbType.DateTime);
            dp.AddParam("@LastUpdate", model.LastUpdate, SqlDbType.DateTime);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime);
            dp.AddParam("@UserIdVoid", model.UserIdVoid, SqlDbType.VarChar);
            dp.AddParam("@IsStokPosted", model.IsStokPosted, SqlDbType.Bit);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IReturBeliKey key)
        {
            const string sql = @"
                DELETE FROM  
                    BTR_ReturBeli
                WHERE
                    ReturBeliId = @ReturBeliId";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturBeliId", key.ReturBeliId, SqlDbType.VarChar); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public ReturBeliModel GetData(IReturBeliKey key)
        {
            const string sql = @"
                SELECT
                    aa.ReturBeliId, aa.ReturBeliDate, aa.ReturBeliCode, aa.SupplierId,  aa.WarehouseId, 
                    aa.NoFakturPajak, aa.DueDate, aa.Total, aa.Disc, aa.Dpp, aa.Tax, aa.GrandTotal,
                    aa.CreateTime, aa.LastUpdate, aa.UserId, aa.VoidDate, aa.UserIdVoid, aa.IsStokPosted, 
                    ISNULL(bb.SupplierName, '') AS SupplierName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_ReturBeli aa
                    LEFT JOIN BTR_SUpplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    ReturBeliId = @ReturBeliId ";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturBeliId", key.ReturBeliId, SqlDbType.VarChar); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<ReturBeliModel>(sql, dp);
            }
        }

        public IEnumerable<ReturBeliModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.ReturBeliId, aa.ReturBeliDate, aa.ReturBeliCode, aa.SupplierId,  aa.WarehouseId, 
                    aa.NoFakturPajak, aa.DueDate, aa.Total, aa.Disc, aa.Dpp, aa.Tax, aa.GrandTotal,
                    aa.CreateTime, aa.LastUpdate, aa.UserId, aa.VoidDate, aa.UserIdVoid, aa.IsStokPosted,
                    ISNULL(bb.SupplierName, '') AS SupplierName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_ReturBeli aa
                    LEFT JOIN BTR_SUpplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    ReturBeliDate BETWEEN @Tgl1 AND @Tgl2
                    AND aa.VoidDate = '3000-01-01'";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime); 
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                var result = conn.Read<ReturBeliModel>(sql, dp);
                return result;
            }
        }
    }
}