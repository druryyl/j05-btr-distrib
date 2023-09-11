using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.SalesContext.FakturControlAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturControlAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.FakturAgg
{
    public class FakturControlStatusDal : IFakturControlStatusDal
    {
        private readonly DatabaseOptions _opt;

        public FakturControlStatusDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<FakturControlStatusModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("FakturDate", "FakturDate"); 
                bcp.AddMap("StatusFaktur", "StatusFaktur");
                bcp.AddMap("StatusDate", "StatusDate");
                bcp.AddMap("Keterangan", "Keterangan");
                bcp.AddMap("UserId", "UserId");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_FakturControlStatus";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IFakturKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_FakturControlStatus
            WHERE
                FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<FakturControlStatusModel> ListData(IFakturKey fakturKey)
        {
            const string sql = @"
            SELECT
                aa.FakturId, aa.FakturDate, aa.StatusFaktur, aa.StatusDate,
                aa.Keterangan, UserId
            FROM 
                BTR_FakturControlStatus aa
            WHERE
                aa.FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", fakturKey.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturControlStatusModel>(sql, dp);
            }
        }

        public IEnumerable<FakturControlStatusModel> ListData(Periode filter)
        {
            const string sql = @"
            SELECT
                aa.FakturId, aa.FakturDate, aa.StatusFaktur, aa.StatusDate,
                aa.Keterangan, UserId
            FROM 
                BTR_FakturControlStatus aa
            WHERE
                aa.FakturDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturControlStatusModel>(sql, dp);
            }
        }
    }
}