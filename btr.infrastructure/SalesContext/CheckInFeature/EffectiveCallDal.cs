using btr.application.SalesContext.CheckInFeature;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SalesContext.CheckInFeature
{
    public class EffectiveCallDal : IEffectiveCallDal
    {
        private readonly DatabaseOptions _opt;

        public EffectiveCallDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<EffectiveCallView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT 
                    aa.CheckInId, aa.CheckInDate, aa.UserEmail, 
                    aa.CustomerId, aa.CustomerCode, aa.CustomerName,
                    COUNT(bb.OrderId) OrderCount
                FROM BTR_CheckIn aa
                    LEFT JOIN BTR_Order bb ON aa.CustomerId = bb.CustomerId
                    AND aa.UserEmail = bb.UserEmail AND aa.CheckInDate = bb.OrderDate
                WHERE
                    aa.CheckInDate BETWEEN @Tgl1 AND @Tgl2      
                GROUP BY 
                    aa.CheckInId, aa.CheckInDate, aa.UserEmail, 
                    aa.CustomerId, aa.CustomerCode, aa.CustomerName
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1.ToString("yyyy-MM-ss"), System.Data.SqlDbType.VarChar);
            dp.AddParam("@Tgl2", filter.Tgl2.ToString("yyyy-MM-ss"), System.Data.SqlDbType.VarChar);

            using ( var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<EffectiveCallView>(sql, dp);
            }
        }
    }
}
