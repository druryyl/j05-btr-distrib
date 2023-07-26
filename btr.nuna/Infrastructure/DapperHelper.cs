using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace btr.nuna.Infrastructure
{
    public static class DapperHelper
    {
        public static void AddParam(this DynamicParameters cmd, string param, object value, SqlDbType type)
        {
            DbType dbType = TypeConvertor.ToDbType(type);
            if (dbType == DbType.AnsiString)
            {
                var length = value.ToString().Length;
                cmd.Add(param, value, dbType, ParameterDirection.Input, length);
            }
            else
            {
                cmd.Add(param, value, dbType, ParameterDirection.Input);
            }
        }

        public static IEnumerable<T> Read<T>(this SqlConnection conn, string sql, DynamicParameters param = null)
        {
            var result = conn.Query<T>(sql, param);
            if (result.Any())
                return result;
            else
                return default;
        }
        public static T ReadSingle<T>(this SqlConnection conn, string sql, DynamicParameters param)
        {
            return conn.QueryFirstOrDefault<T>(sql, param);
        }

        public static int InsertBulk<T>(this SqlConnection conn, string sql, IEnumerable<T> listData)
        {
            return conn.Execute(sql, listData);
        }

        public static void Execute(this string sql, string connString, DynamicParameters param = null)
        {
            using (var conn = new SqlConnection(connString))
                conn.Execute(sql, param);
        }

        public static T ReadSingle<T>(this string sql, string connString, DynamicParameters param = null)
        {
            using(var conn = new SqlConnection(connString))
                return conn.ReadSingle<T>(sql, param);
        }

        public static IEnumerable<T> Read<T>(this string sql, string connString, DynamicParameters param = null)
        {
            using (var conn = new SqlConnection(connString))
                return conn.Read<T>(sql, param);
        }

        //  asynchronous
        public static async Task<IEnumerable<T>> ReadAsync<T>(this SqlConnection conn, string sql, DynamicParameters param = null)
        {
            var result = await conn.QueryAsync<T>(sql, param);
            if (result.Any())
                return result;
            else
                return null;
        }
        public static async Task<T> ReadSingleAsync<T>(this SqlConnection conn, string sql, DynamicParameters param)
        {
            return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
        }


        public static async Task ExecuteAsync(this string sql, string connString, DynamicParameters param = null)
        {
            using (var conn = new SqlConnection(connString))
                await conn.ExecuteAsync(sql, param);
        }

        public static async Task<T> ReadSingleAsync<T>(this string sql, string connString, DynamicParameters param = null)
        {
            using (var conn = new SqlConnection(connString))
                return await conn.ReadSingleAsync<T>(sql, param);
        }

        public static async Task<IEnumerable<T>> ReadAsync<T>(this string sql, string connString, DynamicParameters param = null)
        {
            using (var conn = new SqlConnection(connString))
                return await conn .ReadAsync<T>(sql, param);
        }
    }
}