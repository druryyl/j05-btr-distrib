using btr.application.SalesContext.SalesPersonAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SalesContext.SalesRuteAgg
{
    public class SalesRuteItemDal : ISalesRuteItemDal
    {
        private readonly DatabaseOptions _opt;

        public SalesRuteItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }
        public void Insert(IEnumerable<SalesRuteItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                bcp.AddMap("SalesRuteId", "SalesRuteId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("CustomerId", "CustomerId");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_SalesRuteItem";
                conn.Open();
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(ISalesRuteKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_SalesRuteItem
                WHERE 
                    SalesRuteId = @SalesRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", key.SalesRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
               
        }

        public IEnumerable<SalesRuteItemModel> ListData(ISalesRuteKey filter)
        {
            const string sql = @"
                SELECT
                    aa.SalesRuteId, aa.NoUrut, aa.CustomerId,
                    ISNULL(bb.CustomerName, '') CustomerName,
                    ISNULL(bb.CustomerCode, '') CustomerCode,
                    ISNULL(bb.Address1, '') Address,
                    ISNULL(cc.WilayahName, '') Wilayah
                FROM
                    BTR_SalesRuteItem aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Wilayah cc ON bb.WilayahId = cc.WilayahId
                WHERE
                    aa.SalesRuteId = @SalesRuteId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesRuteId", filter.SalesRuteId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<SalesRuteItemModel>(sql, dp);
            }

        }

        public IEnumerable<SalesRuteItemModel> ListData(ISalesPersonKey filter)
        {
            const string sql = @"
                SELECT
                    aa.SalesRuteId, aa.NoUrut, aa.CustomerId,
                    ISNULL(bb.CustomerName, '') CustomerName,
                    ISNULL(bb.CustomerCode, '') CustomerCode,
                    ISNULL(bb.Address1, '') Address,
                    ISNULL(cc.WilayahName, '') Wilayah
                FROM
                    BTR_SalesRuteItem aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Wilayah cc ON bb.WilayahId = cc.WilayahId
                WHERE
                    aa.SalesPersonId = @SalesPersonId";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesPersonId", filter.SalesPersonId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<SalesRuteItemModel>(sql, dp);
            }
        }
    }
}
