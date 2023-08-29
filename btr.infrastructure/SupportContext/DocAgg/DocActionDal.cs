using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.SupportContext.DocAgg;
using btr.domain.SupportContext.DocAgg;
using btr.domain.SupportContext.PrintManagerAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.DocAgg
{
    public class DocActionDal : IDocActionDal
    {
        private readonly DatabaseOptions _opt;

        public DocActionDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<DocActionModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("DocId", "DocId");
                bcp.AddMap("ActionDate", "ActionDate");
                bcp.AddMap("Action", "Action");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_DocAction";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IDocKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_DocAction
                WHERE
                    DocId = @DocId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DocId", key.DocId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<DocActionModel> ListData(IDocKey docKey)
        {
            const string sql = @"
                SELECT
                    aa.DocId, aa.ActionDate, aa.Action
                FROM 
                    BTR_DocAction aa
                WHERE
                    aa.DocId = @DocId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DocId", docKey.DocId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<DocActionModel>(sql, dp);
            }
        }
    }
}