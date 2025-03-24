using btr.application.SalesContext.RuteAgg;
using btr.domain.SalesContext.RuteAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SalesContext.RuteAgg
{
    public class RuteDal : IRuteDal
    {
        private readonly DatabaseOptions _opt;

        public RuteDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(RuteModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Rute (
                    RuteId, RuteCode, RuteName)
                VALUES (
                    @RuteId, @RuteCode, @RuteName)";

            var dp = new DynamicParameters();
            dp.AddParam("@RuteId", model.RuteId, SqlDbType.VarChar);
            dp.AddParam("@RuteCode", model.RuteCode, SqlDbType.VarChar);
            dp.AddParam("@RuteName", model.RuteName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(RuteModel model)
        {
            const string sql = @"
                UPDATE BTR_Rute
                SET
                    RuteCode = @RuteCode,
                    RuteName = @RuteName
                WHERE
                    RuteId = @RuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@RuteId", model.RuteId, SqlDbType.VarChar);
            dp.AddParam("@RuteCode", model.RuteCode, SqlDbType.VarChar);
            dp.AddParam("@RuteName", model.RuteName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IRuteKey key)
        {
            const string sql = @"
                DELETE FROM BTR_Rute
                WHERE RuteId = @RuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@RuteId", key.RuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public RuteModel GetData(IRuteKey key)
        {
            const string sql = @"
                SELECT
                    RuteId, RuteCode, RuteName
                FROM BTR_Rute
                WHERE RuteId = @RuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@RuteId", key.RuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<RuteModel>(sql, dp).FirstOrDefault();
            }
        }

        public IEnumerable<RuteModel> ListData()
        {
            const string sql = @"
                SELECT
                    RuteId, RuteCode, RuteName
                FROM BTR_Rute";


            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<RuteModel>(sql);
            }
        }
    }
}
