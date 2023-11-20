using btr.application.PurchaseContext.PuchaseInfoRpt;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Infrastructure;

namespace btr.infrastructure.PurchaseContext.InvoiceInfoRpt
{
    public class InvoiceBrgInfoDal : IInvoiceBrgInfoDal
    {
        private readonly DatabaseOptions _opt;

        public InvoiceBrgInfoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<InvoiceBrgInfoDto> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.InvoiceId, aa.InvoiceCode, aa.InvoiceDate AS Tgl,  
                    ISNULL(bb.SatKecil, 0) AS Satuan, 
                    ISNULL(bb.HppSat, 0) AS Hpp, 
                    ISNULL(bb.QtyKecil, 0) AS Qty, 
                    ISNULL(bb.SubTotal, 0) AS SubTotal, 
                    ISNULL(bb.DiscRp, 0) AS Disc, 
                    ISNULL(bb.PpnRp, 0) AS Tax, 
                    ISNULL(bb.Total, 0) AS Total,
                    ISNULL(cc.SupplierName, '') AS SupplierName,
                    ISNULL(dd.BrgName, '') AS BrgName,
                    ISNULL(dd.BrgCode, '') AS BrgCode,
                    ISNULL(ee.KategoriName, '') AS Kategori
                FROM
                    BTR_Invoice aa
                    LEFT JOIN BTR_InvoiceItem bb ON aa.InvoiceId = bb.InvoiceId
                    LEFT JOIN BTR_Supplier cc ON aa.SupplierId = cc.SupplierId
                    LEFT JOIN BTR_Brg dd ON bb.BrgId = dd.BrgId
                    LEFT JOIN BTR_Kategori ee ON dd.KategoriId = ee.KategoriId
                WHERE
                    InvoiceDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<InvoiceBrgInfoDto>(sql, dp);
            }
        }
    }
}

