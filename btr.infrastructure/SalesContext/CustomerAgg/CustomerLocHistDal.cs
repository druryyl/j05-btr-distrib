using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.domain.SalesContext.CustomerAgg;
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

namespace btr.infrastructure.SalesContext.CustomerAgg
{
    public class CustomerLocHistDal : ICustomerLocHistDal
    {
        private readonly DatabaseOptions _opt;

        public CustomerLocHistDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(CustomerLocHistModel model)
        {
            const string sql = @"
                INSERT INTO BTR_CustomerLocHist 
                (LocHistId, CustomerId, ChangeDate, Latitude, Longitude, Accuracy, ChangeUser)
                VALUES 
                (@LocHistId, @CustomerId, @ChangeDate, @Latitude, @Longitude, @Accuracy, @ChangeUser)";

            var dp = new DynamicParameters();
            dp.AddParam("@LocHistId", model.LocHistId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@ChangeDate", model.ChangeDate, SqlDbType.DateTime);
            dp.AddParam("@Latitude", model.Latitude, SqlDbType.Float);
            dp.AddParam("@Longitude", model.Longitude, SqlDbType.Float);
            dp.AddParam("@Accuracy", model.Accuracy, SqlDbType.Float);
            dp.AddParam("@ChangeUser", model.ChangeUser, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(CustomerLocHistModel model)
        {
            const string sql = @"
            UPDATE  
                BTR_CustomerLocHist 
            SET     
                CustomerId = @CustomerId,
                ChangeDate = @ChangeDate,
                Latitude = @Latitude,
                Longitude = @Longitude,
                Accuracy = @Accuracy,
                ChangeUser = @ChangeUser
            WHERE
                LocHistId = @LocHistId";

            var dp = new DynamicParameters();
            dp.AddParam("@LocHistId", model.LocHistId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@ChangeDate", model.ChangeDate, SqlDbType.DateTime);
            dp.AddParam("@Latitude", model.Latitude, SqlDbType.Float);
            dp.AddParam("@Longitude", model.Longitude, SqlDbType.Float);
            dp.AddParam("@Accuracy", model.Accuracy, SqlDbType.Float);
            dp.AddParam("@ChangeUser", model.ChangeUser, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ICustomerLocHistKey key)
        {
            const string sql = @"
            DELETE FROM  
                BTR_CustomerLocHist 
            WHERE
                LocHistId = @LocHistId";

            var dp = new DynamicParameters();
            dp.AddParam("@LocHistId", key.LocHistId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public CustomerLocHistModel GetData(ICustomerLocHistKey key)
        {
            const string sql = @"
            SELECT
                LocHistId, CustomerId, ChangeDate, 
                Latitude, Longitude, Accuracy, ChangeUser
            FROM
                BTR_CustomerLocHist
            WHERE
                LocHistId = @LocHistId";

            var dp = new DynamicParameters();
            dp.AddParam("@LocHistId", key.LocHistId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<CustomerLocHistModel>(sql, dp);
            }
        }

        public IEnumerable<CustomerLocHistModel> ListData()
        {
            const string sql = @"
            SELECT
                LocHistId, CustomerId, ChangeDate, 
                Latitude, Longitude, Accuracy, ChangeUser
            FROM
                BTR_CustomerLocHist";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<CustomerLocHistModel>(sql);
            }
        }

        public IEnumerable<CustomerLocHistModel> ListData(ICustomerKey filter)
        {
            const string sql = @"
                SELECT
                    LocHistId, CustomerId, ChangeDate, 
                    Latitude, Longitude, Accuracy, ChangeUser
                FROM
                    BTR_CustomerLocHist
                WHERE
                    CustomerId = @CustomerId
                ORDER BY
                    ChangeDate DESC, LocHistId";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", filter.CustomerId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<CustomerLocHistModel>(sql, dp);
            }
        }

    }
}
