using btr.application.SalesContext.FakturPerCustomerRpt;
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

namespace btr.infrastructure.SalesContext.FakturPerCustomerRpt
{
    public class FakturPerCustomerDal : IFakturPerCustomerDal
    {
        private readonly DatabaseOptions _opt;

        public FakturPerCustomerDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturPerCustomerView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    ISNULL(bb.FakturCode, '') AS FakturCode,
                    ISNULL(bb.FakturDate, '3000-01-01') AS FakturDate,
                    ISNULL(bb.DueDate, '3000-01-01') AS DueDate,
                    ISNULL(gg.SupplierCode, '') AS SupplierCode,
                    ISNULL(gg.SupplierName, '') AS SupplierName,
                    ISNULL(hh.KategoriName, '') AS KategoriName,
                    ISNULL(cc.BrgCode, '') AS BrgCode,
                    ISNULL(cc.BrgName, '') AS BrgName,
                    ISNULL(dd.CustomerCode, '') AS CustomerCode,
                    ISNULL(dd.CustomerName, '') AS CustomerName, 
                    ISNULL(dd.Address1, '') AS CustomerAddress,
                    ISNULL(dd.Kota, '') AS CustomerKota,
                    ISNULL(ii.WilayahName, '') AS WilayahName,
                    ISNULL(ee.SalesPersonName, '') AS SalesPersonName,
                    aa.QtyBesar,
                    aa.SatBesar,
                    aa.HrgSatBesar,
                    aa.QtyKecil,
                    aa.SatKecil,
                    aa.HrgSatKecil,
                    aa.QtyBonus,
                    aa.QtyPotStok,
                    aa.SubTotal,
                    ISNULL(ff1.DiscProsen, 0) AS DiscProsen1,
                    ISNULL(ff2.DiscProsen, 0) AS DiscProsen2,
                    ISNULL(ff3.DiscProsen, 0) AS DiscProsen3,
                    ISNULL(ff4.DiscProsen, 0) AS DiscProsen4,
                    ISNULL(aa.DiscRp, 0) AS TotalDisc,
                    aa.SubTotal - ISNULL(aa.DiscRp, 0) TotalSebelumTax,
                    aa.PpnRp,
                    aa.Total,
                    ISNULL(jj.StatusFaktur,0) StatusFaktur
                FROM
                    BTR_FakturItem aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Brg cc ON aa.BrgId = cc.BrgId
                    LEFT JOIN BTR_Customer dd ON bb.CustomerId = dd.CustomerId
                    LEFT JOIN BTR_SalesPerson ee ON bb.SalesPersonId = ee.SalesPersonId
                    LEFT JOIN (SELECT FakturItemId, DiscProsen, DiscRp FROM BTR_FakturDiscount WHERE NoUrut = 1) ff1 ON aa.FakturItemId = ff1.FakturItemId
                    LEFT JOIN (SELECT FakturItemId, DiscProsen, DiscRp FROM BTR_FakturDiscount WHERE NoUrut = 2) ff2 ON aa.FakturItemId = ff2.FakturItemId
                    LEFT JOIN (SELECT FakturItemId, DiscProsen, DiscRp FROM BTR_FakturDiscount WHERE NoUrut = 3) ff3 ON aa.FakturItemId = ff3.FakturItemId
                    LEFT JOIN (SELECT FakturItemId, DiscProsen, DiscRp FROM BTR_FakturDiscount WHERE NoUrut = 4) ff4 ON aa.FakturItemId = ff4.FakturItemId
                    LEFT JOIN BTR_Supplier gg ON cc.SupplierId = gg.SupplierId
                    LEFT JOIN BTR_Kategori hh ON cc.KategoriId = hh.KategoriId
                    LEFT JOIN BTR_Wilayah ii ON dd.WilayahId = ii.WilayahId
                    LEFT JOIN BTR_FakturControlStatus jj ON bb.FakturId = jj.FakturId AND StatusFaktur = 2
                WHERE
                    bb.FakturDate BETWEEN @Tgl1 AND @Tgl2 
                    AND bb.VoidDate = '3000-01-01'";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var cn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return cn.Read<FakturPerCustomerView>(sql, dp);
            }
        }
    }
}
