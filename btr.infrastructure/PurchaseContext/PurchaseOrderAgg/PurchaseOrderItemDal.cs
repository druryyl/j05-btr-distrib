using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using btr.domain.PurchaseContext.PurchaseOrderAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.PurchaseOrderAgg
{
    public class PurchaseOrderItemDal
    {
        private readonly DatabaseOptions _opt;

        public PurchaseOrderItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<PurchaseOrderItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("PurchaseOrderId", "PurchaseOrderId");
                bcp.AddMap("PurchaseOrderItemId", "PurchaseOrderItemId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("Qty", "Qty");
                bcp.AddMap("Satuan", "Satuan");
                bcp.AddMap("Harga", "Harga");
                bcp.AddMap("SubTotal", "SubTotal");
                bcp.AddMap("DiskonProsen", "DiskonProsen");
                bcp.AddMap("DiskonRp", "DiskonRp");
                bcp.AddMap("TaxProsen", "TaxProsen");
                bcp.AddMap("TaxRp", "TaxRp");
                bcp.AddMap("Total", "Total");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_PurchaseOrderItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IPurchaseOrderKey key)
        {
            const string sql = @"
            DELETE FROM
                BTR_PurchaseOrderItem
            WHERE
                PurchaseOrderId = @PurchaseOrderId";

            var dp = new DynamicParameters();
            dp.AddParam("@PurchaseOrderId", key.PurchaseOrderId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<PurchaseOrderItemModel> ListData(IPurchaseOrderKey filter)
        {
            const string sql = @"
            SELECT
                aa.PurchaseOrderId, aa.PurchaseOrderItemId,
                aa.NoUrut, aa.BrgId, aa.Qty, aa.Satuan,
                aa.Harga, aa.SubTotal, aa.DiskonProsen,
                aa.DiskonRp, aa.TaxProsen, aa.TaxRp, aa.Total,
                ISNULL(bb.BrgName, '') AS BrgName
            FROM
                BTR_PurchaseOrderItem aa
                LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
            WHERE
                PurchaseOrderId = @PurchaseOrderId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PurchaseOrderId", filter.PurchaseOrderId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PurchaseOrderItemModel>(sql, dp);
            }
        }    }
}