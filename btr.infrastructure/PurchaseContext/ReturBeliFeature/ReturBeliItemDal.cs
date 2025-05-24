using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.PurchaseContext.ReturBeliFeature;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.ReturBeliAgg
{
    public class ReturBeliItemDal : IReturBeliItemDal
    {
        private readonly DatabaseOptions _opt;

        public ReturBeliItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<ReturBeliItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("ReturBeliId","ReturBeliId");
                bcp.AddMap("ReturBeliItemId", "ReturBeliItemId");
                bcp.AddMap("NoUrut","NoUrut");
                bcp.AddMap("BrgId","BrgId");
                bcp.AddMap("QtyInputStr","QtyInputStr");
                bcp.AddMap("QtyDetilStr","QtyDetilStr");

                bcp.AddMap("HrgInputStr","HrgInputStr");
                bcp.AddMap("HrgDetilStr", "HrgDetilStr");

                bcp.AddMap("QtyBesar","QtyBesar");
                bcp.AddMap("SatBesar","SatBesar");
                bcp.AddMap("Conversion","Conversion");
                bcp.AddMap("HppSatBesar", "HppSatBesar");
                
                bcp.AddMap("QtyKecil","QtyKecil");
                bcp.AddMap("SatKecil","SatKecil");
                bcp.AddMap("HppSatKecil", "HppSatKecil");
                
                bcp.AddMap("QtyBeli", "QtyBeli");
                bcp.AddMap("HppSat", "HppSat");
                bcp.AddMap("SubTotal","SubTotal");

                bcp.AddMap("DiscInputStr","DiscInputStr");
                bcp.AddMap("DiscDetilStr","DiscDetilStr");
                bcp.AddMap("DiscRp","DiscRp");
                
                bcp.AddMap("PpnProsen","PpnProsen");
                bcp.AddMap("PpnRp","PpnRp");
                bcp.AddMap("Total","Total");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_ReturBeliItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IReturBeliKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_ReturBeliItem
                WHERE
                    ReturBeliId = @ReturBeliId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@ReturBeliId", key.ReturBeliId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<ReturBeliItemModel> ListData(IReturBeliKey filter)
        {
            const string sql = @"
                SELECT
                    aa.ReturBeliId, aa.ReturBeliItemId, aa.NoUrut, aa.BrgId, 
                    aa.HrgInputStr, aa.HrgDetilStr,
                    aa.QtyInputStr, aa.QtyDetilStr, aa.QtyBesar, aa.SatBesar, aa.Conversion, aa.HppSatBesar,
                    aa.QtyKecil, aa.SatKecil, aa.HppSatKecil, aa.QtyBeli, aa.HppSat, aa.SubTotal,
                    aa.DiscInputStr, aa.DiscDetilStr, aa.DiscRp,
                    aa.PpnProsen, aa.PpnRp, aa.Total,
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(bb.BrgCode, '') AS BrgCode
                FROM                    
                    BTR_ReturBeliItem aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId 
                WHERE
                    aa.ReturBeliId = @ReturBeliId ";
            var dp = new DynamicParameters();
            dp.AddParam("@ReturBeliId", filter.ReturBeliId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturBeliItemModel>(sql, dp);
            }
        }

    }
}