using Dapper;
using j07_btrade_sync.Model;
using Nuna.Lib.DataAccessHelper;
using Nuna.Lib.ValidationHelper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace j07_btrade_sync.Repository
{
    public class OrderDal
    {
        public void Insert(OrderModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Order(
                    OrderId, OrderLocalId, CustomerId, CustomerCode, CustomerName, CustomerAddress,
                    OrderDate, SalesId, SalesName, TotalAmount, UserEmail, StatusSync, FakturCode, OrderNote)
                VALUES (
                    @OrderId, @OrderLocalId, @CustomerId, @CustomerCode, @CustomerName, @CustomerAddress,
                    @OrderDate, @SalesId, @SalesName, @TotalAmount, @UserEmail, @StatusSync, @FakturCode, @OrderNote)";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", model.OrderId, SqlDbType.VarChar);
            dp.AddParam("@OrderLocalId", model.OrderLocalId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", model.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@CustomerAddress", model.CustomerAddress, SqlDbType.VarChar);
            dp.AddParam("@OrderDate", model.OrderDate, SqlDbType.VarChar);
            dp.AddParam("@SalesId", model.SalesId, SqlDbType.VarChar);
            dp.AddParam("@SalesName", model.SalesName, SqlDbType.VarChar);
            dp.AddParam("@TotalAmount", model.TotalAmount, SqlDbType.Decimal);
            dp.AddParam("@UserEmail", model.UserEmail, SqlDbType.VarChar);
            dp.AddParam("@StatusSync", model.StatusSync, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@OrderNote", model.OrderNote, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
            
        }

        public void Update(OrderModel model)
        {
            const string sql = @"
            UPDATE
                BTR_Order
            SET
                OrderLocalId = @OrderLocalId,
                CustomerId = @CustomerId,
                CustomerCode = @CustomerCode,
                CustomerName = @CustomerName,
                CustomerAddress = @CustomerAddress,
                OrderDate = @OrderDate,
                SalesId = @SalesId,
                SalesName = @SalesName,
                TotalAmount = @TotalAmount,
                UserEmail = @UserEmail,
                StatusSync = @StatusSync,
                FakturId = @FakturCode,
                OrderNote = @OrderNote
            WHERE
                OrderId = @OrderId";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", model.OrderId, SqlDbType.VarChar);
            dp.AddParam("@OrderLocalId", model.OrderLocalId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", model.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@CustomerAddress", model.CustomerAddress, SqlDbType.VarChar);
            dp.AddParam("@OrderDate", model.OrderDate, SqlDbType.VarChar);
            dp.AddParam("@SalesId", model.SalesId, SqlDbType.VarChar);
            dp.AddParam("@SalesName", model.SalesName, SqlDbType.VarChar);
            dp.AddParam("@TotalAmount", model.TotalAmount, SqlDbType.Decimal);
            dp.AddParam("@UserEmail", model.UserEmail, SqlDbType.VarChar);
            dp.AddParam("@StatusSync", model.StatusSync, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@OrderNote", model.OrderNote, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IOrderKey key)
        {
            const string sql = @"
            DELETE FROM
                BTR_Order
            WHERE
                OrderId = @OrderId";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", key.OrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete()
        {
            const string sql = @"
            DELETE FROM
                BTR_Order";

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql);
            }
        }

        public OrderModel GetData(IOrderKey key)
        {
            const string sql = @"
            SELECT
                OrderId, OrderLocalId, CustomerId, CustomerCode, CustomerName, CustomerAddress,
                OrderDate, SalesId, SalesName, TotalAmount, UserEmail, StatusSync, FakturCode, OrderNote
            FROM
                BTR_Order
            WHERE
                OrderId = @OrderId";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", key.OrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.ReadSingle<OrderModel>(sql, dp);
            }
        }

        public IEnumerable<OrderModel> ListData(Periode periode) 
        {
            const string sql = @"
            SELECTOrd
                OrderId, OrderLocalId, CustomerId, CustomerCode, CustomerName, CustomerAddress,
                OrderDate, SalesId, SalesName, TotalAmount, UserEmail, StatusSync, FakturCode, OrderNote
            FROM
                BTR_Order
            WHERE 
                OrderDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", periode.Tgl1.ToString("yyyy-MM-dd"), SqlDbType.VarChar);
            dp.AddParam("@Tgl2", periode.Tgl2.ToString("yyyy-MM-dd"), SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.Read<OrderModel>(sql, dp);
            }
        }
    }
}
