using btr.domain.InventoryContext.PackingOrderFeature;
using btr.domain.InventoryContext.WarehouseAgg;
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

namespace btr.infrastructure.InventoryContext.PackingOrderFeature
{
    public interface IPackingOrderDepoDal :
        IInsertBulk<PackingOrderDepoDto>,
        IDelete<IPackingOrderKey>,
        IListData<PackingOrderDepoDto, IPackingOrderKey>
    {

    }
    public class PackingOrderDepoDal : IPackingOrderDepoDal
    {
        private readonly DatabaseOptions _opt;
        public PackingOrderDepoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }
        public void Insert(IEnumerable<PackingOrderDepoDto> listDto)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("PackingOrderId", "PackingOrderId");
                bcp.AddMap("DepoId", "DepoId");

                var fetched = listDto.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "BTR_PackingOrderDepo";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IPackingOrderKey key)
        {
            const string sql = @"
                DELETE FROM BTR_PackingOrderDepo
                WHERE PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", key.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<PackingOrderDepoDto> ListData(IPackingOrderKey filter)
        {
            const string sql = @"
                SELECT
                    aa.PackingOrderId, 
                    aa.DepoId, 
                    ISNULL(bb.DepoName, '') AS DepoName
                FROM BTR_PackingOrderDepo aa
                LEFT JOIN BTR_Depo bb ON aa.DepoId = bb.DepoId  
                WHERE PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", filter.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PackingOrderDepoDto>(sql, dp);
            }
        }
    }

    public class PackingOrderDepoDto
    {
        public PackingOrderDepoDto(string packingOrderId, string depoId, string depoName)
        {
            PackingOrderId = packingOrderId;
            DepoId = depoId;
            DepoName = depoName;
        }

        public string PackingOrderId { get; private set; }
        public string DepoId { get; private set; }
        public string DepoName { get; private set; }

        public DepoType ToModel()
        {
            var result = new DepoType(DepoId, DepoName);
            return result;
        }
        public static PackingOrderDepoDto FromModel(DepoType model, string packingOrderId)
        {
            var result = new PackingOrderDepoDto(packingOrderId, model.DepoId, model.DepoName);
            return result;
        }
    }
}
