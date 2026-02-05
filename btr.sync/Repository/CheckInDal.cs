using Dapper;
using j07_btrade_sync.Model;
using Nuna.Lib.DataAccessHelper;
using Nuna.Lib.ValidationHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Repository
{
    public class CheckInDal
    {
        public void Insert(CheckInModel model)
        {
            const string sql = @"
            INSERT INTO BTR_CheckIn(
                CheckInId, CheckInDate, CheckInTime, UserEmail, 
                CheckInLatitude, CheckInLongitude, Accuracy,
                CustomerId, CustomerCode, CustomerName, CustomerAddress,
                CustomerLatitude, CustomerLongitude, StatusSync)
            VALUES (
                @CheckInId, @CheckInDate, @CheckInTime, @UserEmail, 
                @CheckInLatitude, @CheckInLongitude, @Accuracy,
                @CustomerId, @CustomerCode, @CustomerName, @CustomerAddress,
                @CustomerLatitude, @CustomerLongitude, @StatusSync)";

            var dp = new DynamicParameters();
            dp.AddParam("@CheckInId", model.CheckInId, SqlDbType.VarChar);
            dp.AddParam("@CheckInDate", model.CheckInDate, SqlDbType.VarChar);
            dp.AddParam("@CheckInTime", model.CheckInTime, SqlDbType.VarChar);
            dp.AddParam("@UserEmail", model.UserEmail, SqlDbType.VarChar);
            dp.AddParam("@CheckInLatitude", model.CheckInLatitude, SqlDbType.Float);
            dp.AddParam("@CheckInLongitude", model.CheckInLongitude, SqlDbType.Float);
            dp.AddParam("@Accuracy", model.Accuracy, SqlDbType.Float);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", model.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@CustomerAddress", model.CustomerAddress, SqlDbType.VarChar);
            dp.AddParam("@CustomerLatitude", model.CustomerLatitude, SqlDbType.Float);
            dp.AddParam("@CustomerLongitude", model.CustomerLongitude, SqlDbType.Float);
            dp.AddParam("@StatusSync", model.StatusSync, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(CheckInModel model)
        {
            const string sql = @"
            UPDATE
                BTR_CheckIn
            SET
                CheckInDate = @CheckInDate,
                CheckInTime = @CheckInTime,
                UserEmail = @UserEmail,
                CheckInLatitude = @CheckInLatitude,
                CheckInLongitude = @CheckInLongitude,
                Accuracy = @Accuracy,
                CustomerId = @CustomerId,
                CustomerCode = @CustomerCode,
                CustomerName = @CustomerName,
                CustomerAddress = @CustomerAddress,
                CustomerLatitude = @CustomerLatitude,
                CustomerLongitude = @CustomerLongitude,
                StatusSync = @StatusSync
            WHERE
                CheckInId = @CheckInId";

            var dp = new DynamicParameters();
            dp.AddParam("@CheckInId", model.CheckInId, SqlDbType.VarChar);
            dp.AddParam("@CheckInDate", model.CheckInDate, SqlDbType.VarChar);
            dp.AddParam("@CheckInTime", model.CheckInTime, SqlDbType.VarChar);
            dp.AddParam("@UserEmail", model.UserEmail, SqlDbType.VarChar);
            dp.AddParam("@CheckInLatitude", model.CheckInLatitude, SqlDbType.Float);
            dp.AddParam("@CheckInLongitude", model.CheckInLongitude, SqlDbType.Float);
            dp.AddParam("@Accuracy", model.Accuracy, SqlDbType.Float);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", model.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", model.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@CustomerAddress", model.CustomerAddress, SqlDbType.VarChar);
            dp.AddParam("@CustomerLatitude", model.CustomerLatitude, SqlDbType.Float);
            dp.AddParam("@CustomerLongitude", model.CustomerLongitude, SqlDbType.Float);
            dp.AddParam("@StatusSync", model.StatusSync, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(ICheckInKey key)
        {
            const string sql = @"
            DELETE FROM
                BTR_CheckIn
            WHERE
                CheckInId = @CheckInId";

            var dp = new DynamicParameters();
            dp.AddParam("@CheckInId", key.CheckInId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete()
        {
            const string sql = @"
            DELETE FROM
                BTR_CheckIn";

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                conn.Execute(sql);
            }
        }

        public CheckInModel GetData(ICheckInKey key)
        {
            const string sql = @"
            SELECT
                CheckInId, CheckInDate, CheckInTime, UserEmail, 
                CheckInLatitude, CheckInLongitude, Accuracy,
                CustomerId, CustomerCode, CustomerName, CustomerAddress,
                CustomerLatitude, CustomerLongitude, StatusSync
            FROM
                BTR_CheckIn
            WHERE
                CheckInId = @CheckInId";

            var dp = new DynamicParameters();
            dp.AddParam("@CheckInId", key.CheckInId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.ReadSingle<CheckInModel>(sql, dp);
            }
        }

        public IEnumerable<CheckInModel> ListData()
        {
            const string sql = @"
            SELECT
                CheckInId, CheckInDate, CheckInTime, UserEmail, 
                CheckInLatitude, CheckInLongitude, Accuracy,
                CustomerId, CustomerCode, CustomerName, CustomerAddress,
                CustomerLatitude, CustomerLongitude, StatusSync
            FROM
                BTR_CheckIn";

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.Read<CheckInModel>(sql);
            }
        }

        public IEnumerable<CheckInModel> ListData(Periode periode)
        {
            const string sql = @"
            SELECT
                CheckInId, CheckInDate, CheckInTime, UserEmail, 
                CheckInLatitude, CheckInLongitude, Accuracy,
                CustomerId, CustomerCode, CustomerName, CustomerAddress,
                CustomerLatitude, CustomerLongitude, StatusSync
            FROM
                BTR_CheckIn
            WHERE 
                CheckInDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", periode.Tgl1.ToString("yyyy-MM-dd"), SqlDbType.VarChar);
            dp.AddParam("@Tgl2", periode.Tgl2.ToString("yyyy-MM-dd"), SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.Read<CheckInModel>(sql, dp);
            }
        }

        public IEnumerable<CheckInModel> ListDataByUser(string userEmail)
        {
            const string sql = @"
            SELECT
                CheckInId, CheckInDate, CheckInTime, UserEmail, 
                CheckInLatitude, CheckInLongitude, Accuracy,
                CustomerId, CustomerCode, CustomerName, CustomerAddress,
                CustomerLatitude, CustomerLongitude, StatusSync
            FROM
                BTR_CheckIn
            WHERE
                UserEmail = @UserEmail
            ORDER BY
                CheckInDate DESC, CheckInTime DESC";

            var dp = new DynamicParameters();
            dp.AddParam("@UserEmail", userEmail, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.Read<CheckInModel>(sql, dp);
            }
        }

        public IEnumerable<CheckInModel> ListDataByStatus(string statusSync)
        {
            const string sql = @"
            SELECT
                CheckInId, CheckInDate, CheckInTime, UserEmail, 
                CheckInLatitude, CheckInLongitude, Accuracy,
                CustomerId, CustomerCode, CustomerName, CustomerAddress,
                CustomerLatitude, CustomerLongitude, StatusSync
            FROM
                BTR_CheckIn
            WHERE
                StatusSync = @StatusSync
            ORDER BY
                CheckInDate, CheckInTime";

            var dp = new DynamicParameters();
            dp.AddParam("@StatusSync", statusSync, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                return conn.Read<CheckInModel>(sql, dp);
            }
        }
    }
}
