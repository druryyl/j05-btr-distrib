using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using btr.nuna.Infrastructure;
using btr.application.PurchaseContext.InvoiceBrgInfo;

namespace btr.infrastructure.PurchaseContext.ReturBeliInfoRpt
{
    public class ReturBeliBrgViewDal : IReturBeliBrgViewDal
    {
        private readonly DatabaseOptions _opt;

        public ReturBeliBrgViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<ReturBeliBrgViewDto> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.ReturBeliId, aa.ReturBeliCode, aa.ReturBeliDate AS Tgl,  
                    ISNULL(bb.HppSat, 0) AS Hpp, 
                    ISNULL(bb.QtyBesar, 0) AS QtyBesar, 
                    ISNULL(bb.SatBesar, 0) AS SatuanBesar, 
                    ISNULL(bb.QtyKecil, 0) AS Qty, 
                    ISNULL(bb.SatKecil, 0) AS Satuan, 
                    ISNULL(bb.SubTotal, 0) AS SubTotal, 
                    ISNULL(bb.DiscRp, 0) AS Disc, 
                    ISNULL(bb.PpnRp, 0) AS Tax, 
                    ISNULL(bb.Total, 0) AS Total,
                    ISNULL(cc.SupplierName, '') AS SupplierName,
                    ISNULL(dd.BrgName, '') AS BrgName,
                    ISNULL(dd.BrgCode, '') AS BrgCode,
                    ISNULL(ee.KategoriName, '') AS Kategori
                FROM
                    BTR_ReturBeli aa
                    LEFT JOIN BTR_ReturBeliItem bb ON aa.ReturBeliId = bb.ReturBeliId
                    LEFT JOIN BTR_Supplier cc ON aa.SupplierId = cc.SupplierId
                    LEFT JOIN BTR_Brg dd ON bb.BrgId = dd.BrgId
                    LEFT JOIN BTR_Kategori ee ON dd.KategoriId = ee.KategoriId
                WHERE
                    aa.ReturBeliDate BETWEEN @Tgl1 AND @Tgl2 
                    AND aa.VoidDate = '3000-01-01'";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturBeliBrgViewDto>(sql, dp);
            }
        }
    }
}

