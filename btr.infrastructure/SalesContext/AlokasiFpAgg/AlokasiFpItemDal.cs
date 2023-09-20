using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using btr.application.SalesContext.NomorFpAgg;
using btr.domain.SalesContext.FakturPajak;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.AlokasiFpAgg
{
    public class AlokasiFpItemDal : IAlokasiFpItemDal
    {
        private readonly DatabaseOptions _opt;

        public AlokasiFpItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<AlokasiFpItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("AlokasiFpId", "AlokasiFpId");
                bcp.AddMap("NoFakturPajak", "NoFakturPajak");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("FakturCode", "FakturCode");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_AlokasiFpItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IAlokasiFpKey key)
        {
            const string sql = @"
            DELETE FROM
                BTR_AlokasiFpItem
            WHERE
                AlokasiFpId = @AlokasiFpId";

            var dp = new DynamicParameters();
            dp.AddParam("@AlokasiFpId", key.AlokasiFpId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<AlokasiFpItemModel> ListData(IAlokasiFpKey filter)
        {
            const string sql = @"
            SELECT
                aa.AlokasiFpId, aa.NoFakturPajak,
                aa.NoUrut, aa.FakturId, aa.FakturCode
            FROM
                BTR_AlokasiFpItem aa
            WHERE
                AlokasiFpId = @AlokasiFpId ";

            var dp = new DynamicParameters();
            dp.AddParam("@AlokasiFpId", filter.AlokasiFpId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<AlokasiFpItemModel>(sql, dp);
            }
        }
    }
}