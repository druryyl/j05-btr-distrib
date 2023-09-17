using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using btr.application.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.PackingFakturAgg
{
    public class PackingFakturDal : IPackingFakturDal
    {
        private readonly DatabaseOptions _opt;

        public PackingFakturDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<PackingFakturModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("PackingId", "PackingId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("FakturId", "FakturId");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_PackingFaktur";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IPackingKey key)
        {
            const string sql = @"
            DELETE FROM
                BTR_PackingFaktur
            WHERE
                PackingId = @PackingId";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", key.PackingId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<PackingFakturModel> ListData(IPackingKey filter)
        {
            const string sql = @"
            SELECT
                aa.PackingId, aa.FakturId, aa.NoUrut, 
                ISNULL(bb.GrandTotal, 0) AS GrandTotal,
                ISNULL(bb.FakturCode, '') AS FakturCode,
                ISNULL(cc.CustomerName, '') AS CustomerName,
                ISNULL(cc.Address1, '') AS Address,
                ISNULL(cc.Kota, '') AS Kota
            FROM
                BTR_PackingFaktur aa
                LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
            WHERE
                PackingId = @PackingId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", filter.PackingId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PackingFakturModel>(sql, dp);
            }
        }
    }
}