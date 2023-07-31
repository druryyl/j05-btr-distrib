using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.InventoryContext.BrgAgg.Contracts;
using btr.domain.InventoryContext.BrgAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext
{
    public class BrgSatuanHargaDal : IBrgSatuanHargaDal
    {
        private readonly DatabaseOptions _opt;

        public BrgSatuanHargaDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<BrgSatuanHargaModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("Satuan", "Satuan");
                bcp.AddMap("Conversion", "Conversion");
                bcp.AddMap("HargaJual", "HargaJual");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_BrgSatuanHarga";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IBrgKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_BrgSatuanHarga
            WHERE
                BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", key.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<BrgSatuanHargaModel> ListData(IBrgKey brgKey)
        {
            const string sql = @"
            SELECT
                aa.BrgId, aa.Satuan, aa.Conversion,
                aa.HargaJual
            FROM 
                BTR_BrgSatuanHarga aa
            WHERE
                aa.BrgId = @BrgId ";
            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", brgKey.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<BrgSatuanHargaModel>(sql, dp);
            }
        }
    }
}