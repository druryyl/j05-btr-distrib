using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using btr.application.SalesContext.AlokasiFpAgg;
using btr.domain.SalesContext.AlokasiFpAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.AlokasiFpAgg
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
                aa.NoUrut, aa.FakturId, aa.FakturCode,
                ISNULL(cc.Npwp, '') AS Npwp
            FROM
                BTR_AlokasiFpItem aa
                LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
            WHERE
                AlokasiFpId = @AlokasiFpId ";

            var dp = new DynamicParameters();
            dp.AddParam("@AlokasiFpId", filter.AlokasiFpId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<AlokasiFpItemModel>(sql, dp);
            }
        }

        public AlokasiFpItemModel GetData(INoFakturPajak key)
        {
            const string sql = @"
            SELECT
                aa.AlokasiFpId, aa.NoFakturPajak,
                aa.NoUrut, aa.FakturId, aa.FakturCode,
                ISNULL(cc.Npwp, '') AS Npwp
            FROM
                BTR_AlokasiFpItem aa
                LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
            WHERE
                aa.NoFakturPajak = @NoFakturPajak ";

            var dp = new DynamicParameters();
            dp.AddParam("@NoFakturPajak", key.NoFakturPajak, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<AlokasiFpItemModel>(sql, dp);
            }
        }
    }
}