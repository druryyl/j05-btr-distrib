using btr.application.PurchaseContext.InvoiceHarianDetilRpt;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Infrastructure;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.PurchaseContext.InvoiceHarianDetilRpt
{
    public class InvoiceHarianDetilDal : IInvoiceHarianDetilDal
    {
        private readonly DatabaseOptions _opt;

        public InvoiceHarianDetilDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<InvoiceHarianDetilView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    ISNULL(bb.InvoiceCode, '') InvoiceCode,
                    ISNULL(bb.InvoiceDate, '3000-01-01') InvoiceDate,
                    ISNULL(dd.SupplierName, '') SupplierName,
                    ISNULL(ee.KategoriName, '') KategoriName,
                    ISNULL(cc.BrgCode, '') BrgCode,
                    ISNULL(cc.BrgName, '') BrgName,
                    aa.QtyBesar,
                    aa.SatBesar,
                    aa.HppSatBesar,
                    aa.QtyKecil,
                    aa.SatKecil,
                    aa.HppSatKecil,     

                    aa.QtyBonus,
                    aa.SubTotal,
                    ISNULL(ff1.DiscProsen, 0) AS DiscProsen1,
                    ISNULL(ff2.DiscProsen, 0) AS DiscProsen2,
                    ISNULL(ff3.DiscProsen, 0) AS DiscProsen3,
                    ISNULL(ff4.DiscProsen, 0) AS DiscProsen4,
                    ISNULL(aa.DiscRp, 0) AS TotalDisc,
                    aa.SubTotal - ISNULL(aa.DiscRp, 0) TotalSebelumTax,
                    aa.PpnRp,
                    aa.Total
                FROM
                    BTR_InvoiceItem aa
                    LEFT JOIN BTR_Invoice bb ON aa.InvoiceId = bb.InvoiceId
                    LEFT JOIN BTR_Brg cc ON aa.BrgId = cc.BrgId
                    LEFT JOIN BTR_Supplier dd ON bb.SupplierId = dd.SupplierId
                    LEFT JOIN BTR_Kategori ee ON cc.KategoriId = ee.KategoriId
                    LEFT JOIN (SELECT InvoiceItemId, DiscProsen, DiscRp FROM BTR_InvoiceDisc WHERE NoUrut = 1) ff1 ON aa.InvoiceItemId = ff1.InvoiceItemId
                    LEFT JOIN (SELECT InvoiceItemId, DiscProsen, DiscRp FROM BTR_InvoiceDisc WHERE NoUrut = 2) ff2 ON aa.InvoiceItemId = ff2.InvoiceItemId
                    LEFT JOIN (SELECT InvoiceItemId, DiscProsen, DiscRp FROM BTR_InvoiceDisc WHERE NoUrut = 3) ff3 ON aa.InvoiceItemId = ff3.InvoiceItemId
                    LEFT JOIN (SELECT InvoiceItemId, DiscProsen, DiscRp FROM BTR_InvoiceDisc WHERE NoUrut = 4) ff4 ON aa.InvoiceItemId = ff4.InvoiceItemId
                WHERE
                    bb.InvoiceDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<InvoiceHarianDetilView>(sql, dp);
            }
        }
    }
}
