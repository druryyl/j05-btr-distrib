using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SupportContext.UserAgg;
using btr.domain.SupportContext.UserAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SupportContext.UserAgg
{
    public class UserDal : IUserDal
    {
        private readonly DatabaseOptions _opt;

        public UserDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(UserModel model)
        {
            const string sql = @"
            INSERT INTO BTR_User(
                UserId, UserName, Password, Prefix, RoleId)
            VALUES (
                @UserId, @UserName, @Password, @Prefix, @RoleId)";

            var dp = new DynamicParameters();
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@UserName", model.UserName, SqlDbType.VarChar);
            dp.AddParam("@Password", model.Password, SqlDbType.VarChar);
            dp.AddParam("@Prefix", model.Prefix, SqlDbType.VarChar);
            dp.AddParam("@RoleId", model.RoleId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(UserModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_User
            SET
                UserName = @UserName,
                Password = @Password,
                Prefix = @Prefix,
                RoleId = @RoleId
            WHERE
                UserId = @UserId ";

            var dp = new DynamicParameters();
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@UserName", model.UserName, SqlDbType.VarChar);
            dp.AddParam("@Password", model.Password, SqlDbType.VarChar);
            dp.AddParam("@Prefix", model.Prefix, SqlDbType.VarChar);
            dp.AddParam("@RoleId", model.RoleId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IUserKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_User
            WHERE
                UserId = @UserId ";

            var dp = new DynamicParameters();
            dp.AddParam("@UserId", key.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public UserModel GetData(IUserKey key)
        {
            const string sql = @"
                SELECT
                    aa.UserId, aa.UserName, aa.Password, aa.Prefix, aa.RoleId,
                    ISNULL(bb.RoleName, '') AS RoleName
                FROM
                    BTR_User aa
                    LEFT JOIN BTR_Role bb ON aa.RoleId = bb.RoleId
                WHERE
                    aa.UserId = @UserId ";

            var dp = new DynamicParameters();
            dp.AddParam("@UserId", key.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<UserModel>(sql, dp);
            }
        }

        public IEnumerable<UserModel> ListData()
        {
            const string sql = @"
                SELECT
                    aa.UserId, aa.UserName, aa.Password, aa.Prefix, aa.RoleId,
                    ISNULL(bb.RoleName, '') AS RoleName
                FROM
                    BTR_User aa
                    LEFT JOIN BTR_Role bb ON aa.RoleId = bb.RoleId ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<UserModel>(sql);
            }
        }
    }
}