using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.NomorFpAgg;
using btr.domain.SalesContext.FakturPajak;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.KlasifikasiAgg
{
    public class AlokasiFpDal : IAlokasiFpDal
    {
        private readonly DatabaseOptions _opt;

        public AlokasiFpDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(AlokasiFpModel model)
        {
            const string sql = @"
            INSERT INTO BTR_AlokasiFp(
                AlokasiFpId, AlokasiFpDate, 
                NoAwal, NoAKhir, Kapasitas, Sisa)
            VALUES (
                @AlokasiFpId, @AlokasiFpDate, 
                @NoAwal, @NoAKhir, @Kapasitas, @Sisa)";

            var dp = new DynamicParameters();
            dp.AddParam("@AlokasiFpId", model.AlokasiFpId, SqlDbType.VarChar);
            dp.AddParam("@AlokasiFpDate", model.AlokasiFpDate, SqlDbType.DateTime);
            dp.AddParam("@NoAwal", model.NoAwal, SqlDbType.VarChar);
            dp.AddParam("@NoAkhir", model.NoAkhir, SqlDbType.VarChar);
            dp.AddParam("@Kapasitas", model.Kapasitas, SqlDbType.Int);
            dp.AddParam("@Sisa", model.Sisa, SqlDbType.Int);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(AlokasiFpModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_AlokasiFp
            SET
                AlokasiFpDate = @AlokasiFpDate,
                NoAwal = @NoAwal,
                NoAkhir = @NoAkhir,
                Kapasitas = @Kapasitas
                Sisa = @Sisa
            WHERE
                AlokasiFpId = @AlokasiFpId ";

            var dp = new DynamicParameters();
            dp.AddParam("@AlokasiFpId", model.AlokasiFpId, SqlDbType.VarChar);
            dp.AddParam("@AlokasiFpDate", model.AlokasiFpDate, SqlDbType.VarChar);
            dp.AddParam("@NoAwal", model.NoAwal, SqlDbType.VarChar);
            dp.AddParam("@NoAkhir", model.NoAkhir, SqlDbType.VarChar);
            dp.AddParam("@Kapasitas", model.Kapasitas, SqlDbType.Int);
            dp.AddParam("@Sisa", model.Sisa, SqlDbType.Int);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IAlokasiFpKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_AlokasiFp
            WHERE
                AlokasiFpId = @AlokasiFpId ";

            var dp = new DynamicParameters();
            dp.AddParam("@AlokasiFpId", key.AlokasiFpId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public AlokasiFpModel GetData(IAlokasiFpKey key)
        {
            const string sql = @"
            SELECT
                AlokasiFpId, AlokasiFpDate, NoAwal, NoAkhir, 
                Kapasitas, Sisa
            FROM
                BTR_AlokasiFp
            WHERE
                AlokasiFpId = @AlokasiFpId ";

            var dp = new DynamicParameters();
            dp.AddParam("@AlokasiFpId", key.AlokasiFpId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<AlokasiFpModel>(sql, dp);
            }
        }

        public IEnumerable<AlokasiFpModel> ListData(Periode filter)
        {
            const string sql = @"
            SELECT
                AlokasiFpId, AlokasiFpDate, NoAwal, NoAkhir, 
                Kapasitas, Sisa
            FROM
                BTR_AlokasiFp
            WHERE
                AlokasiFpDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<AlokasiFpModel>(sql, dp);
            }
        }
    }
}