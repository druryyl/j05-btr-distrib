using btr.domain.BrgContext.JenisBrgAgg;
using btr.infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using btr.nuna.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using btr.application.BrgContext.JenisBrgAgg;

namespace btr.infrastructure.BrgContext.JenisBrgAgg
{
    public class JenisBrgDal : IJenisBrgDal
    {
        private readonly DatabaseOptions _opt;

        public JenisBrgDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(JenisBrgModel model)
        {
            const string sql = @"
                INSERT INTO BTR_JenisBrg (JenisBrgId, JenisBrgName)
                VALUES (@JenisBrgId, @JenisBrgName)";

            var dp = new DynamicParameters();
            dp.AddParam("@JenisBrgId", model.JenisBrgId, SqlDbType.VarChar);
            dp.AddParam("@JenisBrgName", model.JenisBrgName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(JenisBrgModel model)
        {
            const string sql = @"
                UPDATE  
                    BTR_JenisBrg 
                SET     
                    JenisBrgName = @JenisBrgName
                WHERE
                    JenisBrgId= @JenisBrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@JenisBrgId", model.JenisBrgId, SqlDbType.VarChar);
            dp.AddParam("@JenisBrgName", model.JenisBrgName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IJenisBrgKey key)
        {
            const string sql = @"
                DELETE FROM  
                    BTR_JenisBrg 
                WHERE
                    JenisBrgId= @JenisBrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@JenisBrgId", key.JenisBrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public JenisBrgModel GetData(IJenisBrgKey key)
        {
            const string sql = @"
                SELECT
                    JenisBrgId, JenisBrgName
                FROM
                    BTR_JenisBrg 
                WHERE
                    JenisBrgId= @JenisBrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@JenisBrgId", key.JenisBrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<JenisBrgModel>(sql, dp);
            }
        }

        public IEnumerable<JenisBrgModel> ListData()
        {
            const string sql = @"
                SELECT
                    JenisBrgId, JenisBrgName
                FROM
                    BTR_JenisBrg  ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<JenisBrgModel>(sql);
            }
        }
    }
}
