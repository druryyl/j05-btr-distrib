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
    public class BrgSatuanDal : IBrgSatuanDal
    {
        private readonly DatabaseOptions _opt;

        public BrgSatuanDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<BrgSatuanModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("Satuan", "Satuan");
                bcp.AddMap("Conversion", "Conversion");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_BrgSatuan";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IBrgKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_BrgSatuan
            WHERE
                BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", key.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<BrgSatuanModel> ListData(IBrgKey brgKey)
        {
            const string sql = @"
            SELECT
                aa.BrgId, aa.Satuan, aa.Conversion
            FROM 
                BTR_BrgSatuan aa
            WHERE
                aa.BrgId = @BrgId ";
            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", brgKey.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<BrgSatuanModel>(sql, dp);
            }
        }
    }
}