using btr.application.SalesContext.SalesPersonAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using btr.nuna.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.SalesContext.SalesPersonAgg
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
                    SalesRuteId, SalesPersonId, HariId)
                VALUES(
                    @SalesRuteId, @SalesPersonId, @HariId)";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", model.SalesRuteId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@HariId", model.HariRuteId, SqlDbType.VarChar);

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
                    HariId = HariId
                WHERE
                    SalesRuteId = @SalesRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", model.SalesRuteId, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@HariId", model.HariRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ISalesRuteKey key)
        {
            const string sql = @"
                DELETE
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
                    ISNULL(cc.HariRuteName, '') HariRuteName
                FROM
                    BTR_SalesRute aa
                    LEFT JOIN BTR_SalesPerson bb ON aa.SalesPersonId = bb.SalesPersonName
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
                    ISNULL(cc.HariRuteName, '') HariRuteName
                FROM
                    BTR_SalesRute aa
                    LEFT JOIN BTR_SalesPerson bb ON aa.SalesPersonId = bb.SalesPersonName
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
