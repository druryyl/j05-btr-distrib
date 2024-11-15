using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.FakturAgg
{
    public class FakturAlokasiFpItemDal : IFakturAlokasiFpItemDal
    {
        private readonly DatabaseOptions _opt;

        public FakturAlokasiFpItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturAlokasiFpItemView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.FakturCode, aa.FakturDate,
                    aa.NoFakturPajak, aa.GrandTotal, aa.UserId,
                    aa.VoidDate, aa.UserIdVoid,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(bb.Address1, '') AS Address,
                    ISNULL(bb.Npwp, '') AS Npwp,
                    IIF(aa.NoFakturPajak = '', 0, 1) IsSet
                FROM
                    BTR_Faktur aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                WHERE
                    aa.FakturDate BETWEEN @Tgl1 AND @Tgl2 
                    AND aa.VoidDate = '3000-01-01'";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturAlokasiFpItemView>(sql, dp);
            }
        }

        public IEnumerable<FakturAlokasiFpItemView> ListData()
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.FakturCode, aa.FakturDate,
                    aa.NoFakturPajak, aa.GrandTotal, aa.UserId,
                    aa.VoidDate, aa.UserIdVoid,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(bb.Address1, '') AS Address,
                    ISNULL(bb.Npwp, '') AS Npwp,
                    IIF(aa.NoFakturPajak = '', 0, 1) IsSet
                FROM
                    BTR_Faktur aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                WHERE
                    aa.FakturDate BETWEEN @Tgl1 AND @Tgl2 
                    AND aa.VoidDate = '3000-01-01'
                    AND aa.NoFakturPajak = '' ";

            var tglLast6Month = DateTime.Now.AddMonths(-6);
            var tglSkrg = DateTime.Now.AddDays(1);
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", tglLast6Month, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", tglSkrg, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturAlokasiFpItemView>(sql, dp);
            }
        }
    }
}