using System.Data;
using System.Data.SqlClient;
using btr.nuna.Application;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.Helpers
{
    public class ParamNoDal : INunaCounterDal
    {
        private readonly DatabaseOptions _opt;

        public ParamNoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public string GetNewHexNumber(string prefix)
        {
            const string sql = @"
            SELECT
                Prefix, HexVal
            FROM
                BTR_ParamNo
            WHERE
                Prefix = @Prefix ";

            var dp = new DynamicParameters();
            dp.AddParam("@Prefix", prefix, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                var dr = conn.ReadSingle<ParamNoDto>(sql, dp);
                if (dr is null)
                    return "1";
                return dr.HexVal;
            }
        }

        public void UpdateNewHexNumber(string prefix, string hexValue)
        {
            const string sql = @"
            UPDATE
                BTR_ParamNo
            SET
                HexVal = @HexVal
            WHERE
                Prefix = @Prefix ";

            var dp = new DynamicParameters();
            dp.AddParam("@Prefix", prefix, SqlDbType.VarChar);
            dp.AddParam("@HexVal", hexValue, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void InsertNewHexNumber(string prefix, string hexValue)
        {
            const string sql = @"
            INSERT INTO 
                BTR_ParamNo (
                    Prefix, HexVal)
            VALUES (
                    @Prefix, @HexVal)";

            var dp = new DynamicParameters();
            dp.AddParam("@Prefix", prefix, SqlDbType.VarChar);
            dp.AddParam("@HexVal", hexValue, SqlDbType.VarChar);


            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        private class ParamNoDto
        {
            public string Prefix { get; set; }
            public string HexVal { get; set; }
        }
    }
}