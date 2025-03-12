using btr.application.InventoryContext.OpnameAgg;
using btr.nuna.Domain;
using Dapper;
using System.Collections.Generic;
using btr.nuna.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.Options;


namespace btr.infrastructure.InventoryContext.OpnameAgg
{
    public class StokOpInfoDal : IStokOpInfoDal
    {
        private readonly DatabaseOptions _opt;

        public StokOpInfoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public StokOpInfoDal()
        {

        }
        public IEnumerable<StokOpInfoView> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.QtyBesarAwal, aa.QtyKecilAwal, 
                    aa.QtyBesarAdjust, aa.QtyKecilAdjust, 
                    aa.QtyBesarOpname, aa.QtyKecilOpname,
                    aa.UserId, aa.StokOpId, aa.PeriodeOp,
                    ISNULL(bb.BrgCode, '') BrgCode,
                    ISNULL(bb.BrgName, '') BrgName,
                    ISNULL(cc.WarehouseName, '') WarehouseName,
                    ISNULL(dd.KategoriName, '') KategoriName,       
                    ISNULL(ee.SupplierName, '') SupplierName
                FROM
                    BTR_StokOp aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId  
                    LEFT JOIN BTR_Kategori dd ON bb.KategoriId = dd.KategoriId
                    LEFT JOIN BTR_Supplier ee ON bb.SupplierId = ee.SupplierId
                WHERE   
                    aa.PeriodeOp BETWEEN @Tgl1 AND @Tgl2  
                    AND aa.QtyBesarOpname + aa.QtyKecilOpname <> 0
                ORDER BY 
                    aa.PeriodeOp, ee.SupplierName, dd.KategoriName, bb.BrgCode";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokOpInfoView>(sql, dp);
            }
        }
    }
}
