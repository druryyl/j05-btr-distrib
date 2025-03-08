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
                    aa.UserId, aa.StokOpId, aa.StokOpDate,
                    ISNULL(bb.BrgCode, '') BrgCode,
                    ISNULL(bb.BrgName, '') BrgName,
                    ISNULL(cc.WarehouseName, '') WarehouseName
                FROM
                    BTR_StokOp aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
                    LEFT JOIN BTR_Warehouse cc ON aa.WarehouseId = cc.WarehouseId  
                WHERE   
                    aa.StokOpDate BETWEEN @Tgl1 AND @Tgl2  ";

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
