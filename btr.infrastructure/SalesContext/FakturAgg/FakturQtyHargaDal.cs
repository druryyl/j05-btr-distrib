using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext
{
    public class FakturQtyHargaDal : IFakturQtyHargaDal
    {
        private readonly DatabaseOptions _opt;

        public FakturQtyHargaDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<FakturQtyHargaModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("FakturItemId", "FakturItemId");
                bcp.AddMap("FakturQtyHargaId", "FakturQtyHargaId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("Satuan", "Satuan");
                bcp.AddMap("Conversion", "Conversion");
                bcp.AddMap("Qty", "Qty");
                bcp.AddMap("HargaJual", "HargaJual");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_FakturQtyHarga";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IFakturKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_FakturQtyHarga
            WHERE
                FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<FakturQtyHargaModel> ListData(IFakturKey fakturKey)
        {
            const string sql = @"
            SELECT
                aa.FakturId, aa.FakturItemId, aa.FakturQtyHargaId,
                aa.NoUrut, aa.BrgId, aa.Satuan,
                aa.Conversion, aa.Qty, aa.HargaJual
            FROM 
                BTR_FakturQtyHarga aa
            WHERE
                aa.FakturId = @FakturId ";
            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", fakturKey.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturQtyHargaModel>(sql, dp);
            }
        }
    }
}