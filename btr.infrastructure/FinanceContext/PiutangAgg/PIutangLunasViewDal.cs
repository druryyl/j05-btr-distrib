using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
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

namespace btr.infrastructure.FinanceContext.PiutangAgg
{
    public class PIutangLunasViewDal : IPiutangLunasViewDal
    {
        private readonly DatabaseOptions _opt;

        public PIutangLunasViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<PiutangLunasView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.PiutangId, aa.CustomerId, 
                    CAST(aa.PiutangDate AS Date) AS PiutangDate, 
                    aa.Total, aa.Potongan, aa.Terbayar, aa.Sisa, 
                    ISNULL(FakturCode, '') AS FakturCode, 
                    ISNULL(CustomerName, '') AS Customer,
                    ISNULL(Address1, '') AS Address,
                    ISNULL(SalesPersonName, '') AS Sales
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Faktur bb ON aa.PiutangId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId
                    LEFT JOIN BTR_SalesPerson dd ON bb.SalesPersonId = dd.SalesPersonId
                WHERE
                    PiutangDate BETWEEN @Tgl1 AND @Tgl2 ";
                
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangLunasView>(sql, dp);
            }
        }
    }
}
