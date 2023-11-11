using btr.application.PurchaseContext.PuchaseInfoRpt;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Infrastructure;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.InvoiceInfoRpt
{
    //  implement interface IInvoiceInfoDal
    public class InvoiceInfoDal : IInvoiceInfoDal
    {
        private readonly DatabaseOptions _opt;

        public InvoiceInfoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<InvoiceInfoDto> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.InvoiceId, aa.InvoiceCode, aa.InvoiceDate AS Tgl,  
                    aa.Total, aa.Disc, aa.Tax, aa.GrandTotal,
                    ISNULL(bb.SupplierName, '') AS SupplierName,
                    ISNULL(cc.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_Invoice aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId
                WHERE
                    InvoiceDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<InvoiceInfoDto>(sql, dp);
            }
        }
    }
}
