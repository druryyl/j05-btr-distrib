using btr.application.InventoryContext.WarehouseAgg;
using btr.domain.BrgContext.BrgAgg;
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

        public void Insert(DepoType model)
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

        public void Update(DepoType model)
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

        public DepoType GetData(IDepoKey key)
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
                return conn.ReadSingle<DepoType>(sql, dp);
            }
        }

        public DepoType GetData(IBrgKey brg)
        {
            const string sql = @"
            SELECT
                aa.DepoId, aa.DepoName
            FROM
                BTR_Depo aa
                INNER JOIN BTR_Supplier bb ON aa.DepoId = bb.DepoId  
                INNER JOIN BTR_Kategori cc ON bb.SupplierId = cc.SupplierId
                INNER JOIN BTR_Brg dd ON cc.KategoriId = dd.KategoriId
            WHERE
                dd.BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", brg.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<DepoType>(sql, dp);
            }
        }

        public IEnumerable<DepoType> ListData()
        {
            const string sql = @"
            SELECT
                DepoId, DepoName
            FROM
                BTR_Depo";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<DepoType>(sql);
            }
        }
    }
}
