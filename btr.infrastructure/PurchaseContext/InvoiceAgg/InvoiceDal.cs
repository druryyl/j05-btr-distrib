using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.InvoiceAgg
{
    public class InvoiceDal : IInvoiceDal
    {
        private readonly DatabaseOptions _opt;

        public InvoiceDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(InvoiceModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Invoice (
                    InvoiceId, InvoiceDate, InvoiceCode, SupplierId,  WarehouseId, 
                    NoFakturPajak, DueDate, Total, Disc, Dpp, Tax, GrandTotal,
                    CreateTime, LastUpdate, UserId, VoidDate, UserIdVoid, IsStokPosted, Note)
                VALUES(
                    @InvoiceId, @InvoiceDate, @InvoiceCode, @SupplierId,  @WarehouseId, 
                    @NoFakturPajak, @DueDate, @Total, @Disc, @Dpp, @Tax, @GrandTotal,
                    @CreateTime, @LastUpdate, @UserId, @VoidDate, @UserIdVoid, @IsStokPosted, @Note)";

            var dp = new DynamicParameters();
            dp.AddParam("@InvoiceId", model.InvoiceId, SqlDbType.VarChar); 
            dp.AddParam("@InvoiceDate", model.InvoiceDate, SqlDbType.DateTime); 
            dp.AddParam("@InvoiceCode", model.InvoiceCode, SqlDbType.VarChar); 
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
            dp.AddParam("@Note", model.Note, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(InvoiceModel model)
        {
            const string sql = @"
                UPDATE  
                    BTR_Invoice
                SET
                    InvoiceDate = @InvoiceDate, 
                    InvoiceCode = @InvoiceCode, 
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
                    IsStokPosted = @IsStokPosted,
                    Note = @Note    
                WHERE
                    InvoiceId = @InvoiceId";

            var dp = new DynamicParameters();

            dp.AddParam("@InvoiceId", model.InvoiceId, SqlDbType.VarChar); 
            dp.AddParam("@InvoiceDate", model.InvoiceDate, SqlDbType.DateTime); 
            dp.AddParam("@InvoiceCode", model.InvoiceCode, SqlDbType.VarChar); 
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
            dp.AddParam("@Note", model.Note, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IInvoiceKey key)
        {
            const string sql = @"
                DELETE FROM  
                    BTR_Invoice
                WHERE
                    InvoiceId = @InvoiceId";

            var dp = new DynamicParameters();
            dp.AddParam("@InvoiceId", key.InvoiceId, SqlDbType.VarChar); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public InvoiceModel GetData(IInvoiceKey key)
        {
            const string sql = @"
                SELECT
                    aa.InvoiceId, aa.InvoiceDate, aa.InvoiceCode, aa.SupplierId,  aa.WarehouseId, 
                    aa.NoFakturPajak, aa.DueDate, aa.Total, aa.Disc, aa.Dpp, aa.Tax, aa.GrandTotal,
                    aa.CreateTime, aa.LastUpdate, aa.UserId, aa.VoidDate, aa.UserIdVoid, aa.IsStokPosted, aa.Note,
                    ISNULL(bb.SupplierName, '') AS SupplierName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_Invoice aa
                    LEFT JOIN BTR_SUpplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    InvoiceId = @InvoiceId ";

            var dp = new DynamicParameters();
            dp.AddParam("@InvoiceId", key.InvoiceId, SqlDbType.VarChar); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<InvoiceModel>(sql, dp);
            }
        }

        public IEnumerable<InvoiceModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.InvoiceId, aa.InvoiceDate, aa.InvoiceCode, aa.SupplierId,  aa.WarehouseId, 
                    aa.NoFakturPajak, aa.DueDate, aa.Total, aa.Disc, aa.Dpp, aa.Tax, aa.GrandTotal,
                    aa.CreateTime, aa.LastUpdate, aa.UserId, aa.VoidDate, aa.UserIdVoid, aa.IsStokPosted, aa.Note,
                    ISNULL(bb.SupplierName, '') AS SupplierName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_Invoice aa
                    LEFT JOIN BTR_SUpplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    InvoiceDate BETWEEN @Tgl1 AND @Tgl2
                    AND aa.VoidDate = '3000-01-01'";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime); 
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                var result = conn.Read<InvoiceModel>(sql, dp);
                return result;
            }
        }
    }
}