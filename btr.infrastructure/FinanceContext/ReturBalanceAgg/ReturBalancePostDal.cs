using btr.application.FinanceContext.ReturBalanceAgg;
using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.InventoryContext.ReturJualAgg;
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
    public class ReturBalancePostDal : IReturBalancePostDal
    {
        private readonly DatabaseOptions _opt;

        public ReturBalancePostDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<ReturBalancePostModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("ReturJualId", "BrgId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("PostDate", "PostDate");
                bcp.AddMap("UserId", "UserId");
                bcp.AddMap("FakturId","FakturId");
                bcp.AddMap("NilaiFaktur","NilaiFaktur");
                bcp.AddMap("NilaiPotong","NilaiPotong");
                bcp.AddMap("NilaiPost", "NilaiPost");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_ReturBalancePost";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IReturJualKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_ReturBalancePost
                WHERE
                    ReturJualId = @ReturJualId";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<ReturBalancePostModel> ListData(IReturJualKey filter)
        {
            const string sql = @"
                SELECT
                    aa.ReturJualId, aa.NoUrut, aa.PostDate, aa.UserId, aa.FakturId, 
                    aa.NilaiFaktur, aa.NilaiPotong,  aa.NilaiPost,  
                    ISNULL(bb.FakturCode, '') FakturCode,
                    ISNULL(bb.FakturDate, '3000-01-01') FakturDate
                FROM
                    BTR_ReturBalancePost aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                WHERE
                    aa.ReturJualId = @ReturJualId ";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", filter.ReturJualId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturBalancePostModel>(sql, dp);
            }
        }
    }
}
