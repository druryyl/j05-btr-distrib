using btr.application.SalesContext.FakturInfo;
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
using btr.application.SalesContext.FakturPajakInfo;
using btr.nuna.Infrastructure;

namespace btr.infrastructure.SalesContext.FakturPajakInfo
{
    internal class FakturPajakInfoDal : IFakturPajakViewDal
    {
        private readonly DatabaseOptions _opt;

        public FakturPajakInfoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturPajakView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT 
                    aa.FakturId, aa.FakturCode, aa.FakturDate as Tgl, 
                    aa.Total, aa.Discount, aa.Tax, aa.GrandTotal,
                    ISNULL(bb.UserName, '') Admin, 
                    ISNULL(cc.CustomerName, '') AS Customer,
                    ISNULL(cc.CustomerCode, '') AS CustomerCode,
                    ISNULL(cc.Address1, '') AS Address,
                    ISNULL(cc.Npwp , '') AS Npwp,
                    ISNULL(aa.NoFakturPajak, '') AS NoFakturPajak

                FROM
                    BTR_Faktur aa
                    LEFT JOIN BTR_User bb ON aa.UserId = bb.UserId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId 
                    LEFT JOIN BTR_Wilayah dd ON cc.WilayahId = dd.WilayahId 
                    LEFT JOIN BTR_SalesPerson ee ON aa.SalesPersonId = ee.SalesPersonId 
                    LEFT JOIN BTR_Warehouse ff ON aa.WarehouseId = ff.WarehouseId
                WHERE
                    aa.FakturDate BETWEEN @Tgl1 AND @Tgl2  
                    AND aa.VoidDate = '3000-01-01'";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturPajakView>(sql, dp);
            }
        }
    }
}
