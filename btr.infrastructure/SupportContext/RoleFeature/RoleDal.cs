using btr.domain.SupportContext.RoleFeature;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.SupportContext.RoleFeature
{
    public interface IRoleDal :
        IInsert<RoleDto>,
        IUpdate<RoleDto>,
        IDelete<IRoleKey>,
        IGetData<RoleDto, IRoleKey>,
        IListData<RoleDto>
    {
    }
    public class RoleDal : IRoleDal
    {
        private readonly DatabaseOptions _opt;

        public RoleDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(RoleDto model)
        {
            const string sql = @"
                INSERT INTO BTR_Role(
                    RoleId, RoleName)
                VALUES (
                    @RoleId, @RoleName)";

            var dp = new DynamicParameters();
            dp.AddParam("@RoleId", model.RoleId, SqlDbType.VarChar);
            dp.AddParam("@RoleName", model.RoleName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(RoleDto model)
        {
            const string sql = @"
                UPDATE 
                    BTR_Role
                SET
                    RoleName = @RoleName
                WHERE
                    RoleId = @RoleId ";

            var dp = new DynamicParameters();
            dp.AddParam("@RoleId", model.RoleId, SqlDbType.VarChar);
            dp.AddParam("@RoleName", model.RoleName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IRoleKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_Role
                WHERE
                    RoleId = @RoleId ";

            var dp = new DynamicParameters();
            dp.AddParam("@RoleId", key.RoleId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public RoleDto GetData(IRoleKey key)
        {
            const string sql = @"
                SELECT
                    RoleId, RoleName
                FROM
                    BTR_Role
                WHERE
                    RoleId = @RoleId ";

            var dp = new DynamicParameters();
            dp.AddParam("@RoleId", key.RoleId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<RoleDto>(sql, dp);
            }
        }

        public IEnumerable<RoleDto> ListData()
        {
            const string sql = @"
                SELECT
                    RoleId, RoleName
                FROM
                    BTR_Role
                ORDER BY
                    RoleId";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<RoleDto>(sql);
            }
        }
    }

    public class RoleDto
    {
        public RoleDto(string roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public RoleType ToModel(IEnumerable<MenuType> listMenu)
        {
            var result = new RoleType(RoleId, RoleName, listMenu);
            return result;
        }
        public static RoleDto FromModel(RoleType model)
        {
            var result = new RoleDto(model.RoleId, model.RoleName);
            return result;
        }
    }
}
