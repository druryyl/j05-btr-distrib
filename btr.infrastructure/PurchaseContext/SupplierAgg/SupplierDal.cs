using btr.application.PurchaseContext.SupplierAgg.Contracts;
using btr.domain.PurchaseContext.SupplierAgg;
using btr.infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.nuna.Infrastructure;

namespace btr.infrastructure.PurchaseContext.SupplierAgg
{
    public class SupplierDal : ISupplierDal
    {
        private readonly DatabaseOptions _opt;

        public SupplierDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(SupplierModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Supplier (
                    SupplierId, SupplierName, SupplierCode,
                    Address1, Address2, Kota, KodePos, NoTelp,
                    NoFax, ContactPerson, Npwp, NoPkp, Keyword, DepoId)
                VALUES(
                    @SupplierId, @SupplierName, @SupplierCode,
                    @Address1, @Address2, @Kota, @KodePos, @NoTelp,
                    @NoFax, @ContactPerson, @Npwp, @NoPkp, @Keyword, @DepoId) ";

            var dp = new DynamicParameters();
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);
            dp.AddParam("@SupplierName", model.SupplierName, SqlDbType.VarChar);
            dp.AddParam("@SupplierCode", model.SupplierCode, SqlDbType.VarChar);

            dp.AddParam("@Address1", model.Address1, SqlDbType.VarChar);
            dp.AddParam("@Address2", model.Address2, SqlDbType.VarChar);
            dp.AddParam("@Kota", model.Kota, SqlDbType.VarChar);
            dp.AddParam("@KodePos", model.KodePos, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", model.NoTelp, SqlDbType.VarChar);
            dp.AddParam("@NoFax", model.NoFax, SqlDbType.VarChar);
            dp.AddParam("@ContactPerson", model.ContactPerson, SqlDbType.VarChar);

            dp.AddParam("@Npwp", model.Npwp, SqlDbType.VarChar);
            dp.AddParam("@NoPkp", model.NoPkp, SqlDbType.VarChar);
            dp.AddParam("@Keyword", model.Keyword, SqlDbType.VarChar);
            dp.AddParam("@DepoId", model.DepoId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(SupplierModel model)
        {
            const string sql = @"
                UPDATE 
                    BTR_Supplier
                SET
                    SupplierId = @SupplierId, 
                    SupplierName = @SupplierName, 
                    SupplierCode = @SupplierCode,
                    Address1 = @Address1,
                    Address2 = @Address2, 
                    Kota = @Kota, 
                    KodePos = @KodePos, 
                    NoTelp = @NoTelp,
                    NoFax = @NoFax, 
                    ContactPerson = @ContactPerson, 
                    Npwp = @Npwp, 
                    NoPkp = @NoPkp,
                    Keyword = @Keyword,
                    DepoId = @DepoId
                WHERE
                    SupplierId = @SupplierId ";

            var dp = new DynamicParameters();
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);
            dp.AddParam("@SupplierName", model.SupplierName, SqlDbType.VarChar);
            dp.AddParam("@SupplierCode", model.SupplierCode, SqlDbType.VarChar);

            dp.AddParam("@Address1", model.Address1, SqlDbType.VarChar);
            dp.AddParam("@Address2", model.Address2, SqlDbType.VarChar);
            dp.AddParam("@Kota", model.Kota, SqlDbType.VarChar);
            dp.AddParam("@KodePos", model.KodePos, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", model.NoTelp, SqlDbType.VarChar);
            dp.AddParam("@NoFax", model.NoFax, SqlDbType.VarChar);
            dp.AddParam("@ContactPerson", model.ContactPerson, SqlDbType.VarChar);

            dp.AddParam("@Npwp", model.Npwp, SqlDbType.VarChar);
            dp.AddParam("@NoPkp", model.NoPkp, SqlDbType.VarChar);
            dp.AddParam("@Keyword", model.Keyword, SqlDbType.VarChar);
            dp.AddParam("@DepoId", model.DepoId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ISupplierKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_Supplier
                WHERE
                    SupplierId = @SupplierId ";

            var dp = new DynamicParameters();
            dp.AddParam("@SupplierId", key.SupplierId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public SupplierModel GetData(ISupplierKey key)
        {
            const string sql = @"
                SELECT
                    aa.SupplierId, aa.SupplierName, aa.SupplierCode,
                    aa.Address1, aa.Address2, aa.Kota, aa.KodePos, aa.NoTelp,
                    aa.NoFax, aa.ContactPerson, aa.Npwp, aa.NoPkp, aa.Keyword, aa.DepoId,
                    ISNULL(bb.DepoName, '') AS DepoName
                FROM
                    BTR_Supplier aa
                    LEFT JOIN BTR_Depo bb ON aa.DepoId = bb.DepoId      
                WHERE
                    SupplierId = @SupplierId  ";

            var dp = new DynamicParameters();
            dp.AddParam("@SupplierId", key.SupplierId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<SupplierModel>(sql, dp);
            }
        }

        public IEnumerable<SupplierModel> ListData()
        {
            const string sql = @"
                SELECT
                    aa.SupplierId, aa.SupplierName, aa.SupplierCode,
                    aa.Address1, aa.Address2, aa.Kota, aa.KodePos, aa.NoTelp,
                    aa.NoFax, aa.ContactPerson, aa.Npwp, aa.NoPkp, aa.Keyword, aa.DepoId,
                    ISNULL(bb.DepoName, '') AS DepoName
                FROM
                    BTR_Supplier aa
                    LEFT JOIN BTR_Depo bb ON aa.DepoId = bb.DepoId ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<SupplierModel>(sql);
            }
        }
    }
}
