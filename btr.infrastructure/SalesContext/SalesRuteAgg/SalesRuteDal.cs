using btr.application.SalesContext.SalesPersonAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.SalesContext.SalesRuteAgg
{
    public class SalesRuteDal : ISalesRuteDal
    {
        private readonly DatabaseOptions _opt;

        public SalesRuteDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(SalesRuteModel model)
        {
            const string sql = @"
                INSERT INTO BTR_SalesRute(
                    SalesRuteId, SalesPersonId, HariRuteId)
                VALUES (
                    @SalesRuteId, @SalesPersonId, @HariRuteId)";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", model.SalesRuteId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@HariRuteId", model.HariRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(SalesRuteModel model)
        {
            const string sql = @"
                UPDATE
                    BTR_SalesRute
                SET
                    SalesPersonId = @SalesPersonId, 
                    HariRuteId = @HariRuteId
                WHERE
                    SalesRuteId = @SalesRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", model.SalesRuteId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@HariRuteId", model.HariRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ISalesRuteKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_SalesRute
                WHERE
                    SalesRuteId = @SalesRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", key.SalesRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public SalesRuteModel GetData(ISalesRuteKey key)
        {
            const string sql = @"
                SELECT
                    aa.SalesRuteId, aa.SalesPersonId, aa.HariRuteId,
                    ISNULL(bb.SalesPersonName, '') SalesPersonName, 
                    ISNULL(cc.HariRuteName, '') HariRuteName,
                    ISNULL(cc.ShortName, '') ShortName
                FROM
                    BTR_SalesRute aa
                    LEFT JOIN BTR_SalesPerson bb ON aa.SalesPersonId = bb.SalesPersonId
                    LEFT JOIN BTR_HariRute cc ON aa.HariRuteId = cc.HariRuteId
                WHERE
                    aa.SalesRuteId = @SalesRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", key.SalesRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<SalesRuteModel>(sql, dp);
            }
        }

        public IEnumerable<SalesRuteModel> ListData(ISalesPersonKey filter)
        {
            const string sql = @"
                SELECT
                    aa.SalesRuteId, aa.SalesPersonId, aa.HariRuteId,
                    ISNULL(bb.SalesPersonName, '') SalesPersonName, 
                    ISNULL(cc.HariRuteName, '') HariRuteName,
                    ISNULL(cc.ShortName, '') ShortName
                FROM
                    BTR_SalesRute aa
                    LEFT JOIN BTR_SalesPerson bb ON aa.SalesPersonId = bb.SalesPersonId
                    LEFT JOIN BTR_HariRute cc ON aa.HariRuteId = cc.HariRuteId
                WHERE
                    aa.SalesPersonId = @SalesPersonId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesPersonId", filter.SalesPersonId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<SalesRuteModel>(sql, dp);
            }
        }
    }
}
