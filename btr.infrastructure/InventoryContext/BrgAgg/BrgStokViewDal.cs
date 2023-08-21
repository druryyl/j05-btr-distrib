using btr.application.BrgContext.BrgStokViewAgg.Contracts;
using btr.domain.BrgContext.BrgStokViewAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

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
    }
}
