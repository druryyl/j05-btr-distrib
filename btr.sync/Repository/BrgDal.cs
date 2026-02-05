using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Repository
{
    public class BrgDal
    {
        public IEnumerable<BrgType> ListData()
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.BrgCode, aa.BrgName,
                    ISNULL(bb.KategoriName, '') + ' - ' + ISNULL(gg.Keyword, '') AS KategoriName,
                    ISNULL(cc.Satuan, '') AS SatBesar,
                    ISNULL(dd.Satuan, '') AS SatKecil,
                    ISNULL(cc.Conversion, 1) AS Konversi,
                    ISNULL(ff.HrgSat, ISNULL(ff1.HrgSat,0)) AS HrgSat,
                    ISNULL(ee.Stok, 0) AS Stok,
                    '     ' AS ServerId 

                FROM 
                    BTR_Brg aa
                    LEFT JOIN BTR_Kategori bb ON aa.KategoriId = bb.KategoriId
                    LEFT JOIN BTR_BrgSatuan cc ON aa.BrgId = cc.BrgId AND cc.Conversion > 1
                    LEFT JOIN BTR_BrgSatuan dd ON aa.BrgId = dd.BrgId AND dd.Conversion = 1
                    LEFT JOIN (
                        SELECT BrgId, SUM(Qty) AS Stok
                        FROM BTR_StokBalanceWarehouse
                        GROUP BY BrgId
                    ) ee ON aa.BrgId = ee.BrgId
                    LEFT JOIN (
                        SELECT BrgId, Harga AS HrgSat
                        FROM BTR_BrgHarga
                        WHERE HargaTypeId = 'MT'
                    ) ff ON aa.BrgId = ff.BrgId
                    LEFT JOIN (
                        SELECT BrgId, MAX(Harga) AS HrgSat
                        FROM BTR_BrgHarga
                        GROUP BY BrgId
                    ) ff1 ON aa.BrgId = ff1.BrgId

                    LEFT JOIN BTR_Supplier gg ON aa.SupplierId = gg.SupplierId
                 ";
            
            using(var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                var result = conn.Query<BrgType>(sql).ToList();
                return result;
            }
        }
    }
}
