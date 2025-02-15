using btr.application.InventoryContext.MutasiAgg;
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
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.InventoryContext.MutasiAgg
{
    public class MutasiBrgViewDal : IMutasiBrgViewDal
    {
        private readonly DatabaseOptions _opt;

        public MutasiBrgViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<MutasiBrgView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.MutasiId, aa.MutasiDate, 
                    CASE aa.JenisMutasi
                        WHEN 0 THEN 'Mutasi Keluar'
                        WHEN 1 THEN 'Mutasi Masuk'
                        WHEN 2 THEN 'Klaim Supplier'
                    END AS JenisMutasi,     
                    bb.BrgId,  bb.Qty AS InPcs, bb.Hpp AS HrgSat, 
                    bb.Qty * bb.Hpp AS SubTotal,
                    bb.DiscRp, bb.NilaiSediaan AS Total,
                    ISNULL(cc.BrgCode, '') BrgCode,
                    ISNULL(cc.BrgName, '') BrgName,
                    ISNULL(dd.SupplierName, '') AS SupplierName,
                    ISNULL(ee.KategoriName, '') AS KategoriName
                FROM
                    BTR_Mutasi aa
                    INNER JOIN BTR_MutasiItem bb ON aa.MutasiId = bb.MutasiId
                    LEFT JOIN BTR_Brg cc ON bb.BrgId = cc.BrgId
                    LEFT JOIN BTR_Supplier dd ON cc.SupplierId = dd.SupplierId
                    LEFT JOIN BTR_Kategori ee ON cc.KategoriId = ee.KategoriId
                WHERE
                    aa.MutasiDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<MutasiBrgView>(sql, dp);
            }
            
        }
    }
}
