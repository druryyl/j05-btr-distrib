using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.PurchaseContext.PurchaseOrderAgg.Contracts;
using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.PurchaseOrderAgg
{
    public class PurchaseOrderDal : IPurchaseOrderDal
    {
        private readonly DatabaseOptions _opt;

        public PurchaseOrderDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(PurchaseOrderModel model)
        {
            const string sql = @"
                INSERT INTO BTR_PurchaseOrder(
                    PurchaseOrderId, PurchaseOrderDate, UserId,
                    SupplierId,  WarehouseId)
                VALUES(
                    @PurchaseOrderId, @PurchaseOrderDate, @UserId,
                    @SupplierId,  @WarehouseId)";

            var dp = new DynamicParameters();
            dp.AddParam("@PurchaseOrderId", model.PurchaseOrderId, SqlDbType.VarChar);
            dp.AddParam("@PurchaseOrderDate", model.PurchaseOrderDate, SqlDbType.DateTime);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(PurchaseOrderModel model)
        {
            const string sql = @"
                UPDATE 
                    BTR_PurchaseOrder
                SET
                   PurchaseOrderDate = @PurchaseOrderDate, 
                   UserId = @UserId,
                   SupplierId = @SupplierId,  
                   WarehouseId = @WarehouseId
                WHERE
                    PurchaseOrderId = @PurchaseOrderId";

            var dp = new DynamicParameters();
            dp.AddParam("@PurchaseOrderId", model.PurchaseOrderId, SqlDbType.VarChar);
            dp.AddParam("@PurchaseOrderDate", model.PurchaseOrderDate, SqlDbType.DateTime);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IPurchaseOrderKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_PurchaseOrder
                WHERE
                    PurchaseOrderId = @PurchaseOrderId";

            var dp = new DynamicParameters();
            dp.AddParam("@PurchaseOrderId", key.PurchaseOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public PurchaseOrderModel GetData(IPurchaseOrderKey key)
        {
            const string sql = @"
                SELECT
                    aa.PurchaseOrderId, aa.PurchaseOrderDate, aa.UserId, 
                    aa.SupplierId, aa.WarehouseId,
                    ISNULL(bb.SupplierId, '') AS SupplierName,
                    ISNULL(cc.WarehouseId, '') AS WarehouseName
                FROM
                    BTR_PurchaseOrder aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    aa.PurchaseOrderId = @PurchaseOrderId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@PurchaseOrderId", key.PurchaseOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<PurchaseOrderModel>(sql, dp);
            }
        }

        public IEnumerable<PurchaseOrderModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.PurchaseOrderId, aa.PurchaseOrderDate, aa.UserId, 
                    aa.SupplierId, aa.WarehouseId,
                    ISNULL(bb.SupplierId, '') AS SupplierName,
                    ISNULL(cc.WarehouseId, '') AS WarehouseName
                FROM
                    BTR_PurchaseOrder aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    aa.PurchaseOrderDate BETWEEN @Tgl1 AND @Tgl2 ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PurchaseOrderModel>(sql, dp);
            }
        }

        public IEnumerable<PurchaseOrderModel> ListData(ISupplierKey filter)
        {
            const string sql = @"
                SELECT
                    aa.PurchaseOrderId, aa.PurchaseOrderDate, aa.UserId, 
                    aa.SupplierId, aa.WarehouseId,
                    ISNULL(bb.SupplierId, '') AS SupplierName,
                    ISNULL(cc.WarehouseId, '') AS WarehouseName
                FROM
                    BTR_PurchaseOrder aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    aa.SupplierId = @SupplierId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@SupplierId", filter.SupplierId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PurchaseOrderModel>(sql, dp);
            }
        }
    }
}