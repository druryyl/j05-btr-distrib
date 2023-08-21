using btr.application.BrgContext.BrgStokViewAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgStokViewAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace btr.infrastructure.InventoryContext.BrgAgg
{
    public class BrgStokViewDal : IBrgStokViewDal
    {
        private readonly DatabaseOptions _opt;

        public BrgStokViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<BrgStokViewModel> ListData(IWarehouseKey filter)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.BrgName, SUM(bb.Qty) Stok
                FROM
                    BTR_Brg aa
                    LEFT JOIN BTR_Stok bb ON aa.BrgId = bb.BrgId
                        AND bb.WarehouseId = @WarehouseId
                GROUP BY
                    aa.BrgId, aa.BrgName ";

            var dp = new DynamicParameters();
            dp.AddParam("@WarehouseId", filter.WarehouseId, System.Data.SqlDbType.VarChar);

            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<BrgStokViewModel>(sql, dp);
            }
        }

        public BrgStokViewModel GetData<T>(T key) where T : IBrgKey, IWarehouseKey
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.BrgName, SUM(bb.Qty) Stok
                FROM
                    BTR_Brg aa
                    LEFT JOIN BTR_Stok bb ON aa.BrgId = bb.BrgId
                        AND bb.WarehouseId = @WarehouseId
                WHERE
                    aa.BrgId = @BrgId 
                GROUP BY
                    aa.BrgId, aa.BrgName ";

            var dp = new DynamicParameters();
            dp.AddParam("@WarehouseId", key.WarehouseId, System.Data.SqlDbType.VarChar);
            dp.AddParam("@BrgId", key.BrgId, System.Data.SqlDbType.VarChar);


            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<BrgStokViewModel>(sql, dp);
            }
        }
    }
}
