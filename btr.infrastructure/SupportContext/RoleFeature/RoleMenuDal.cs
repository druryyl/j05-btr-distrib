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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SupportContext.RoleFeature
{
    public interface IRoleMenuDal :
        IInsertBulk<RoleMenuDto>,
        IDelete<IRoleKey>,
        IListData<RoleMenuDto, IRoleKey>
    {

    }
    public class RoleMenuDal : IRoleMenuDal
    {
        private readonly DatabaseOptions _opt;

        public RoleMenuDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }
        public void Insert(IEnumerable<RoleMenuDto> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                bcp.AddMap("RoleId", "RoleId");
                bcp.AddMap("MenuId", "MenuId");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_RoleMenu";
                conn.Open();
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IRoleKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_RoleMenu
            WHERE 
                RoleId = @RoleId";

            var dp = new DynamicParameters();
            dp.AddParam("@RoleId", key.RoleId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<RoleMenuDto> ListData(IRoleKey filter)
        {
            const string sql = @"
            SELECT
                aa.RoleId, aa.MenuId, 
                ISNULL(bb.GroupOrder, 0) AS GroupOrder,
                ISNULL(bb.FormType, '') AS FormType,
                ISNULL(bb.MenuName, '') AS MenuName, 
                ISNULL(bb.Caption, '') AS Caption
            FROM
                BTR_RoleMenu aa
                LEFT JOIN BTR_Menu bb ON aa.MenuId = bb.MenuId
            WHERE
                RoleId = @RoleId";

            var dp = new DynamicParameters();
            dp.AddParam("@RoleId", filter.RoleId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<RoleMenuDto>(sql, dp);
            }
        }
    }

    public class RoleMenuDto
    {
        public RoleMenuDto(string roleId, string menuId, 
            int groupOrder, string formType,
            string menuName, string caption)
        {
            RoleId = roleId;
            MenuId = menuId;
            GroupOrder = groupOrder;
            FormType = formType;
            MenuName = menuName;
            Caption = caption;
        }

        public string RoleId { get; set; }
        public string MenuId { get; set; }
        public int GroupOrder { get; set; }
        public string FormType { get; set; }
        public string MenuName { get; set; }
        public string Caption { get; set; }
        
        public static RoleMenuDto FromModel(MenuType model, IRoleKey roleKey)
        {
            var result = new RoleMenuDto(roleKey.RoleId, model.MenuId, 
                model.GroupOrder, model.FormType,
                model.MenuName, model.Caption);
            return result;
        }
        public MenuType ToModel()
        {
            var result = new MenuType(MenuId, GroupOrder, FormType, MenuName, Caption);
            return result;
        }
    }
}
