using btr.domain.InventoryContext.PackingOrderFeature;
using btr.infrastructure.Helpers;
using btr.infrastructure.InventoryContext.PackingOrderFeature;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BtrGudang.Infrastructure.PackingOrderFeature
{
    public interface IPackingOrderItemDal :
        IInsertBulk<PackingOrderItemDto>,
        IDelete<IPackingOrderKey>,
        IListData<PackingOrderItemDto, IPackingOrderKey>
    {

    }
    public class PackingOrderItemDal : IPackingOrderItemDal
    {
        private readonly DatabaseOptions _opt;
        public PackingOrderItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }
        public void Insert(IEnumerable<PackingOrderItemDto> listDto)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("PackingOrderId","PackingOrderId");
                bcp.AddMap("NoUrut","NoUrut");
                bcp.AddMap("BrgId","BrgId");
                bcp.AddMap("BrgCode","BrgCode");
                bcp.AddMap("BrgName","BrgName");
                bcp.AddMap("QtyBesar", "QtyBesar");
                bcp.AddMap("SatBesar", "SatBesar");
                bcp.AddMap("QtyKecil", "QtyKecil");
                bcp.AddMap("SatKecil", "SatKecil");

                var fetched = listDto.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "BTR_PackingOrderItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IPackingOrderKey key)
        {
            const string sql = @"
                DELETE FROM BTR_PackingOrderItem
                WHERE PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", key.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<PackingOrderItemDto> ListData(IPackingOrderKey filter)
        {
            const string sql = @"
                SELECT
                    PackingOrderId, NoUrut, 
                    BrgId, BrgCode, BrgName,
                    QtyBesar, SatBesar,
                    QtyKecil, SatKecil
                FROM BTR_PackingOrderItem
                WHERE PackingOrderId = @PackingOrderId
                ORDER BY NoUrut
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", filter.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PackingOrderItemDto>(sql, dp);
            }
        }
    }
}
