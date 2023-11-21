using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.ReturJualAgg
{
    public class ReturJualItemDal : IReturJualItemDal
    {
        private readonly DatabaseOptions _opt;

        public ReturJualItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<ReturJualItemModel> listModel)
        {
            //  create insert using sql bulk copy
            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("ReturJualId", "ReturJualId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("QtyInputStr", "QtyInputStr");
                bcp.AddMap("QtyBesar", "QtyBesar");
                bcp.AddMap("SatBesar", "SatBesar");
                bcp.AddMap("HrgSatBesar", "HrgSatBesar");
                bcp.AddMap("Conversion", "Conversion");
                bcp.AddMap("QtyKecil", "QtyKecil");
                bcp.AddMap("SatKecil", "SatKecil");
                bcp.AddMap("HrgSatKecil", "HrgSatKecil");
                bcp.AddMap("Qty", "Qty");
                bcp.AddMap("HrgSat", "HrgSat");
                bcp.AddMap("SubTotal", "SubTotal");
                bcp.AddMap("DiscRp", "DiscRp");
                bcp.AddMap("PpnRp", "PpnRp");
                bcp.AddMap("Total", "Total");
                
                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "BTR_ReturJualItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IReturJualKey key)
        {
            //  create delete query for ReturJualItemModel from table BTR_ReturJualItem
            const string sql = @"
                DELETE FROM BTR_ReturJualItem
                WHERE ReturJualId = @ReturJualId";
            
            //  assign parameter in query to key using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, SqlDbType.VarChar);
            
            //  execute query using dapper
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
            
        }

        public IEnumerable<ReturJualItemModel> ListData(IReturJualKey filter)
        {
            throw new NotImplementedException();
            //  create query select for ReturJualItemModel from table BTR_ReturJualItem
            //    const string sql = @"
            //        SELECT 
            //            ReturJualId, 
            //            NoUrut, 
            //            BrgId, 
            //            QtyInputStr, 
            //            QtyBesar, 
            //            SatBesar, 
            //            HrgSatBesar, 
            //            Conversion, 
            //            QtyKecil, 
            //            SatKecil, 
            //            HrgSatKecil, 
            //            Qty, 
            //            HrgSat, 
            //            SubTotal, 
            //            DiscRp, 
            //            PpnRp, 
            //            Total
            //        FROM 
            //            BTR_ReturJualItem
            //        WHERE 
            //            ReturJualId = @ReturJualId
            //        ORDER BY 
            //            NoUrut ASC";
            //    return null;
        }
    }
}