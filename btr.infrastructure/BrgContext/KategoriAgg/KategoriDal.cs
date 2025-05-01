using btr.domain.BrgContext.KategoriAgg;
using btr.infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using btr.nuna.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using btr.application.BrgContext.KategoriAgg;

namespace btr.infrastructure.BrgContext.KategoriAgg
{
    public class KategoriDal : IKategoriDal
    {
        private readonly DatabaseOptions _opt;

        public KategoriDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(KategoriModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Kategori (KategoriId, KategoriName, Code, SupplierId)
                VALUES (@KategoriId, @KategoriName, @Code, @SupplierId)";

            var dp = new DynamicParameters();
            dp.AddParam("@KategoriId", model.KategoriId, SqlDbType.VarChar);
            dp.AddParam("@KategoriName", model.KategoriName, SqlDbType.VarChar);
            dp.AddParam("@Code", model.Code, SqlDbType.VarChar);
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);


            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(KategoriModel model)
        {
            const string sql = @"
                UPDATE  
                    BTR_Kategori 
                SET     
                    KategoriName = @KategoriName,
                    Code = @Code,
                    SupplierId = @SupplierId
                WHERE
                    KategoriId= @KategoriId ";

            var dp = new DynamicParameters();
            dp.AddParam("@KategoriId", model.KategoriId, SqlDbType.VarChar);
            dp.AddParam("@KategoriName", model.KategoriName, SqlDbType.VarChar);
            dp.AddParam("@Code", model.Code, SqlDbType.VarChar);
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IKategoriKey key)
        {
            const string sql = @"
                DELETE FROM  
                    BTR_Kategori 
                WHERE
                    KategoriId= @KategoriId ";

            var dp = new DynamicParameters();
            dp.AddParam("@KategoriId", key.KategoriId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public KategoriModel GetData(IKategoriKey key)
        {
            const string sql = @"
                SELECT
                    aa.KategoriId, aa.KategoriName, aa.Code, aa.SupplierId,
                    ISNULL(SupplierName,' ') SupplierName
                FROM
                    BTR_Kategori  aa
                LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                WHERE
                    aa.KategoriId= @KategoriId ";

            var dp = new DynamicParameters();
            dp.AddParam("@KategoriId", key.KategoriId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<KategoriModel>(sql, dp);
            }
        }

        public IEnumerable<KategoriModel> ListData()
        {
            const string sql = @"
                SELECT
                    aa.KategoriId, aa.KategoriName, aa.Code, aa.SupplierId,
                    ISNULL(SupplierName,' ') SupplierName
                FROM
                    BTR_Kategori  aa
                LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<KategoriModel>(sql);
            }
        }
    }
}
