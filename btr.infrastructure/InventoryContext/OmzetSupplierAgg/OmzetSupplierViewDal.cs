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
using btr.application.SalesContext.OmzetSupplierInfo;

namespace btr.infrastructure.InventoryContext.OmzetSupplierAgg
{
    public class OmzetSupplierViewDal : IOmzetSupplierViewDal
    {
        private readonly DatabaseOptions _opt;

        public OmzetSupplierViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<OmzetSupplierView> ListData(Periode filter)
        {
            //  QUERY
            const string sql = @"
                SELECT 
                    dd.SupplierName, CONVERT(Date, bb.FakturDate) AS FakturDate, SUM(aa.Total) AS Total
                FROM 
                    BTR_FakturItem aa
                    LEFT jOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Brg cc ON aa.BrgId = cc.BrgId
                    LEFT JOIN BTR_Supplier dd ON cc.SupplierId = dd.SupplierId
                WHERE
                    bb.FakturDate BETWEEN @Tgl1 AND @Tgl2
                    AND bb.VoidDate = '3000-01-01'
                GROUP BY
                    dd.SupplierName, CONVERT(Date, bb.FakturDate)";

            //  PARAMETER
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            //  READ
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<OmzetSupplierView>(sql, dp);
            }
        }
    }
}
