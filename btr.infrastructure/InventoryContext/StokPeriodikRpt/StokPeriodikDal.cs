using btr.application.InventoryContext.StokPeriodikInfo;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.InventoryContext.StokPeriodikRpt
{
    public class StokPeriodikDal : IStokPeriodikDal
    {
        private readonly DatabaseOptions _opt;

        public StokPeriodikDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<StokPeriodikDto> ListData(DateTime tgl)
        {
            const string sql = @"
                SELECT 
                    aa.BrgId, aa.WarehouseId, 
                    ISNULL(bb.BrgCode, '') BrgCode,
                    ISNULL(bb.BrgName, '') BrgName,
                    ISNULL(bb.SupplierId, '') SupplierId,
                    ISNULL(cc.SupplierName, '') SupplierName,
                    ISNULL(bb.KategoriId, '') KategoriId,
                    ISNULL(dd.KategoriName, '') KategoriName,
                    ISNULL(bb.Hpp, 0) Hpp,
                    SUM(aa.QtyIn - aa.QtyOut) Qty
                FROM 
                    BTR_StokMutasi aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Supplier cc ON bb.SupplierId = cc.SupplierId
                    LEFT JOIN BTR_Kategori dd ON bb.KategoriId = dd.KategoriId

                WHERE 
                    MutasiDate <= @mutasiDate
                GROUP BY 
                    aa.BrgId, aa.WarehouseId, bb.BrgCode, bb.BrgName,
                    bb.SupplierId, cc.SupplierName, bb.KategoriId, 
                    dd.KategoriName, bb.Hpp ";

            var dp = new DynamicParameters();
            dp.AddParam("@mutasiDate", tgl.Date.AddHours(23).AddMinutes(59).AddSeconds(59), System.Data.SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokPeriodikDto>(sql, dp);
            }
        }
    }
}
