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
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();

                bcp.ColumnMappings.Add("ReturJualId", "ReturJualId");
                bcp.ColumnMappings.Add("ReturJualItemId", "ReturJualItemId");
                bcp.ColumnMappings.Add("NoUrut", "NoUrut");
                bcp.ColumnMappings.Add("BrgId", "BrgId");

                bcp.ColumnMappings.Add("QtyInputStr", "QtyInputStr");
                bcp.ColumnMappings.Add("HrgInputStr", "HrgInputStr");
                bcp.ColumnMappings.Add("QtyHrgDetilStr", "QtyHrgDetilStr");
                bcp.ColumnMappings.Add("DiscInputStr", "DiscInputStr");
                bcp.ColumnMappings.Add("DiscDetilStr", "DiscDetilStr");

                bcp.ColumnMappings.Add("Qty", "Qty");
                bcp.ColumnMappings.Add("HrgSat", "HrgSat");
                bcp.ColumnMappings.Add("SubTotal", "SubTotal");
                bcp.ColumnMappings.Add("DiscRp", "DiscRp");
                bcp.ColumnMappings.Add("PpnRp", "PpnRp");
                bcp.ColumnMappings.Add("PpnProsen", "PpnProsen");
                bcp.ColumnMappings.Add("Total", "Total");

                var fetched = listModel.ToList();
                bcp.DestinationTableName = "BTR_ReturJualItem";
                bcp.BatchSize = fetched.Count;
                //  TODO: Error Duplicate Key saat insert
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IReturJualKey key)
        {
            //  QUERY
            const string sql = @"
                DELETE FROM BTR_ReturJualItem
                WHERE ReturJualId = @ReturJualId";
            
            //  PARAMETER
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", key.ReturJualId, SqlDbType.VarChar);
            
            //  EXECUTE
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
            
        }

        public IEnumerable<ReturJualItemModel> ListData(IReturJualKey filter)
        {
            //  QUERY
            const string sql = @"
                SELECT 
                    ReturJualId, ReturJualItemId, NoUrut, BrgId, 
                    QtyInputStr, HrgInputStr, DiscInputStr, 
                    Qty, HrgSat, SubTotal, DiscRp, PpnRp, PpnProsen, Total
                FROM 
                    BTR_ReturJualItem
                WHERE 
                    ReturJualId = @ReturJualId
                ORDER BY 
                    NoUrut";
            
            //  PARAMETER
            var dp = new DynamicParameters();
            dp.AddParam("@ReturJualId", filter.ReturJualId, SqlDbType.VarChar);

            //  LOAD
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ReturJualItemModel>(sql, dp);
            }
        }
    }
}