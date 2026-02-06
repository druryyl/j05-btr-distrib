using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.InventoryContext.WarehouseAgg
{
    public class DepoDal : IDepoDal
    {
        private readonly DatabaseOptions _opt;

        public DepoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(DepoModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Depo(
                DepoId, DepoName)
            VALUES (
                @DepoId, @DepoName)";

            var dp = new DynamicParameters();
            dp.AddParam("@DepoId", model.DepoId, SqlDbType.VarChar);
            dp.AddParam("@DepoName", model.DepoName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(DepoModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Depo
            SET
                DepoName = @DepoName
            WHERE
                DepoId = @DepoId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DepoId", model.DepoId, SqlDbType.VarChar);
            dp.AddParam("@DepoName", model.DepoName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IDepoKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Depo
            WHERE
                DepoId = @DepoId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DepoId", key.DepoId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public DepoModel GetData(IDepoKey key)
        {
            const string sql = @"
            SELECT
                DepoId, DepoName
            FROM
                BTR_Depo
            WHERE
                DepoId = @DepoId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DepoId", key.DepoId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<DepoModel>(sql, dp);
            }
        }

        public IEnumerable<DepoModel> ListData()
        {
            const string sql = @"
            SELECT
                DepoId, DepoName
            FROM
                BTR_Depo";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<DepoModel>(sql);
            }
        }
    }
}
