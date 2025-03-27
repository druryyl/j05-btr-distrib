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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SalesContext.SalesRuteAgg
{
    public class SalesRuteItemViewDal : ISalesRuteItemViewDal
    {
        private readonly DatabaseOptions _opt;

        public SalesRuteItemViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<SalesRuteItemViewDto> ListData(ISalesPersonKey filter)
        {
            const string sql = @"
                SELECT
                    aa.CustomerId, bb.HariRuteId,
                    ISNULL(cc.CustomerName, '') CustomerName,
                    ISNULL(dd.HariRuteName, '') HariRuteName,
                    ISNULL(dd.ShortName, '') ShortName
                FROM
                    BTR_SalesRuteItem aa
                    LEFT JOIN BTR_SalesRute bb ON aa.SalesRuteId = bb.SalesRuteId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId
                    LEFT JOIN BTR_HariRute dd ON bb.HariRuteId = dd.HariRuteId
                WHERE
                    aa.SalesPersonId = @SalesPersonId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesPersonId", filter.SalesPersonId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<SalesRuteItemViewDto>(sql, dp);
            }
        }
    }
}
