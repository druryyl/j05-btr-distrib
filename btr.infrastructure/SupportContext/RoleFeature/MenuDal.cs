using btr.domain.SupportContext.RoleFeature;
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

namespace btr.infrastructure.SupportContext.RoleFeature
{
    public interface IMenuDal :
        IInsert<MenuDto>,
        IUpdate<MenuDto>,
        IDelete<IMenuKey>,
        IGetData<MenuDto, IMenuKey>,
        IListData<MenuDto>
    {
    }

    public class MenuDal : IMenuDal
    {
        private readonly DatabaseOptions _opt;

        public MenuDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(MenuDto model)
        {
            const string sql = @"
            INSERT INTO BTR_Menu(
                MenuId, GroupOrder, FormType, MenuName, Caption)
            VALUES (
                @MenuId, @GroupOrder, @FormType, @MenuName, @Caption)";

            var dp = new DynamicParameters();
            dp.AddParam("@MenuId", model.MenuId, SqlDbType.VarChar);
            dp.AddParam("@GroupOrder", model.GroupOrder, SqlDbType.Int);
            dp.AddParam("@FormType", model.FormType, SqlDbType.VarChar);
            dp.AddParam("@MenuName", model.MenuName, SqlDbType.VarChar);
            dp.AddParam("@Caption", model.Caption, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(MenuDto model)
        {
            const string sql = @"
            UPDATE 
                BTR_Menu
            SET
                GroupOrder = @GroupOrder,
                FormType = @FormType,
                MenuName = @MenuName,
                Caption = @Caption
            WHERE
                MenuId = @MenuId ";

            var dp = new DynamicParameters();
            dp.AddParam("@MenuId", model.MenuId, SqlDbType.VarChar);
            dp.AddParam("@GroupOrder", model.GroupOrder, SqlDbType.Int);
            dp.AddParam("@FormType", model.FormType, SqlDbType.VarChar);
            dp.AddParam("@MenuName", model.MenuName, SqlDbType.VarChar);
            dp.AddParam("@Caption", model.Caption, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IMenuKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Menu
            WHERE
                MenuId = @MenuId ";

            var dp = new DynamicParameters();
            dp.AddParam("@MenuId", key.MenuId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public MenuDto GetData(IMenuKey key)
        {
            const string sql = @"
            SELECT
                MenuId, GroupOrder, FormType, MenuName, Caption
            FROM
                BTR_Menu
            WHERE
                MenuId = @MenuId ";

            var dp = new DynamicParameters();
            dp.AddParam("@MenuId", key.MenuId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<MenuDto>(sql, dp);
            }
        }

        public IEnumerable<MenuDto> ListData()
        {
            const string sql = @"
            SELECT
                MenuId, GroupOrder, FormType, MenuName, Caption
            FROM
                BTR_Menu
            ORDER BY
                MenuId";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<MenuDto>(sql);
            }
        }
    }
}
