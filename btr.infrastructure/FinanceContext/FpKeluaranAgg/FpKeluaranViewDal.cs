using btr.application.FinanceContext.FpKeluaragAgg;
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

namespace btr.infrastructure.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranViewDal : IFpKeluaranViewDal
    {
        private readonly DatabaseOptions _opt;

        public FpKeluaranViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FpKeluaranViewDto> ListData(Periode periode)
        {
            const string sql = @"
                SELECT 
                    aa.FakturId, aa.FakturCode, aa.FakturDate, ee.CustomerName, aa.GrandTotal,
                    cc.FpKeluaranDate,
                    cc.FpKeluaranId,
                    bb.NpwpNikPembeli,
                    bb.JenisIdPembeli,
                    bb.NamaPembeli,
                    bb.AlamatPembeli,
                    SUM(dd.ppn) Ppn

                FROM
                    BTR_Faktur aa
                    LEFT JOIN BTR_FpKeluaranFaktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_FpKeluaran cc ON bb.FpKeluaranId = cc.FpKeluaranId
                    LEFT JOIN BTR_FpKeluaranBrg dd ON bb.FpKeluaranFakturId = dd.FpKeluaranFakturId
                    LEFT JOIN BTR_Customer ee ON aa.CustomerId = ee.CustomerId

                WHERE
                    aa.FakturDate BETWEEN @tgl1 AND @tgl2
                    AND aa.VoidDate = @tglVoid

                GROUP BY
                    aa.FakturId, aa.FakturCode, aa.FakturDate, aa.GrandTotal,
                    ee.CustomerName, cc.FpKeluaranId, cc.FpKeluaranDate,
                    bb.NpwpNikPembeli, bb.JenisIdPembeli,
                    bb.NamaPembeli, bb.AlamatPembeli ";

            var dp = new DynamicParameters();
            dp.AddParam("@tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@tgl2", periode.Tgl2, SqlDbType.DateTime);
            dp.AddParam("@tglVoid", new DateTime(3000,1,1), SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<FpKeluaranViewDto>(sql, dp);
            }
        }
    }
}
 