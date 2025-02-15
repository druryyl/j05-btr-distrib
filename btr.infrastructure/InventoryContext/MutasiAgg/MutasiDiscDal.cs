using btr.application.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace btr.infrastructure.InventoryContext.MutasiAgg
{
    public class MutasiDiscDal : IMutasiDiscDal
    {
        private readonly DatabaseOptions _opt;

        public MutasiDiscDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<MutasiDiscModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("MutasiId", "MutasiId");
                bcp.AddMap("MutasiItemId", "MutasiItemId");
                bcp.AddMap("MutasiDiscId", "MutasiDiscId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("DiscProsen", "DiscProsen");
                bcp.AddMap("DiscRp", "DiscRp");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_MutasiDisc";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IMutasiKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_MutasiDisc
                WHERE
                    MutasiId = @mutasiId";

            var dp = new DynamicParameters();
            dp.AddParam("@mutasiId", key.MutasiId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<MutasiDiscModel> ListData(IMutasiKey filter)
        {
            const string sql = @"
                SELECT
                    MutasiId, MutasiItemId, MutasiDiscId, 
                    NoUrut, BrgId, DiscProsen, DiscRp
                FROM
                    BTR_MutasiDisc
                WHERE
                    MutasiId = @mutasiId";

            var dp = new DynamicParameters();
            dp.AddParam("@mutasiId", filter.MutasiId, SqlDbType.VarChar);

            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                var result = conn.Read<MutasiDiscModel>(sql, dp);
                return result;  
            }
        }
    }
}
