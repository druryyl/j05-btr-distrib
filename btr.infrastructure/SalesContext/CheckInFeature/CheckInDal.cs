using btr.domain.SalesContext.CheckInFeature;
using btr.infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Infrastructure;
using Dapper;
using btr.application.SalesContext.CheckInFeature;
using btr.nuna.Domain;

namespace btr.infrastructure.SalesContext.CheckInFeature
{
    public class CheckInDal : ICheckInDal
    {
        private readonly DatabaseOptions _opt;

        public CheckInDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

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

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
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

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
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

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete()
        {
            const string sql = @"
            DELETE FROM
                BTR_CheckIn";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
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

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<CheckInModel>(sql, dp);
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

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<CheckInModel>(sql, dp);
            }
        }

    }


}
