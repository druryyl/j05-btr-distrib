using btr.application.SalesContext.RuteAgg;
using btr.domain.SalesContext.RuteAgg;
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

namespace btr.infrastructure.SalesContext.RuteAgg
{
    public class RuteItemDal : IRuteItemDal
    {
        private readonly DatabaseOptions _opt;

        public RuteItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<RuteItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                bcp.AddMap("RuteId", "RuteId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("CustomerId", "CustomerId");

                bcp.DestinationTableName = "BTR_RuteItem";
                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                conn.Open();
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IRuteKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_RuteItem
                WHERE
                    RuteId = @RuteId ";

            var dp = new DynamicParameters();
            dp.AddParam("@RuteId", key.RuteId, System.Data.SqlDbType.VarChar);

            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<RuteItemModel> ListData(IRuteKey filter)
        {
            const string sql = @"
                SELECT
                    aa.RuteId, aa.NoUrut, aa.CustomerId,
                    ISNULL(bb.CustomerName, '') AS CustomerName ,
                    ISNULL(bb.CustomerCode, '') AS CustomerCode ,
                    ISNULL(bb.Address1, '')  AS Address
                FROM
                    BTR_RuteItem aa 
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId      
                WHERE
                    aa.RuteId = @RuteId ";

            var dp = new DynamicParameters();
            dp.AddParam("@RuteId", filter.RuteId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<RuteItemModel>(sql, dp);
            }
        }
    }
}
