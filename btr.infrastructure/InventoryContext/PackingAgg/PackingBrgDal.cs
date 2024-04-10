using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.PackingAgg
{
    public class PackingBrgDal : IPackingBrgDal
    {
        private readonly DatabaseOptions _opt;

        public PackingBrgDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<PackingBrgModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("PackingId", "PackingId");
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("SupplierId", "SupplierId");
                bcp.AddMap("BrgId", "BrgId");

                bcp.AddMap("QtyKecil", "QtyKecil");
                bcp.AddMap("SatKecil", "SatKecil");
                bcp.AddMap("QtyBesar", "QtyBesar");
                bcp.AddMap("SatBesar", "SatBesar");
                bcp.AddMap("HargaJual", "HargaJual");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_PackingBrg";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IPackingKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_PackingBrg
            WHERE
                PackingId = @PackingId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", key.PackingId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<PackingBrgModel> ListData(IPackingKey stok)
        {
            const string sql = @"
            SELECT
                aa.PackingId, aa.FakturId, aa.SupplierId, aa.BrgId, 
                aa.QtyKecil, aa.SatKecil, aa.QtyBesar, aa.SatBesar,
                aa.HargaJual,
                ISNULL(bb.SupplierName, '') AS SupplierName,
                ISNULL(cc.BrgName, '') AS BrgName,
                ISNULL(cc.BrgCode, '') AS BrgCode
            FROM 
                BTR_PackingBrg aa
                LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                LEFT JOIN BTR_Brg cc ON aa.BrgId = cc.BrgId
            WHERE
                aa.PackingId = @PackingId ";
            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", stok.PackingId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PackingBrgModel>(sql, dp);
            }
        }
    }
}