using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.PurchaseContext.ReturBeliFeature;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.ReturBeliAgg
{
    public class ReturBeliDiscDal : IReturBeliDiscDal
    {
        private readonly DatabaseOptions _opt;

        public ReturBeliDiscDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<ReturBeliDiscModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("ReturBeliId","ReturBeliId");
                bcp.AddMap("ReturBeliItemId","ReturBeliItemId");
                bcp.AddMap("ReturBeliDiscId","ReturBeliDiscId");
                bcp.AddMap("NoUrut","NoUrut");
                bcp.AddMap("BrgId","BrgId");
                bcp.AddMap("DiscProsen","DiscProsen");
                bcp.AddMap("DiscRp","DiscRp");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_ReturBeliDisc";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IReturBeliKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_ReturBeliDisc
                WHERE
                    ReturBeliId = @ReturBeliId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@ReturBeliId", key.ReturBeliId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<ReturBeliDiscModel> ListData(IReturBeliKey filter)
        {
            const string sql = @"
                SELECT
                    aa.ReturBeliId, aa.ReturBeliItemId, aa.ReturBeliDiscId, aa.NoUrut, aa.BrgId, 
                    aa.DiscProsen, aa.DiscRp
                FROM                    
                    BTR_ReturBeliDisc aa
                WHERE
                    aa.ReturBeliId = @ReturBeliId ";
            var dp = new DynamicParameters();
            dp.AddParam("@ReturBeliId", filter.ReturBeliId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturBeliDiscModel>(sql, dp);
            }
        }
    }
}