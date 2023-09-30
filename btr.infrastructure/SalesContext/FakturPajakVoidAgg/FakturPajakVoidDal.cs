using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturPajakVoidAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturPajakVoidAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.FakturPajakVoidAgg
{
    public class FakturPajakVoidDal : IFakturPajakVoidDal
    {
        private readonly DatabaseOptions _opt;

        public FakturPajakVoidDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(FakturPajakVoidModel model)
        {
            const string sql = @"
            INSERT INTO BTR_FakturPajakVoid(
                NoFakturPajak, VoidDate, AlasanVoid, UserId)
            VALUES (
                @NoFakturPajak, @VoidDate, @AlasanVoid, @UserId)";

            var dp = new DynamicParameters();
            dp.AddParam("@NoFakturPajak", model.NoFakturPajak, SqlDbType.VarChar);
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime);
            dp.AddParam("@AlasanVoid", model.AlasanVoid, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(FakturPajakVoidModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_FakturPajakVoid
            SET
                VoidDate = @VoidDate,
                AlasanVoid = @AlasanVoid,
                UserId = @UserId
            WHERE
                NoFakturPajak = @NoFakturPajak";

            var dp = new DynamicParameters();
            dp.AddParam("@NoFakutPajak", model.NoFakturPajak, SqlDbType.VarChar);
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime);
            dp.AddParam("@AlasanVoid", model.AlasanVoid, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(INoFakturPajak key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_FakturPajakVoid
            WHERE
                NoFakturPajak = @NoFakturPajak ";

            var dp = new DynamicParameters();
            dp.AddParam("@NoFakturPajak", key.NoFakturPajak, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public FakturPajakVoidModel GetData(INoFakturPajak key)
        {
            const string sql = @"
            SELECT
                NoFakturPajak, VoidDate, AlasanVoid, UserId
            FROM
                BTR_FakturPajakVoid
            WHERE
                NoFakturPajak = @NoFakturPajak ";

            var dp = new DynamicParameters();
            dp.AddParam("@NoFakturPajak", key.NoFakturPajak, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<FakturPajakVoidModel>(sql, dp);
            }
        }

        public IEnumerable<FakturPajakVoidModel> ListData(Periode filter)
        {
            const string sql = @"
            SELECT
                NoFakturPajak, VoidDate, AlasanVoid, UserId
            FROM
                BTR_FakturPajakVoid
            WHERE
                VoidDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturPajakVoidModel>(sql, dp);
            }
        }
    }
}