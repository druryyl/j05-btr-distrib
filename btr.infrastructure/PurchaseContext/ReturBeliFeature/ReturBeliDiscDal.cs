using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.InvoiceAgg
{
    public class ReturBeliDiscDal : IInvoiceDiscDal
    {
        private readonly DatabaseOptions _opt;

        public ReturBeliDiscDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<InvoiceDiscModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("InvoiceId","InvoiceId");
                bcp.AddMap("InvoiceItemId","InvoiceItemId");
                bcp.AddMap("InvoiceDiscId","InvoiceDiscId");
                bcp.AddMap("NoUrut","NoUrut");
                bcp.AddMap("BrgId","BrgId");
                bcp.AddMap("DiscProsen","DiscProsen");
                bcp.AddMap("DiscRp","DiscRp");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_InvoiceDisc";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IInvoiceKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_InvoiceDisc
                WHERE
                    InvoiceId = @InvoiceId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@InvoiceId", key.InvoiceId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<InvoiceDiscModel> ListData(IInvoiceKey filter)
        {
            const string sql = @"
                SELECT
                    aa.InvoiceId, aa.InvoiceItemId, aa.InvoiceDiscId, aa.NoUrut, aa.BrgId, 
                    aa.DiscProsen, aa.DiscRp
                FROM                    
                    BTR_InvoiceDisc aa
                WHERE
                    aa.InvoiceId = @InvoiceId ";
            var dp = new DynamicParameters();
            dp.AddParam("@InvoiceId", filter.InvoiceId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<InvoiceDiscModel>(sql, dp);
            }
        }
    }
}