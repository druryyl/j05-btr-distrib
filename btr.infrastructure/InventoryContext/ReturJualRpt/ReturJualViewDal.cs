using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.application.SalesContext.FakturBrgInfo;
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

namespace btr.infrastructure.InventoryContext.ReturJualRpt
{
    public class ReturJualViewDal : IReturJualViewDal
    {
        private readonly DatabaseOptions _opt;

        public ReturJualViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<ReturJualView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT 
                    aa.ReturJualId, aa.ReturJualCode, aa.ReturJualDate, aa.JenisRetur, 
                    aa.Total, aa.DiscRp, aa.PpnRp, aa.GrandTotal,
                    ISNULL(cc.CustomerCode, '') AS CustomerCode,
                    ISNULL(cc.CustomerName, '') AS CustomerName,
                    ISNULL(cc.Address1, '') AS Address,
                    ISNULL(dd.WilayahName, '') AS WilayahName,
                    ISNULL(ii.SalesPersonName, '') AS SalesName,
                    ISNULL(jj.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_ReturJual aa
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId 
                    LEFT JOIN BTR_Wilayah dd ON cc.WilayahId = dd.WilayahId 
                    LEFT JOIN BTR_SalesPerson ii ON aa.SalesPersonId = ii.SalesPersonId
                    LEFT JOIN BTR_Warehouse jj ON aa.WarehouseId = jj.WarehouseId
                WHERE
                    aa.ReturJualDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturJualView>(sql, dp);
            }
        }
    }
}
