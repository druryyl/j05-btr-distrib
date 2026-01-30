using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturPerSupplierRpt;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.FakturPerSupplierRpt
{
    public class FakturPerSupplierDal : IFakturPerSupplierDal
    {
        private readonly DatabaseOptions _opt;

        public FakturPerSupplierDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturPerSupplierView> ListData(Periode periode)
        {
            const string sql = @"
                SELECT
                    ISNULL(gg.SupplierName, '') AS SupplierName,
                    ISNULL(gg.Address1, '') AS SupplierAddress,
                    ISNULL(gg.Kota, '') AS SupplierKota,
                    ISNULL(bb.FakturCode, '') AS FakturCode,
                    ISNULL(bb.FakturDate, '') AS FakturDate,
                    ISNULL(bb.DueDate, '') AS JatuhTempo,
                    ISNULL(ee.SalesPersonName, '') AS SalesPersonName,
                    ISNULL(dd.CustomerCode, '') AS CustomerCode,
                    ISNULL(dd.CustomerName, '') AS CustomerName, 
                    ISNULL(hh.KlasifikasiName, '') AS Klasifikasi,
                    ISNULL(dd.Address1, '') AS CustomerAddress,
                    ISNULL(dd.Kota, '') AS CustomerKota,
                    ISNULL(cc.BrgCode, '') AS BrgCode,
                    ISNULL(cc.BrgName, '') AS BrgName,
                    aa.QtyBesar,
                    aa.HrgSatBesar,
                    aa.QtyKecil,
                    aa.HrgSatKecil,
                    aa.QtyJual,
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
                    LEFT JOIN BTR_Klasifikasi hh ON dd.KlasifikasiId = hh.KlasifikasiId
                WHERE
                    bb.FakturDate BETWEEN @Tgl1 AND @Tgl2  
                    AND bb.VoidDate = @TglVoid ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", periode.Tgl2, SqlDbType.DateTime);
            dp.AddParam("@TglVoid", new DateTime(3000,1,1), SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturPerSupplierView>(sql, dp);
            }
        }
    }
}