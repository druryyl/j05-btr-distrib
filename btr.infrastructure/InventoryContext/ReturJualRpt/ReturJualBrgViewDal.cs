using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturBrgInfo;

namespace btr.infrastructure.InventoryContext.FakturInfoAgg
{
    public class ReturJualBrgViewDal : IReturJualBrgViewDal
    {
        private readonly DatabaseOptions _opt;

        public ReturJualBrgViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<ReturJualBrgView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT 
                    aa.ReturJualId, aa.ReturJualCode, aa.ReturJualDate, aa.JenisRetur, 
                    ISNULL(ii.SalesPersonName, '')SalesName,
                    ISNULL(cc.CustomerCode, '') AS CustomerCode,
                    ISNULL(cc.CustomerName, '') AS CustomerName,
                    ISNULL(cc.Address1, '') AS Address,
                    ISNULL(dd.WilayahName, '') AS WilayahName,
                    ISNULL(ff.BrgCode, '') AS BrgCode,
                    ISNULL(ff.BrgName, '') AS BrgName,
                    ISNULL(gg.KategoriName, '') AS KategoriName,
                    ISNULL(hh.SupplierName, '') AS SupplierName,
                    ISNULL(bb.BrgId, '') BrgId,
                    ISNULL(bb.Qty, 0) AS InPcs,
                    ISNULL(bb.HrgSat, 0) AS HrgSat,
                    ISNULL(bb.Qty, 0) * ISNULL(bb.HrgSat, 0) AS SubTotal,
                    ISNULL(ff1.DiscProsen, 0) AS Disc1,
                    ISNULL(ff2.DiscProsen, 0) AS Disc2,
                    ISNULL(ff3.DiscProsen, 0) AS Disc3,
                    ISNULL(ff4.DiscProsen, 0) AS Disc4,
                    ISNULL(bb.DiscRp, 0) AS DiscRp,
                    ISNULL(bb.PpnRp, 0) AS PpnRp,
                    ISNULL(bb.Total, 0) AS Total
                FROM
                    BTR_ReturJual aa
                    LEFT JOIN BTR_ReturJualItem bb ON aa.ReturJualId = bb.ReturJualId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId 
                    LEFT JOIN BTR_Wilayah dd ON cc.WilayahId = dd.WilayahId 
                    LEFT JOIN BTR_Brg ff ON bb.BrgId = ff.BrgId
                    LEFT JOIN BTR_Kategori gg ON ff.KategoriId = gg.KategoriId
                    LEFT JOIN BTR_Supplier hh ON ff.SupplierId = hh.SupplierId
                    LEFT JOIN BTR_SalesPerson ii ON aa.SalesPersonId = ii.SalesPersonId
                    LEFT JOIN BTR_ReturJualItemDisc ff1 ON bb.ReturJualItemId = ff1.ReturJualItemId AND ff1.NoUrut = 1
                    LEFT JOIN BTR_ReturJualItemDisc ff2 ON bb.ReturJualItemId = ff2.ReturJualItemId AND ff2.NoUrut = 2  
                    LEFT JOIN BTR_ReturJualItemDisc ff3 ON bb.ReturJualItemId = ff3.ReturJualItemId AND ff3.NoUrut = 3
                    LEFT JOIN BTR_ReturJualItemDisc ff4 ON bb.ReturJualItemId = ff4.ReturJualItemId AND ff4.NoUrut = 4
                WHERE
                    aa.ReturJualDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturJualBrgView>(sql, dp);
            }
        }
    }
}
