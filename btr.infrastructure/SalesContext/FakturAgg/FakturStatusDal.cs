using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturControlAgg;
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

namespace btr.infrastructure.SalesContext.FakturAgg
{
    public class FakturStatusDal : IFakturStatusDal
    {
        private readonly DatabaseOptions _opt;

        public FakturStatusDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturControlStatusModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT aa.FakturId, aa.FakturDate, 
                       ISNULL(cc.StatusDate, aa.FakturDate) StatusDate, 
                       ISNULL(cc.Keterangan, '') Keterangan, 
                       ISNULL(cc.UserId, '') UserId,
                        ISNULL(bb.StatusFaktur,0) StatusFaktur
                FROM
                    BTR_Faktur aa
                    LEFT JOIN(
                        SELECT FakturId, MAX(StatusFaktur) StatusFaktur
                        FROM BTR_FakturControlStatus
                        GROUP BY FakturId) bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_FakturControlStatus cc ON bb.FakturId = cc.FakturId AND bb.StatusFaktur = cc.StatusFaktur
                WHERE
                    aa.FakturDate BETWEEN @tgl1 AND @tgl2";

            var dp = new DynamicParameters();

            dp.AddParam("@tgl1", filter.Tgl1, System.Data.SqlDbType.DateTime);
            dp.AddParam("@tgl2", filter.Tgl2, System.Data.SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturControlStatusModel>(sql, dp).ToList();
            }
        }
    }
}
