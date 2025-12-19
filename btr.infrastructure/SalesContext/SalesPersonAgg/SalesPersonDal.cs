using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.SalesPersonAgg
{
    public class SalesPersonDal : ISalesPersonDal
    {
        private readonly DatabaseOptions _opt;

        public SalesPersonDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(SalesPersonModel model)
        {
            const string sql = @"
            INSERT INTO BTR_SalesPerson(
                SalesPersonId, SalesPersonCode, SalesPersonName, WilayahId, Email)
            VALUES (
                @SalesPersonId, @SalesPersonCode, @SalesPersonName, @WilayahId, @Email)";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonCode", model.SalesPersonCode, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonName", model.SalesPersonName, SqlDbType.VarChar);
            dp.AddParam("@WilayahId", model.WilayahId, SqlDbType.VarChar);
            dp.AddParam("@EMail", model.Email, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(SalesPersonModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_SalesPerson
            SET
                SalesPersonName = @SalesPersonName,
                SalesPersonCode = @SalesPersonCode,
                WilayahId = @WilayahId,
                Email = @Email
            WHERE
                SalesPersonId = @SalesPersonId ";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonCode", model.SalesPersonCode, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonName", model.SalesPersonName, SqlDbType.VarChar);
            dp.AddParam("@WilayahId", model.WilayahId, SqlDbType.VarChar);
            dp.AddParam("@EMail", model.Email, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ISalesPersonKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_SalesPerson
            WHERE
                SalesPersonId = @SalesPersonId ";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesPersonId", key.SalesPersonId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public SalesPersonModel GetData(ISalesPersonKey key)
        {
            const string sql = @"
            SELECT
                aa.SalesPersonId, aa.SalesPersonName, 
                aa.SalesPersonCode, aa.WilayahId, aa.Email,
                ISNULL(bb.WilayahName, '') AS WilayahName
            FROM
                BTR_SalesPerson aa
                LEFT JOIN BTR_Wilayah bb ON aa.WilayahId = bb.WilayahId
            WHERE
                SalesPersonId = @SalesPersonId ";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesPersonId", key.SalesPersonId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<SalesPersonModel>(sql, dp);
            }
        }

        public IEnumerable<SalesPersonModel> ListData()
        {
            const string sql = @"
            SELECT
                aa.SalesPersonId, aa.SalesPersonName, 
                aa.SalesPersonCode, aa.WilayahId, aa.Email,
                ISNULL(bb.WilayahName, '') AS WilayahName
            FROM
                BTR_SalesPerson aa
                LEFT JOIN BTR_Wilayah bb ON aa.WilayahId = bb.WilayahId ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<SalesPersonModel>(sql);
            }
        }
    }
}