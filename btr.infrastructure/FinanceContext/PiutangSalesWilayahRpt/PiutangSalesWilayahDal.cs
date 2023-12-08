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

namespace btr.infrastructure.FinanceContext.PiutangSalesWilayahRpt
{
    public class PiutangSalesWilayahDal : IPiutangSalesWilayahDal
    {
        private readonly DatabaseOptions _opt;

        public PiutangSalesWilayahDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<PiutangSalesWilayahDto> ListData(Periode filter)
        {
            const string sql = @"
            SELECT
                ISNULL(cc.SalesPersonName, '') AS SalesName,
                ISNULL(dd.WilayahName, '') AS WilayahName,
                ISNULL(bb.FakturCode, '') AS FakturCode,
                ISNULL(bb.FakturDate, '3000-01-01') AS FakturDate,
                ISNULL(ee.CustomerName, '') AS CustomerName,
                ISNULL(aa.DueDate, '3000-01-01') AS JatuhTempo,
                ISNULL(aa.Total, 0) AS TotalJual,
                ISNULL(ff.BayarTunai, 0) AS BayarTunai,
                ISNULL(gg.BayarGiro, 0) AS BayarGiro,
                ISNULL(hh.Retur, 0) AS Retur,
                ISNULL(ii.Potongan, 0) AS Potongan,
                ISNULL(jj.MateraiAdmin, 0) AS MateraiAdmin,
                aa.Sisa AS KurangBayar
            FROM 
                BTR_Piutang aa
                LEFT JOIN BTR_Faktur bb ON aa.PiutangId = bb.FakturId
                LEFT JOIN BTR_SalesPerson cc ON bb.SalesPersonId = cc.SalesPersonId
                LEFT JOIN BTR_Customer ee ON bb.CustomerId = ee.CustomerId
                LEFT JOIN BTR_Wilayah dd ON ee.WilayahId = dd.WilayahId
                LEFT JOIN (
                    SELECT  PiutangId, SUM(aa1.Nilai) BayarTunai
                    FROM BTR_PiutangLunas aa1
                    WHERE aa1.JenisLunas = 0 
                    GROUP BY PiutangId) ff ON aa.PiutangId = ff.PiutangId
                LEFT JOIN (
                    SELECT  PiutangId, SUM(aa1.Nilai) BayarGiro
                    FROM BTR_PiutangLunas aa1
                    WHERE aa1.JenisLunas = 1 
                    GROUP BY PiutangId) gg ON aa.PiutangId = gg.PiutangId
                LEFT JOIN (
                    SELECT  PiutangId, SUM(aa1.NilaiPlus - aa1.NilaiMinus) Retur
                    FROM BTR_PiutangElement aa1
                    WHERE aa1.ElementName = 'Retur' 
                    GROUP BY PiutangId) hh ON aa.PiutangId = hh.PiutangId
                LEFT JOIN (
                    SELECT  PiutangId, SUM(aa1.NilaiPlus - aa1.NilaiMinus) Potongan
                    FROM BTR_PiutangElement aa1
                    WHERE aa1.ElementName = 'Potongan' 
                    GROUP BY PiutangId) ii ON aa.PiutangId = ii.PiutangId
                LEFT JOIN (
                    SELECT  PiutangId, SUM(aa1.NilaiPlus - aa1.NilaiMinus) MateraiAdmin
                    FROM BTR_PiutangElement aa1
                    WHERE aa1.ElementName = 'Materai' OR aa1.ElementName = 'Admin' 
                    GROUP BY PiutangId) jj ON aa.PiutangId = jj.PiutangId
            WHERE
                aa.PiutangDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangSalesWilayahDto>(sql, dp);
            }
        }
    }
}
