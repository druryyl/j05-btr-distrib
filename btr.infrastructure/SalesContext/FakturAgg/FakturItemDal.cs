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
                bcp.AddMap("FakturItemId","FakturItemId"); 
                bcp.AddMap("NoUrut","NoUrut"); 

                bcp.AddMap("BrgId","BrgId"); 
                bcp.AddMap("BrgCode","BrgCode"); 
                bcp.AddMap("StokHargaStr","StokHargaStr");
                bcp.AddMap("QtyInputStr", "QtyInputStr");
                bcp.AddMap("QtyDetilStr", "QtyDetilStr");
                bcp.AddMap("HrgInputStr", "HrgInputStr");

                bcp.AddMap("QtyBesar","QtyBesar"); 
                bcp.AddMap("SatBesar","SatBesar"); 
                bcp.AddMap("Conversion","Conversion"); 
                bcp.AddMap("HrgSatBesar","HrgSatBesar"); 

                bcp.AddMap("QtyKecil","QtyKecil"); 
                bcp.AddMap("SatKecil","SatKecil"); 
                bcp.AddMap("HrgSatKecil","HrgSatKecil"); 

                bcp.AddMap("QtyJual","QtyJual"); 
                bcp.AddMap("HrgSat","HrgSat"); 
                bcp.AddMap("SubTotal","SubTotal"); 

                bcp.AddMap("QtyBonus","QtyBonus"); 
                bcp.AddMap("QtyPotStok","QtyPotStok"); 

                bcp.AddMap("DiscInputStr","DiscInputStr"); 
                bcp.AddMap("DiscDetilStr","DiscDetilStr"); 
                bcp.AddMap("DiscRp","DiscRp"); 

                bcp.AddMap("DppProsen", "DppProsen");
                bcp.AddMap("DppRp", "DppRp");
                bcp.AddMap("PpnProsen","PpnProsen"); 
                bcp.AddMap("PpnRp","PpnRp"); 
                bcp.AddMap("Total","Total"); 

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
                aa.QtyInputStr, aa.QtyDetilStr, aa.HrgInputStr,
                aa.QtyBesar, aa.SatBesar, aa.Conversion, aa.HrgSatBesar,                
                aa.QtyKecil, aa.SatKecil, aa.HrgSatKecil,
                aa.QtyJual, aa.HrgSat, aa.SubTotal,
                aa.QtyBonus, aa.QtyPotStok,
                aa.DiscInputStr, aa.DiscDetilStr, aa.DiscRp,
                aa.DppProsen, aa.DppRp,
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