using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.PurchaseContext.InvoiceAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.PurchaseContext.InvoiceAgg
{
    public class InvoiceItemDal : IInvoiceItemDal
    {
        private readonly DatabaseOptions _opt;

        public InvoiceItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<InvoiceItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("InvoiceId","InvoiceId");
                bcp.AddMap("InvoiceItemId", "InvoiceItemId");
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
                bcp.AddMap("QtyBonus","QtyBonus");
                bcp.AddMap("QtyPotStok","QtyPotStok");

                bcp.AddMap("DiscInputStr","DiscInputStr");
                bcp.AddMap("DiscDetilStr","DiscDetilStr");
                bcp.AddMap("DiscRp","DiscRp");
                
                bcp.AddMap("PpnProsen","PpnProsen");
                bcp.AddMap("PpnRp","PpnRp");
                bcp.AddMap("Total","Total");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_InvoiceItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IInvoiceKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_InvoiceItem
                WHERE
                    InvoiceId = @InvoiceId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@InvoiceId", key.InvoiceId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<InvoiceItemModel> ListData(IInvoiceKey filter)
        {
            const string sql = @"
                SELECT
                    aa.InvoiceId, aa.InvoiceItemId, aa.NoUrut, aa.BrgId, 
                    aa.QtyInputStr, aa.QtyDetilStr, aa.QtyBesar, aa.SatBesar, aa.Conversion, aa.HppSatBesar,
                    aa.QtyKecil, aa.SatKecil, aa.HppSatKecil, aa.QtyBeli, aa.HppSat, aa.SubTotal,
                    aa.QtyBonus, aa.QtyPotStok, aa.DiscInputStr, aa.DiscDetilStr, aa.DiscRp,
                    aa.PpnProsen, aa.PpnRp, aa.Total,
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(bb.BrgCode, '') AS BrgCode
                FROM                    
                    BTR_InvoiceItem aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId 
                WHERE
                    aa.InvoiceId = @InvoiceId ";
            var dp = new DynamicParameters();
            dp.AddParam("@InvoiceId", filter.InvoiceId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<InvoiceItemModel>(sql, dp);
            }
        }
    }
}