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
    public class ReturJualItemDiscDal : IReturJualItemDiscDal
    {
        private readonly DatabaseOptions _opt;

        public ReturJualItemDiscDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<ReturJualItemDiscModel> model)
        {
            //  create bulk insert ReturJualItemDiscModel
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();

                bcp.ColumnMappings.Add("ReturJualId", "ReturJualId");
                bcp.ColumnMappings.Add("ReturJualItemId", "ReturJualItemId");
                bcp.ColumnMappings.Add("ReturJualItemDiscId", "ReturJualItemDiscId");
                bcp.ColumnMappings.Add("NoUrut", "NoUrut");
                bcp.ColumnMappings.Add("BrgId", "BrgId");
                bcp.ColumnMappings.Add("DiscNo", "DiscNo");
                bcp.ColumnMappings.Add("BaseHrg", "BaseHrg");
                bcp.ColumnMappings.Add("DiscProsen", "DiscProsen");
                bcp.ColumnMappings.Add("DiscRp", "DiscRp");

                var fetched = model.ToList();
                bcp.DestinationTableName = "BTR_ReturJualItemDisc";
                bcp.BatchSize = fetched.Count;
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IReturJualKey key)
        {
            // QUERY
            const string sql = @"
                DELETE FROM BTR_ReturJualItemDisc
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

        public IEnumerable<ReturJualItemDiscModel> ListData(IReturJualKey filter)
        {
            //  QUERY
            const string sql = @"
                SELECT
                    ReturJualId, ReturJualItemId, ReturJualItemDiscId, 
                    NoUrut, BrgId, DiscNo, BaseHrg, DiscProsen, DiscRp
                FROM
                    BTR_ReturJualItemDisc
                WHERE
                    ReturJualId = @ReturJualId";
            
            //  PARAMETER
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", filter.ReturJualId, System.Data.SqlDbType.VarChar);

            //  EXECUTE
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<ReturJualItemDiscModel>(sql, dp);
            }
        }
    }
}
