using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.BrgContext.BrgAgg
{
    public class BrgHargaDal : IBrgHargaDal
    {
        private readonly DatabaseOptions _opt;

        public BrgHargaDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<BrgHargaModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("HargaTypeId", "HargaTypeId");
                bcp.AddMap("Harga", "Harga");
                bcp.AddMap("Hpp", "Hpp");
                bcp.AddMap("HargaTimestamp", "HargaTimestamp");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_BrgHarga";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IBrgKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_BrgHarga
                WHERE
                    BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgID", key.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<BrgHargaModel> ListData(IBrgKey filter)
        {
            const string sql = @"
                SELECT
                    BrgId, HargaTypeId,  Harga, Hpp, HargaTimestamp
                FROM
                    BTR_BrgHarga
                WHERE
                    BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgID", filter.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<BrgHargaModel>(sql, dp);
            }
        }
    }
}
