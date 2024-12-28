using btr.application.SupportContext.ParamSistemAgg;
using btr.domain.SupportContext.ParamSistemAgg;
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

namespace btr.infrastructure.SupportContext.ParamSistemAgg
{
    public class ParamSistemDal : IParamSistemDal
    {
        private readonly DatabaseOptions _opt;

        public ParamSistemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(ParamSistemModel model)
        {
            const string sql = @"
                INSERT INTO BTR_ParamSistem
                    (ParamCode, ParamValue)
                VALUES
                    (@ParamCode, @ParamValue)";

            var dp = new DynamicParameters();
            dp.AddParam("@ParamCode", model.ParamCode, SqlDbType.VarChar);
            dp.AddParam("@ParamValue", model.ParamValue, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(ParamSistemModel model)
        {
            //  update
            const string sql = @"
                UPDATE BTR_ParamSistem
                SET
                    ParamValue = @ParamValue
                WHERE
                    ParamCode = @ParamCode";
            var dp = new DynamicParameters();
            dp.AddParam("@ParamCode", model.ParamCode, SqlDbType.VarChar);
            dp.AddParam("@ParamValue", model.ParamValue, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ParamSistemModel key)
        {
            //  delete
            const string sql = @"
                DELETE FROM BTR_ParamSistem
                WHERE
                    ParamCode = @ParamCode";
            var dp = new DynamicParameters();
            dp.AddParam("@ParamCode", key.ParamCode, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public ParamSistemModel GetData(IParamSistemKey key)
        {
            //  get data
            const string sql = @"
                SELECT
                    ParamCode, ParamValue
                FROM
                    BTR_ParamSistem
                WHERE
                    ParamCode = @ParamCode";
            var dp = new DynamicParameters();
            dp.AddParam("@ParamCode", key.ParamCode, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<ParamSistemModel>(sql, dp).FirstOrDefault();
            }
        }

        public IEnumerable<ParamSistemModel> ListData()
        {
            //  list data
            const string sql = @"
                SELECT
                    ParamCode, ParamValue
                FROM
                    BTR_ParamSistem";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<ParamSistemModel>(sql);
            }
        }
    }
}
