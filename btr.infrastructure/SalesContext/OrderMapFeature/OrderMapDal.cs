using btr.application.SalesContext.OrderMapFeature;
using btr.domain.SalesContext.OrderAgg;
using btr.domain.SalesContext.OrderStatusFeature;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.nuna.Infrastructure;

namespace btr.infrastructure.SalesContext.OrderMapFeature
{
    public class OrderMapDal : IOrderMapDal
    {
        private readonly DatabaseOptions _opt;

        public OrderMapDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(OrderMapModel model)
        {
            const string sql = @"
            INSERT INTO BTR_OrderMap(
                OrderId, FakturId, 
                FakturCode, UserName, Timestamp)
            VALUES (
                @OrderId, @FakturId, 
                @FakturCode, @UserName, @Timestamp)";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", model.OrderId, SqlDbType.VarChar);
            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@UserName", model.UserName, SqlDbType.VarChar);
            dp.AddParam("@Timestamp", model.Timestamp, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(OrderMapModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_OrderMap
            SET
                FakturId = @FakturId,
                FakturCode = @FakturCode,
                UserName = @UserName,
                Timestamp = @Timestamp
            WHERE
                OrderId = @OrderId ";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", model.OrderId, SqlDbType.VarChar);
            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@UserName", model.UserName, SqlDbType.VarChar);
            dp.AddParam("@Timestamp", model.Timestamp, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IOrderKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_OrderMap
            WHERE
                OrderId = @OrderId ";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", key.OrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public OrderMapModel GetData(IOrderKey key)
        {
            const string sql = @"
            SELECT
                aa.OrderId, aa.FakturId, aa.FakturCode, 
                aa.UserName, aa.Timestamp,
                cc.FakturDate, cc.GrandTotal AS NilaiFaktur
            FROM
                BTR_OrderMap aa
                LEFT JOIN BTR_ORDER bb ON aa.OrderId = bb.OrderId
                LEFT JOIN BTR_Faktur cc ON aa.FakturId = cc.FakturId
            WHERE
                aa.OrderId = @OrderId ";

            var dp = new DynamicParameters();
            dp.AddParam("@OrderId", key.OrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<OrderMapModel>(sql, dp);
            }
        }

        public IEnumerable<OrderMapModel> ListData(Periode periode)
        {
            const string sql = @"
            SELECT
                aa.OrderId, aa.FakturId, aa.FakturCode, 
                aa.UserName, aa.Timestamp,
                cc.FakturDate, cc.GrandTotal AS NilaiFaktur
            FROM
                BTR_OrderMap aa
                LEFT JOIN BTR_ORDER bb ON aa.OrderId = bb.OrderId
                LEFT JOIN BTR_Faktur cc ON aa.FakturId = cc.FakturId
            WHERE 
                bb.OrderDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", periode.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<OrderMapModel>(sql, dp);
            }
        }
    }
}
