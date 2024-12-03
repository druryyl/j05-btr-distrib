using btr.application.FinanceContext.FakturPotBalanceAgg;
using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace btr.infrastructure.FinanceContext.ReturBalanceAgg
{
    public class FakturPotBalancePostDal : IFakturPotBalancePostDal
    {
        private readonly DatabaseOptions _opt;

        public FakturPotBalancePostDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<FakturPotBalancePostModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("NoUrut", "HargaTypeId");
                bcp.AddMap("PostDate", "Harga");
                bcp.AddMap("UserId", "HargaTimestamp");
                bcp.AddMap("ReturJualId", "ReturJualId");
                bcp.AddMap("NilaiRetur", "NilaiRetur");
                bcp.AddMap("NilaiPost", "NilaiPost");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_FakturPotBalancePost";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IFakturKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_FakturPotBalancePost
                WHERE
                    FakturId = @FakturId";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<FakturPotBalancePostModel> ListData(IFakturKey filter)
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.NoUrut, aa.PostDate, aa.UserId, aa.ReturJualId, 
                    aa.NilaiRetur, aa.NilaiPost,  
                    ISNULL(bb.ReturJualDate, '3000-01-01') ReturJualDate
                FROM
                    BTR_FakturPotBalancePost aa
                    LEFT JOIN BTR_ReturJual bb ON aa.ReturJualId = bb.ReturJualId
                WHERE
                    aa.FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", filter.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturPotBalancePostModel>(sql, dp);
            }
        }
    }
}
