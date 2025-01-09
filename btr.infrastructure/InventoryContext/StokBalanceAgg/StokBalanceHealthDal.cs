using btr.application.InventoryContext.StokAgg;
using btr.domain.InventoryContext.StokBalanceAgg;
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

namespace btr.infrastructure.InventoryContext.StokBalanceAgg
{
    public class StokBalanceHealthDal : IStokBalanceHealthDal
    {
        private readonly DatabaseOptions _opt;

        public StokBalanceHealthDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<StokBalanceHealthDto> ListData()
        {
            const string sql = @"
                SELECT  
                    COUNT(aa.BrgId) StokBalanceCount,
                    bb.StokBalanceFailed
                FROM
                    BTR_StokBalanceWarehouse aa
                    FULL JOIN (
                        SELECT
                            COUNT(*) StokBalanceFailed
                        FROM 
                            (
                                SELECT 
                                    ISNULL(aa.BrgId, bb.BrgId) BrgId,
                                    ISNULL(aa.WarehouseId, bb.WarehouseId) WarehouseId,
                                    aa.Qty Qty1, bb.Qty Qty2
                                FROM BTR_StokBalanceWarehouse aa
                                FULL JOIN (
                                    SELECT BrgId, WarehouseId, SUM(Qty) Qty
                                    FROM BTR_Stok
                                    GROUP BY BrgId, WarehouseId
                                    ) bb ON aa.BrgId = bb.BrgId AND aa.WarehouseId = bb.WarehouseId
                                WHERE aa.Qty <> bb.Qty
                            ) bb1
                    )  bb ON 1 = 1
                GROUP BY     
                    bb.StokBalanceFailed";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                var result = conn.Read<StokBalanceHealthDto>(sql);
                return result;
            }
        }

        public void RepairStokHealth()
        {
            const string sql = @"
                UPDATE BTR_StokBalanceWarehouse
                SET Qty = ISNULL(bb.Qty,0)
                FROM BTR_StokBalanceWarehouse aa
                LEFT JOIN (
                    SELECT BrgId, WarehouseId, SUM(Qty) Qty
                    FROM BTR_Stok
                    GROUP BY BrgId, WarehouseId
                    ) bb  ON aa.BrgId = bb.BrgId AND aa.WarehouseId = bb.WarehouseId
                ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql);
            }

        }
    }
}
