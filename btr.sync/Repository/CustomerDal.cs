using Dapper;
using j07_btrade_sync.Model;
using Nuna.Lib.DataAccessHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace j07_btrade_sync.Repository
{
    public class CustomerDal
    {
        public IEnumerable<CustomerType> ListData()
        {
            const string sql = @"
                SELECT
                    aa.CustomerId, aa.CustomerCode, aa.CustomerName,
                    aa.Address1 as Alamat, ISNULL(bb.WilayahName, '') AS Wilayah,
                    aa.Latitude, aa.Longitude, aa.Accuracy, aa.CoordinateTimeStamp, aa.CoordinateUser,
                    '     ' AS ServerId 
                FROM
                    BTR_Customer aa
                    LEFT JOIN BTR_Wilayah bb ON aa.WilayahId = bb.WilayahId";

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                var result = conn.Query<CustomerType>(sql).ToList();
                return result;
            }
        }

        public CustomerType GetData(string custId)
        {
            const string sql = @"
                SELECT
                    aa.CustomerId, aa.CustomerCode, aa.CustomerName,
                    aa.Address1 as Alamat, ISNULL(bb.WilayahName, '') AS Wilayah,
                    aa.Latitude, aa.Longitude, aa.Accuracy, aa.CoordinateTimeStamp, aa.CoordinateUser,
                    '     ' AS ServerId
                FROM
                    BTR_Customer aa
                    LEFT JOIN BTR_Wilayah bb ON aa.WilayahId = bb.WilayahId
                WHERE
                    aa.CustomerId = @CustomerId ";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", custId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                var result = conn.QuerySingle<CustomerType>(sql, dp);
                return result;
            }
        }

        public void UpdateLocation(CustomerType customer)
        {
            const string sql = @"
                UPDATE
                    BTR_Customer 
                SET
                    Latitude = @Latitude,
                    Longitude = @Longitude,
                    Accuracy = @Accuracy,
                    CoordinateTimeStamp = @CoordinateTimeStamp,
                    CoordinateUser = @CoordinateUser
                WHERE
                    CustomerId = @CustomerId ";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", customer.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@Latitude", customer.Latitude, SqlDbType.Float);
            dp.AddParam("@Longitude", customer.Longitude, SqlDbType.Float);
            dp.AddParam("@Accuracy", customer.Accuracy, SqlDbType.Float);
            dp.AddParam("@CoordinateTimeStamp", customer.CoordinateTimeStamp, SqlDbType.DateTime);
            dp.AddParam("@CoordinateUser", customer.CoordinateUser, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
            
            InsertLocHist(customer);
        }

        private void InsertLocHist(CustomerType customer)
        {
            var model = CustomerLocHistModel.Create(customer);

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

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
        }
    }
}
