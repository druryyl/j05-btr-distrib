using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.WilayahAgg;
using btr.domain.SalesContext.WilayahAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.WilayahAgg
{
    public class WilayahDal : IWilayahDal
    {
        private readonly DatabaseOptions _opt;

        public WilayahDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(WilayahModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Wilayah(
                WilayahId, WilayahName)
            VALUES (
                @WilayahId, @WilayahName)";

            var dp = new DynamicParameters();
            dp.AddParam("@WilayahId", model.WilayahId, SqlDbType.VarChar);
            dp.AddParam("@WilayahName", model.WilayahName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(WilayahModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Wilayah
            SET
                WilayahName = @WilayahName
            WHERE
                WilayahId = @WilayahId ";

            var dp = new DynamicParameters();
            dp.AddParam("@WilayahId", model.WilayahId, SqlDbType.VarChar);
            dp.AddParam("@WilayahName", model.WilayahName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IWilayahKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Wilayah
            WHERE
                WilayahId = @WilayahId ";

            var dp = new DynamicParameters();
            dp.AddParam("@WilayahId", key.WilayahId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public WilayahModel GetData(IWilayahKey key)
        {
            const string sql = @"
            SELECT
                WilayahId, WilayahName
            FROM
                BTR_Wilayah
            WHERE
                WilayahId = @WilayahId ";

            var dp = new DynamicParameters();
            dp.AddParam("@WilayahId", key.WilayahId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<WilayahModel>(sql, dp);
            }
        }

        public IEnumerable<WilayahModel> ListData()
        {
            const string sql = @"
            SELECT
                WilayahId, WilayahName
            FROM
                BTR_Wilayah";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<WilayahModel>(sql);
            }
        }
    }
}