using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturInfo;

namespace btr.infrastructure.SalesContext.FakturInfoAgg
{
    public class FakturViewDal : IFakturViewDal
    {
        private readonly DatabaseOptions _opt;

        public FakturViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT 
                    aa.FakturId, aa.FakturCode, aa.FakturDate as Tgl, 
                    aa.Total, aa.Discount, aa.Tax, aa.GrandTotal,
                    ISNULL(bb.UserName, '') Admin, 
                    ISNULL(cc.CustomerName, '') AS Customer,
                    ISNULL(cc.CustomerCode, '') AS CustomerCode,
                    ISNULL(cc.Address1, '') AS Address,
                    ISNULL(dd.WilayahName , '') AS WilayahName,
                    ISNULL(ee.SalesPersonName, '') SalesPersonName,
                    ISNULL(ff.WarehouseName, '') AS WarehouseName
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

            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturView>(sql, dp);
            }
        }
    }
}
