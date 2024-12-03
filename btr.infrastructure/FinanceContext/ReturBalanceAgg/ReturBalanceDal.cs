using btr.application.FinanceContext.ReturBalanceAgg;
using btr.domain.FinanceContext.ReturBalanceAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using btr.nuna.Infrastructure;
using System.Data;
using System.Data.SqlClient;

namespace btr.distrib.FinanceContext.ReturBalanceAgg
{

    public class ReturBalanceDal : IReturBalanceDal
    {
        private readonly DatabaseOptions _opt;

        public ReturBalanceDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(ReturBalanceModel model)
        {
            const string sql = @"
                INSERT INTO BTR_ReturBalance(
                    ReturJualId, NilaiRetur, NilaiSumPost)
                VALUES (
                    @ReturJualId,  @NilaiRetur, @NilaiSumPost)";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", model.ReturJualId, SqlDbType.VarChar);
            dp.AddParam("@NilaiRetur", model.NilaiRetur, SqlDbType.Decimal);
            dp.AddParam("@NilaiSumPost", model.NilaiSumPost, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(ReturBalanceModel model)
        {
            const string sql = @"
                UPDATE
                    BTR_ReturBalance
                SET
                    NilaiRetur = @NilaiRetur, 
                    NilaiSumPost = @NilaiSumPost
                WHERE
                    ReturJualId = @ReturJualId";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", model.ReturJualId, SqlDbType.VarChar);
            dp.AddParam("@NilaiRetur", model.NilaiRetur, SqlDbType.Decimal);
            dp.AddParam("@NilaiSumPost", model.NilaiSumPost, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IReturJualKey key)
        {
            const string sql = @"
                DELETE
                    BTR_ReturBalance
                WHERE
                    ReturJualId = @ReturJualId";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public ReturBalanceModel GetData(IReturJualKey key)
        {
            const string sql = @"
                SELECT
                    aa.ReturJualId, NilaiRetur, NilaiSumPost,
                    ISNULL(bb.ReturJualDate, '3000-01-01') ReturJualDate,
                    ISNULL(bb.CustomerId, '') CustomerId,
                    ISNULL(cc.CustomerName, '') CustomerName
                FROM
                    BTR_ReturBalance aa
                    LEFT JOIN BTR_ReturJual bb ON aa.ReturJualId = bb.ReturJualId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                WHERE
                    ReturJualId = @ReturJualId";

            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<ReturBalanceModel>(sql, dp);
            }
        }

        public IEnumerable<ReturBalanceModel> ListData(ICustomerKey filter)
        {
            const string sql = @"
                SELECT
                    aa.ReturJualId, NilaiRetur, NilaiSumPost,
                    ISNULL(bb.ReturJualDate, '3000-01-01') ReturJualDate,
                    ISNULL(bb.CustomerId, '') CustomerId,
                    ISNULL(cc.CustomerName, '') CustomerName
                FROM
                    BTR_ReturBalance aa
                    LEFT JOIN BTR_ReturJual bb ON aa.ReturJualId = bb.ReturJualId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                WHERE
                    bb.CustomerId = @CustomerId";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", filter.CustomerId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturBalanceModel>(sql, dp);
            }
        }
    }
}
