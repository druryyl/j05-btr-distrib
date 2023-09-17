using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.SalesContext.CustomerAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.CustomerAgg
{
    public class CustomerDal : ICustomerDal
    {
        private readonly DatabaseOptions _opt;

        public CustomerDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(CustomerModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Customer(
                CustomerId, CustomerName, WilayahId, KlasifikasiId, HargaTypeId, 
                Address1, Address2, Kota, KodePos, NoTelp, NoFax, 
                Npwp, Nppkp, AlamatWp, AlamatWp2, IsKenaPajak, 
                IsSuspend, Plafond, CreditBalance)
            VALUES (
                @CustomerId, @CustomerName, @WilayahId, @KlasifikasiId, @HargaTypeId, 
                @Address1, @Address2, @Kota, @KodePos, @NoTelp, @NoFax, 
                @Npwp, @Nppkp, @AlamatWp, @AlamatWp2, @IsKenaPajak, 
                @IsSuspend, @Plafond, @CreditBalance)";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);
            
            dp.AddParam("@WilayahId", model.WilayahId, SqlDbType.VarChar);
            dp.AddParam("@KlasifikasiId", model.KlasifikasiId, SqlDbType.VarChar);
            dp.AddParam("@HargaTypeId", model.HargaTypeId, SqlDbType.VarChar);
         
            dp.AddParam("@Address1", model.Address1, SqlDbType.VarChar);
            dp.AddParam("@Address2", model.Address2, SqlDbType.VarChar);
            dp.AddParam("@Kota", model.Kota, SqlDbType.VarChar);
            dp.AddParam("@KodePos", model.KodePos, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", model.NoTelp, SqlDbType.VarChar);
            dp.AddParam("@NoFax", model.NoFax, SqlDbType.VarChar);

            dp.AddParam("@Npwp", model.Npwp, SqlDbType.VarChar);
            dp.AddParam("@Nppkp", model.Nppkp, SqlDbType.VarChar);
            dp.AddParam("@AddressWp", model.AddressWp, SqlDbType.VarChar);
            dp.AddParam("@AddressWp2", model.AddressWp2, SqlDbType.VarChar);
            dp.AddParam("@IsKenaPajak", model.IsKenaPajak, SqlDbType.Bit);
            dp.AddParam("@IsSuspend", model.IsSuspend, SqlDbType.Bit);
            dp.AddParam("@Plafond", model.Plafond, SqlDbType.Decimal);
            dp.AddParam("@CreditBalance", model.CreditBalance, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        
        }

    
        public void Update( CustomerModel model)
        {
            const  string sql  = @"
            UPDATE 
                BTR_Customer
            SET
                CustomerId = @CustomerId,
                CustomerName = @CustomerName,
                WilayahId = @WilayahId,
                KlasifikasiId = @KlasifikasiId,
                HargaTypeId = @HargaTypeId,
                Address1 = @Address1,
                Address2 = @Address2,
                Kota = @Kota,
                KodePos = @KodePos,
                NoTelp = @NoTelp,
                NoFax = @NoFax,
                Npwp = @Npwp,
                Nppkp = @Nppkp,
                AddressWp = @AddressWp,
                AddressWp2 = @AddressWp2,
                IsKenaPajak = @IsKenaPajak,
                IsSuspend = @IsSuspend,
                Plafond = @Plafond,
                CreditBalance = @CreditBalance
            WHERE
                CustomerId = @CustomerId ";

            //  TODO; CODING CUSTOMER
            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);

            dp.AddParam("@WilayahId", model.WilayahId, SqlDbType.VarChar);
            dp.AddParam("@KlasifikasiId", model.KlasifikasiId, SqlDbType.VarChar);
            dp.AddParam("@HargaTypeId", model.HargaTypeId, SqlDbType.VarChar);

            dp.AddParam("@Address1", model.Address1, SqlDbType.VarChar);
            dp.AddParam("@Address2", model.Address2, SqlDbType.VarChar);
            dp.AddParam("@Kota", model.Kota, SqlDbType.VarChar);
            dp.AddParam("@KodePos", model.KodePos, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", model.NoTelp, SqlDbType.VarChar);
            dp.AddParam("@NoFax", model.NoFax, SqlDbType.VarChar);

            dp.AddParam("@Npwp", model.Npwp, SqlDbType.VarChar);
            dp.AddParam("@Nppkp", model.Nppkp, SqlDbType.VarChar);
            dp.AddParam("@AddressWp", model.AddressWp, SqlDbType.VarChar);
            dp.AddParam("@AddressWp2", model.AddressWp2, SqlDbType.VarChar);
            dp.AddParam("@IsKenaPajak", model.IsKenaPajak, SqlDbType.Bit);
            dp.AddParam("@IsSuspend", model.IsSuspend, SqlDbType.Bit);
            dp.AddParam("@Plafond", model.Plafond, SqlDbType.Decimal);
            dp.AddParam("@CreditBalance", model.CreditBalance, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ICustomerKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Customer
            WHERE
                CustomerId = @CustomerId ";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", key.CustomerId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public CustomerModel GetData(ICustomerKey key)
        {
            const string sql = @"
            SELECT
                aa.CustomerId, aa.CustomerName, aa.CustomerCode, 
                aa.WilayahId, aa.KlasifikasiId, aa.HargaTypeId, 
                aa.Address1, aa.Address2, aa.Kota, aa.KodePos, aa.NoTelp, aa.NoFax, 
                aa.Npwp, aa.Nppkp, aa.AddressWp, aa.AddressWp2, aa.IsKenaPajak, aa.IsSuspend, 
                aa.Plafond, aa.CreditBalance, 
                ISNULL(bb.WilayahName, '') AS WilayahName,
                ISNULL(cc.KlasifikasiName, '') AS KlasifikasiName,
                ISNULL(dd.HargaTypeName, '') AS HargaTypeName
            FROM
                BTR_Customer aa
                LEFT JOIN BTR_Wilayah bb ON aa.WilayahId = bb.WilayahId
                LEFT JOIN BTR_Klasifikasi cc ON aa.KlasifikasiId = cc.KlasifikasiId
                LEFT JOIN BTR_HargaType dd ON aa.HargaTypeId = dd.HargaTypeId
            WHERE
                aa.CustomerId = @CustomerId ";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", key.CustomerId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<CustomerModel>(sql, dp);
            }
        }

        public IEnumerable<CustomerModel> ListData()
        {
            const string sql = @"
                SELECT
                    aa.CustomerId, aa.CustomerName, aa.CustomerCode, 
                    aa.WilayahId, aa.KlasifikasiId, aa.HargaTypeId, 
                    aa.Address1, aa.Address2, aa.Kota, aa.KodePos, aa.NoTelp, aa.NoFax, 
                    aa.Npwp, aa.Nppkp, aa.AddressWp, aa.AddressWp2, aa.IsKenaPajak, aa.IsSuspend, 
                    aa.Plafond, aa.CreditBalance, 
                    ISNULL(bb.WilayahName, '') AS WilayahName,
                    ISNULL(cc.KlasifikasiName, '') AS KlasifikasiName,
                    ISNULL(dd.HargaTypeName, '') AS HargaTypeName
                FROM
                    BTR_Customer aa
                    LEFT JOIN BTR_Wilayah bb ON aa.WilayahId = bb.WilayahId
                    LEFT JOIN BTR_Klasifikasi cc ON aa.KlasifikasiId = cc.KlasifikasiId
                    LEFT JOIN BTR_HargaType dd ON aa.HargaTypeId = dd.HargaTypeId ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<CustomerModel>(sql);
            }
        }
    }
}