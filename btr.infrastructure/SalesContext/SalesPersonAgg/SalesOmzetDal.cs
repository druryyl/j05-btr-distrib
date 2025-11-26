using btr.application.SalesContext.OrderFeature;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace btr.infrastructure.SalesContext.SalesPersonAgg
{
    public class SalesOmzetDal : ISalesOmzetDal
    {
        private readonly DatabaseOptions _opt;

        public SalesOmzetDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<SalesOmzetView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT 
                    ISNULL(aa.OrderId, '') OrderId, 
                    CAST(ISNULL(aa.OrderDate, '3000-01-01') AS DATETIME) OrderDate, 
                    ISNULL(aa.OrderTotal, 0) OrderTotal,
                    ISNULL(bb.FakturCode, '') FakturCode, ISNULL(bb.FakturDate, '3000-01-01') FakturDate, 
                    ISNULL(aa.CustomerName, ee.CustomerName) AS CustomerName,
                    ISNULL(bb.GrandTotal,0) FakturTotal,
                    ISNULL(cc.StatusDate, '3000-01-01') AS OmzetDate,
                    ISNULL(dd.SalesPersonName, aa.UserEmail) AS SalesPersonName
                FROM
                    (
                        SELECT aa.OrderId, aa.OrderDate, aa.UserEmail, 
                                aa.CustomerName,
                                SUM(bb.LineTotal) OrderTotal
                        FROM BTR_Order aa
                        LEFT JOIN BTR_OrderItem bb ON aa.OrderId = bb.OrderId
                        GROUP BY aa.OrderId, aa.OrderDate, aa.UserEmail, aa.CustomerName
                    ) aa
                    FULL JOIN BTR_Faktur bb ON aa.OrderId = bb.OrderId 
                    LEFT JOIN BTR_FakturControlStatus cc ON bb.FakturId = cc.FakturId AND StatusFaktur = 2
                    LEFT JOIN BTR_SalesPerson dd ON bb.SalesPersonId = dd.SalesPersonId
                    LEFT JOIN BTR_Customer ee ON bb.CustomerId = ee.CustomerId
                WHERE
                    bb.FakturDate BETWEEN @Tgl1 AND @Tgl2
                    OR aa.OrderDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, System.Data.SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, System.Data.SqlDbType.DateTime);

            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<SalesOmzetView>(sql, dp);
            }
        }
    }
}
