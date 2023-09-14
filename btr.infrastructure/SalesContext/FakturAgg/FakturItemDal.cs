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

namespace btr.infrastructure.SalesContext.FakturAgg
{
    public class FakturItemDal : IFakturItemDal
    {
        private readonly DatabaseOptions _opt;

        public FakturItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<FakturItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("FakturItemId", "FakturItemId");
                bcp.AddMap("NoUrut", "NoUrut");
                
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("BrgCode", "BrgCode");
                bcp.AddMap("StokHargaStr", "StokHargaStr");
                
                bcp.AddMap("QtyInputStr", "QtyInputStr");
                bcp.AddMap("QtyDetilStr", "QtyDetilStr");
                bcp.AddMap("QtyPotStok", "QtyPotStok");
                bcp.AddMap("QtyJual", "QtyJual");
                
                bcp.AddMap("HargaSatuan", "HargaSatuan");
                bcp.AddMap("SubTotal", "SubTotal");
                
                bcp.AddMap("DiscInputStr", "DiscInputStr");
                bcp.AddMap("DiscDetilStr", "DiscDetilStr");
                bcp.AddMap("DiscRp", "DiscRp");

                bcp.AddMap("PpnProsen", "PpnProsen");
                bcp.AddMap("PpnRp", "PpnRp");
                bcp.AddMap("Total", "Total");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_FakturItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IFakturKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_FakturItem
            WHERE
                FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<FakturItemModel> ListData(IFakturKey fakturKey)
        {
            const string sql = @"
            SELECT
                aa.FakturId, aa.FakturItemId, aa.NoUrut, 
                aa.BrgId, aa.BrgCode, aa.StokHargaStr, 
                aa.QtyInputStr, aa.QtyDetilStr, aa.QtyPotStok, aa.QtyJual, 
                aa.HargaSatuan, SubTotal,
                aa.DiscInputStr, aa.DiscDetilStr, aa.DiscRp,
                aa.PpnProsen, aa.PpnRp, aa.Total,
                ISNULL(bb.BrgName, '') AS BrgName
            FROM 
                BTR_FakturItem aa
                LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId
            WHERE
                aa.FakturId = @FakturId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", fakturKey.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturItemModel>(sql, dp);
            }
        }
    }
}