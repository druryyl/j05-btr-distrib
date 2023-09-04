using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.SupportContext.ParamUserAgg;
using btr.domain.SupportContext.ParamUser;
using btr.domain.SupportContext.UserAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SupportContext.ParamUserAgg
{
    public class ParamUserDal : IParamUserDal
    {
        private readonly DatabaseOptions _opt;

        public ParamUserDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<ParamUserModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("UserId", "UserId");
                bcp.AddMap("ParamKey", "ParamKey");
                bcp.AddMap("ParamVal", "ParamVal");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_ParamUser";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IUserKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_ParamUser
                WHERE
                    UserId = @UserId ";

            var dp = new DynamicParameters();
            dp.AddParam("@UserId", key.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<ParamUserModel> ListData(IUserKey UserKey)
        {
            const string sql = @"
                SELECT
                    aa.UserId, aa.ParamKey, aa.ParamVal
                FROM 
                    BTR_ParamUser aa
                WHERE
                    aa.UserId = @UserId ";

            var dp = new DynamicParameters();
            dp.AddParam("@UserId", UserKey.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ParamUserModel>(sql, dp);
            }
        }
    }
}