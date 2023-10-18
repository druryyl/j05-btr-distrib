using btr.application.InventoryContext.ImportOpnameAgg.Contracts;
using btr.domain.InventoryContext.ImportOpnameAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace btr.infrastructure.InventoryContext.ImportOpnameAgg
{
    public class ImportOpnameDal : IImportOpnameDal
    {
        private readonly DatabaseOptions _opt;

        public ImportOpnameDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<ImportOpnameModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                bcp.AddMap("BrgCode", "BrgCode");
                bcp.AddMap("WarehouseId", "WarehouseId");
                bcp.AddMap("Qty", "Qty");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_ImportOpname";

                conn.Open();
                bcp.WriteToServer(fetched.AsDataTable());
            }

        }
        public void Delete()
        {
            const string sql = @"
                DELETE FROM BTR_ImportOpname";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql);
            }
        }

        public IEnumerable<ImportOpnameModel> ListData()
        {
            const string sql = @"
                SELECT
                    BrgCode, WarehouseId, Qty
                FROM
                    BTR_ImportOpname";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ImportOpnameModel>(sql);
            }
        }
    }
}
