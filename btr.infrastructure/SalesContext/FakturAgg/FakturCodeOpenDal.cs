using btr.application.SalesContext.FakturAgg.Contracts;
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

namespace btr.infrastructure.SalesContext.FakturAgg
{
    public class FakturCodeOpenDal : IFakturCodeOpenDal
    {
        private readonly DatabaseOptions _opt;

        public FakturCodeOpenDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(string model)
        {
            const string sql = @"
                INSERT INTO BTR_FakturCodeOpen (FakturCode) VALUES(@FakturCode)";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturCode", model, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(string key)
        {
            const string sql = @"
                DELETE FROM BTR_FakturCodeOpen WHERE FakturCode = @FakturCode";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturCode", key, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<string> ListData()
        {
            const string sql = @"
                SELECT FakturCode FROM BTR_FakturCodeOpen";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<string>(sql);
            }
        }
    }
}
