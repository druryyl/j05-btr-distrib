using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.InventoryContext.ReturJualAgg;
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

namespace btr.infrastructure.InventoryContext.ReturJualAgg
{
    public class ReturJualItemQtyHrgDal : IReturJualItemQtyHrgDal
    {
        private readonly DatabaseOptions _opt;

        public ReturJualItemQtyHrgDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(ReturJualItemQtyHrgModel model)
        {
            //  create bulk insert ReturJualItemQtyHarModel
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();

                bcp.ColumnMappings.Add("ReturJualId", "ReturJualId");
                bcp.ColumnMappings.Add("ReturJualItemId", "ReturJualItemId");
                bcp.ColumnMappings.Add("ReturJualItemQtyHrgId", "ReturJualItemQtyHrgId");
                bcp.ColumnMappings.Add("NoUrut", "NoUrut");
                bcp.ColumnMappings.Add("BrgId", "BrgId");
                bcp.ColumnMappings.Add("JenisQty", "JenisQty");
                bcp.ColumnMappings.Add("Conversion", "Conversion");
                bcp.ColumnMappings.Add("Qty", "Qty");
                bcp.ColumnMappings.Add("HrgSat", "HrgSat");
                bcp.ColumnMappings.Add("SubTotal", "SubTotal");

                var fetched = new List<ReturJualItemQtyHrgModel> { model };
                bcp.DestinationTableName = "BTR_ReturJualItemQtyHrg";
                bcp.BatchSize = fetched.Count;
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IReturJualKey key)
        {
            // QUERY
            const string sql = @"
                DELETE FROM BTR_ReturJualItemQtyHrg
                WHERE ReturJualId = @ReturJualId";

            //  PARAMETER
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, System.Data.SqlDbType.VarChar);

            //  EXECUTE
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<ReturJualItemQtyHrgModel> ListData(IReturJualKey filter)
        {
            // QUERY
            const string sql = @"
                SELECT 
                    ReturJualId, ReturJualItemId, ReturJualItemQtyHrgId, NoUrut, 
                    BrgId, JenisQty, Conversion, Qty, HrgSat, SubTotal
                FROM 
                    BTR_ReturJualItemQtyHrg
                WHERE 
                    ReturJualId = @ReturJualId";

            //  PARAMETER
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", filter.ReturJualId, System.Data.SqlDbType.VarChar);

            //  LOAD
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturJualItemQtyHrgModel>(sql, dp);
            }
        }
    }
}
