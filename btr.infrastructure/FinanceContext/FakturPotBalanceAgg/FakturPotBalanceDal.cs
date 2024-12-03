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
using btr.application.FinanceContext.FakturPotBalanceAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Domain;

namespace btr.distrib.FinanceContext.ReturBalanceAgg
{

    public class FakturPotBalanceDal : IFakturPotBalanceDal
    {
        private readonly DatabaseOptions _opt;

        public FakturPotBalanceDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(FakturPotBalanceModel model)
        {
            const string sql = @"
                INSERT INTO BTR_FakturPotBalance(
                    FakturId, NilaiFaktur, NilaiPotong, NilaiSumPost)
                VALUES (
                    @FakturId, @NilaiFaktur, @NilaiPotong, @NilaiSumPost)";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@NilaiFaktur", model.NilaiFaktur, SqlDbType.Decimal);
            dp.AddParam("@NilaiPotong", model.NilaiPotong, SqlDbType.Decimal);
            dp.AddParam("@NilaiSumPost", model.NilaiSumPost, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(FakturPotBalanceModel model)
        {
            const string sql = @"
                UPDATE
                    BTR_FakturPotBalance
                SET
                    NilaiFaktur = @NilaiFaktur,
                    NilaiPotong = @NilaiPotong, 
                    NilaiSumPost = @NilaiSumPost
                WHERE
                    FakturId= @FakturId";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@NilaiFaktur", model.NilaiFaktur, SqlDbType.Decimal);
            dp.AddParam("@NilaiPotong", model.NilaiPotong, SqlDbType.Decimal);
            dp.AddParam("@NilaiSumPost", model.NilaiSumPost, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IFakturKey key)
        {
            const string sql = @"
                DELETE
                    BTR_FakturPotBalance
                WHERE
                    FakturId = @FakturId";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public FakturPotBalanceModel GetData(IFakturKey key)
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.NilaiFaktur, aa.NilaiPotong, aa.NilaiSumPost,
                    ISNULL(bb.FakturDate, '3000-01-01') FakturDate,
                    ISNULL(bb.CustomerId, '') CustomerId,
                    ISNULL(cc.CustomerName, '') CustomerName
                FROM
                    BTR_FakturPotBalance aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                WHERE
                    FakturId = @FakturId";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<FakturPotBalanceModel>(sql, dp);
            }
        }

        public IEnumerable<FakturPotBalanceModel> ListData(ICustomerKey filter)
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.NilaiFaktur, aa.NilaiPotong, aa.NilaiSumPost,
                    ISNULL(bb.FakturDate, '3000-01-01') FakturDate,
                    ISNULL(bb.CustomerId, '') CustomerId,
                    ISNULL(cc.CustomerName, '') CustomerName
                FROM
                    BTR_FakturPotBalance aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                WHERE
                    bb.CustomerId = @CustomerId";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", filter.CustomerId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturPotBalanceModel>(sql, dp);
            }
        }

        public IEnumerable<FakturPotBalanceModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.NilaiFaktur, aa.NilaiPotong, aa.NilaiSumPost,
                    ISNULL(bb.FakturDate, '3000-01-01') FakturDate,
                    ISNULL(bb.CustomerId, '') CustomerId,
                    ISNULL(cc.CustomerName, '') CustomerName
                FROM
                    BTR_FakturPotBalance aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                WHERE
                    bb.FakturDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturPotBalanceModel>(sql, dp);
            }
        }
    }
}
