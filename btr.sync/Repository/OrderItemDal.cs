using Dapper;
using j07_btrade_sync.Model;
using Nuna.Lib.DataAccessHelper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace j07_btrade_sync.Repository
{
    public class OrderItemDal
    {
        public void Insert(IEnumerable<OrderItemType> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();

                bcp.AddMap("OrderId", "OrderId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("BrgCode", "BrgCode");
                bcp.AddMap("BrgName", "BrgName");
                bcp.AddMap("KategoriName", "KategoriName");
                bcp.AddMap("QtyBesar", "QtyBesar");
                bcp.AddMap("SatBesar", "SatBesar");
                bcp.AddMap("QtyKecil", "QtyKecil");
                bcp.AddMap("SatKecil", "SatKecil");
                bcp.AddMap("QtyBonus", "QtyBonus");

                bcp.AddMap("Konversi", "Konversi");
                bcp.AddMap("UnitPrice", "UnitPrice");

                bcp.AddMap("Disc1", "Disc1");
                bcp.AddMap("Disc2", "Disc2");
                bcp.AddMap("Disc3", "Disc3");
                bcp.AddMap("Disc4", "Disc4");

                bcp.AddMap("LineTotal", "LineTotal");

                var fetched = listModel.ToList();
                bcp.DestinationTableName = "dbo.BTR_OrderItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IOrderKey key)
        {
            const string sql = @"
                DELETE FROM BTR_OrderItem
                WHERE OrderId = @OrderId";

            var dp = new DynamicParameters();
            dp.AddParam("OrderId", key.OrderId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<OrderItemType> ListData(IOrderKey filter)
        {
            const string sql = @"
                SELECT 
                    OrderId, NoUrut, BrgId, BrgCode, BrgName, KategoriName, 
                    QtyBesar, SatBesar, QtyKecil, SatKecil, Konversi, 
                    UnitPrice, LineTotal
                FROM BTR_OrderItem
                WHERE OrderId = @OrderId";

            var dp = new DynamicParameters();
            dp.AddParam("OrderId", filter.OrderId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.Read<OrderItemType>(sql, dp);
            }
        }
    }
}
