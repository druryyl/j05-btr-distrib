using btr.application.SalesContext.RuteAgg;
using btr.domain.SalesContext.HariRuteAgg;
using btr.infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Infrastructure;
using System.Data.SqlClient;

namespace btr.infrastructure.SalesContext.HariRuteAgg
{
    public class HariRuteDal : IHariRuteDal
    {
        private readonly DatabaseOptions _opt;

        public HariRuteDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(HariRuteModel model)
        {
            const string sql = @"
                INSERT INTO BTR_HariRute (
                    HariRuteId, HariRuteName, ShortName)
                VALUES (
                    @HariRuteId, @HariRuteName, @ShortName)"
            ;

            var dp = new DynamicParameters();
            dp.AddParam("@HariRuteId", model.HariRuteId, SqlDbType.VarChar); 
            dp.AddParam("@HariRuteName", model.HariRuteName, SqlDbType.VarChar); 
            dp.AddParam("@ShortName", model.ShortName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(HariRuteModel model)
        {
            const string sql = @"
                UPDATE 
                    BTR_HariRute
                SET
                    @HariRuteId = HariRuteId, 
                    @HariRuteName = HariRuteName, 
                    @ShortName = ShortName
                WHERE
                    HariRuteId = @HariRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@HariRuteId", model.HariRuteId, SqlDbType.VarChar);
            dp.AddParam("@HariRuteName", model.HariRuteName, SqlDbType.VarChar);
            dp.AddParam("@ShortName", model.ShortName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IHariRuteKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_HariRute
                WHERE
                    HariRuteId = @HariRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@HariRuteId", key.HariRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public HariRuteModel GetData(IHariRuteKey key)
        {
            const string sql = @"
                SELECT
                    HariRuteId, HariRuteName, ShortName
                FROM
                    BTR_HariRute
                WHERE
                    HariRuteId = @HariRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@HariRuteId", key.HariRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<HariRuteModel>(sql, dp);
            }
        }

        public IEnumerable<HariRuteModel> ListData()
        {
            const string sql = @"
                SELECT
                    HariRuteId, HariRuteName, ShortName
                FROM
                    BTR_HariRute ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<HariRuteModel>(sql);
            }
        }
    }
}
