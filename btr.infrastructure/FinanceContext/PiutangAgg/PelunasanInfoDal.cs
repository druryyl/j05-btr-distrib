using btr.application.FinanceContext.PiutangAgg.Contracts;
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
    public class PelunasanInfoDal : IPelunasanInfoDal
    {
        private readonly DatabaseOptions _opt;

        public PelunasanInfoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<PelunasanInfoDto> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    ISNULL(ee.SalesPersonName, '') AS SalesPersonName,
                    ISNULL(cc.CustomerName, '') AS CustomerName,
                    ISNULL(cc.CustomerCode, '') AS CustomerCode,
                    ISNULL(cc.Address1, '') AS Address1,
                    ISNULL(cc.Kota, '') AS Kota,
                    ISNULL(dd.FakturCode, '') AS FakturCode,
                    bb.Total AS TotalFaktur,
                    ISNULL(gg1.NilaiPlus,0) - ISNULL(gg1.NilaiMinus,0) AS Retur,
                    ISNULL(gg2.NilaiPlus,0) - ISNULL(gg2.NilaiMinus,0) AS Potongan,
                    ISNULL(gg3.NilaiPlus,0) - ISNULL(gg3.NilaiMinus,0) AS Materai,
                    ISNULL(gg4.NilaiPlus,0) - ISNULL(gg4.NilaiMinus,0) AS Admin,
                    aa.PelunasanId,
                    aa.LunasDate AS TglBayar,
                    CASE JenisLunas WHEN 0 THEN aa.Nilai WHEN 2 THEN aa.Nilai ELSE 0 END AS Cash,
                    CASE JenisLunas WHEN 1 THEN aa.Nilai ELSE 0 END AS BgTransfer,
                    bb.Sisa
                FROM
                    BTR_PiutangLunas aa
                    INNER JOIN BTR_Piutang bb ON aa.PiutangId = bb.PiutangId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                    LEFT JOIN BTR_Faktur dd ON bb.PiutangId = dd.FakturId
                    LEFT JOIN BTR_SalesPerson ee ON dd.SalesPersonId = ee.SalesPersonId
                    LEFT JOIN BTR_TagihanFaktur ff ON aa.TagihanId = ff.TagihanId AND aa.PiutangId = ff.FakturId
                    LEFT JOIN BTR_PiutangElement gg1 ON aa.PiutangId = gg1.PiutangId AND gg1.ElementTag = 1
                    LEFT JOIN BTR_PiutangElement gg2 ON aa.PiutangId = gg2.PiutangId AND gg2.ElementTag = 2
                    LEFT JOIN BTR_PiutangElement gg3 ON aa.PiutangId = gg3.PiutangId AND gg3.ElementTag = 3
                    LEFT JOIN BTR_PiutangElement gg4 ON aa.PiutangId = gg4.PiutangId AND gg4.ElementTag = 4
                WHERE
                    aa.LunasDate BETWEEN @tgl1 AND @tgl2
                ORDER BY
                    ee.SalesPersonName, cc.CustomerCode, aa.PiutangId, aa.NoUrut ";

            var dp = new DynamicParameters();
            dp.AddParam("@tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PelunasanInfoDto>(sql, dp);
            }
        }
    }
}
