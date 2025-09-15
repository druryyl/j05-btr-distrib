using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturBrgInfo;

namespace btr.infrastructure.SalesContext.FakturInfoAgg
{
    public class FakturBrgViewDal : IFakturBrgViewDal
    {
        private readonly DatabaseOptions _opt;

        public FakturBrgViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturBrgView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT 
                    aa.FakturId, aa.FakturCode, aa.FakturDate, 
                    ISNULL(cc.CustomerName, '') AS CustomerName,
                    ISNULL(dd.WilayahName, '') AS WilayahName,
                    ISNULL(ff.BrgName, '') AS BrgName,
                    ISNULL(gg.KategoriName, '') AS KategoriName,
                    ISNULL(hh.SupplierName, '') AS SupplierName,
                    ISNULL(bb.QtyJual, 0) AS QtyJual,
                    ISNULL(bb.HrgSat, 0) AS HrgSat,
                    ISNULL(bb.QtyJual, 0) * ISNULL(bb.HrgSat, 0) AS SubTotal,
                    ISNULL(bb.DiscRp, 0) AS DiscRp,
                    ISNULL(bb.PpnRp, 0) AS PpnRp,
                    ISNULL(bb.Total, 0) AS Total,
                    ISNULL(ii.KlasifikasiName, '') AS KlasifikasiName
                FROM
                    BTR_Faktur aa
                    LEFT JOIN BTR_FakturItem bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId 
                    LEFT JOIN BTR_Wilayah dd ON cc.WilayahId = dd.WilayahId 
                    LEFT JOIN BTR_Brg ff ON bb.BrgId = ff.BrgId
                    LEFT JOIN BTR_Kategori gg ON ff.KategoriId = gg.KategoriId
                    LEFT JOIN BTR_Supplier hh ON ff.SupplierId = hh.SupplierId
                    LEFT JOIN BTR_Klasifikasi ii ON cc.KlasifikasiId = ii.KlasifikasiId
                WHERE
                    aa.FakturDate BETWEEN @Tgl1 AND @Tgl2 
                    AND aa.VoidDate = '3000-01-01'";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturBrgView>(sql, dp);
            }
        }
    }
}
