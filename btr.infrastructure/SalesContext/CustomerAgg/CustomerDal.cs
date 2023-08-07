using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.SalesContext.CustomerAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext
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
                CustomerId, CustomerName, Plafond, CreditBalance, Address1, Address2)
            VALUES (
                @CustomerId, @CustomerName, @Plafond, @CreditBalance)";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@Plafond", model.Plafond, SqlDbType.Decimal);
            dp.AddParam("@CreditBalance", model.CreditBalance, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(CustomerModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Customer
            SET
                CustomerName = @CustomerName,
                Plafond = @Plafond,
                CreditBalance = @CreditBalance
            WHERE
                CustomerId = @CustomerId ";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);
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
                CustomerId, CustomerName, Plafond, CreditBalance
            FROM
                BTR_Customer
            WHERE
                CustomerId = @CustomerId ";

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
                CustomerId, CustomerName, Plafond, CreditBalance
            FROM
                BTR_Customer";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<CustomerModel>(sql);
            }
        }
    }
}