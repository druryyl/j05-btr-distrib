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
                ;WITH OrdersInRange AS (
                    SELECT o.OrderId, o.OrderDate, o.UserEmail, o.CustomerName, o.CustomerId,
                            SUM(oi.LineTotal) AS OrderTotal
                    FROM BTR_Order o
                    LEFT JOIN BTR_OrderItem oi ON o.OrderId = oi.OrderId
                    WHERE o.OrderDate BETWEEN @Tgl1 AND @Tgl2
                    GROUP BY o.OrderId, o.OrderDate, o.UserEmail, o.CustomerName, o.CustomerId
                ),
                FaktursInRange AS (
                    SELECT f.FakturId, f.FakturCode, f.FakturDate, f.OrderId, f.GrandTotal, f.SalesPersonId, f.CustomerId
                    FROM BTR_Faktur f
                    WHERE f.FakturDate BETWEEN @Tgl1 AND @Tgl2
                )

                -- Rows driven by orders in the date range (attach faktur if present)
                SELECT 
                    ISNULL(o.OrderId, '') AS OrderId,
                    ISNULL(o.OrderDate, '3000-01-01') AS OrderDate,
                    ISNULL(o.OrderTotal, 0) AS OrderTotal,
                    ISNULL(f.FakturCode, '') AS FakturCode,
                    ISNULL(f.FakturDate, '3000-01-01') AS FakturDate,
                    ISNULL(o.CustomerName, c.CustomerName) AS CustomerName,
                    ISNULL(c.CustomerCode, '') AS Code,
                    ISNULL(c.Address1, '') AS Alamat,
                    ISNULL(f.GrandTotal, 0) AS FakturTotal,
                    ISNULL(scs.StatusDate, '3000-01-01') AS OmzetDate,
                    ISNULL(sp.SalesPersonName, o.UserEmail) AS SalesPersonName
                FROM OrdersInRange o
                LEFT JOIN FaktursInRange f ON o.OrderId = f.OrderId
                LEFT JOIN BTR_FakturControlStatus scs ON f.FakturId = scs.FakturId AND scs.StatusFaktur = 2
                LEFT JOIN BTR_SalesPerson sp ON f.SalesPersonId = sp.SalesPersonId
                LEFT JOIN BTR_Customer c ON o.CustomerId = c.CustomerId

                UNION ALL

                -- Faktur-driven rows where the matching order was NOT in the orders result set
                SELECT 
                    ISNULL(f.OrderId, '') AS OrderId,
                    CAST('3000-01-01' AS DATETIME) AS OrderDate,
                    0 AS OrderTotal,
                    ISNULL(f.FakturCode, '') AS FakturCode,
                    ISNULL(f.FakturDate, '3000-01-01') AS FakturDate,
                    ISNULL(c.CustomerName, '') AS CustomerName,
                    ISNULL(c.CustomerCode, '') AS Code,
                    ISNULL(c.Address1, '') AS Alamat,
                    ISNULL(f.GrandTotal, 0) AS FakturTotal,
                    ISNULL(scs.StatusDate, '3000-01-01') AS OmzetDate,
                    ISNULL(sp.SalesPersonName, '') AS SalesPersonName
                FROM FaktursInRange f
                LEFT JOIN BTR_FakturControlStatus scs ON f.FakturId = scs.FakturId AND scs.StatusFaktur = 2
                LEFT JOIN BTR_SalesPerson sp ON f.SalesPersonId = sp.SalesPersonId
                LEFT JOIN BTR_Customer c ON f.CustomerId = c.CustomerId
                WHERE f.OrderId NOT IN (SELECT OrderId FROM OrdersInRange)";

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
