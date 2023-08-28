using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.WilayahAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.KlasifikasiAgg
{
    public class KlasifikasiDal : IKlasifikasiDal
    {
        private readonly DatabaseOptions _opt;

        public KlasifikasiDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(KlasifikasiModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Klasifikasi(
                KlasifikasiId, KlasifikasiName)
            VALUES (
                @KlasifikasiId, @KlasifikasiName)";

            var dp = new DynamicParameters();
            dp.AddParam("@KlasifikasiId", model.KlasifikasiId, SqlDbType.VarChar);
            dp.AddParam("@KlasifikasiName", model.KlasifikasiName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(KlasifikasiModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Klasifikasi
            SET
                KlasifikasiName = @KlasifikasiName
            WHERE
                KlasifikasiId = @KlasifikasiId ";

            var dp = new DynamicParameters();
            dp.AddParam("@KlasifikasiId", model.KlasifikasiId, SqlDbType.VarChar);
            dp.AddParam("@KlasifikasiName", model.KlasifikasiName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IKlasifikasiKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Klasifikasi
            WHERE
                KlasifikasiId = @KlasifikasiId ";

            var dp = new DynamicParameters();
            dp.AddParam("@KlasifikasiId", key.KlasifikasiId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public KlasifikasiModel GetData(IKlasifikasiKey key)
        {
            const string sql = @"
            SELECT
                KlasifikasiId, KlasifikasiName
            FROM
                BTR_Klasifikasi
            WHERE
                KlasifikasiId = @KlasifikasiId ";

            var dp = new DynamicParameters();
            dp.AddParam("@KlasifikasiId", key.KlasifikasiId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<KlasifikasiModel>(sql, dp);
            }
        }

        public IEnumerable<KlasifikasiModel> ListData()
        {
            const string sql = @"
            SELECT
                KlasifikasiId, KlasifikasiName
            FROM
                BTR_Klasifikasi";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<KlasifikasiModel>(sql);
            }
        }
    }
}