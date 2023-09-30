using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.MutasiAgg
{
    public class MutasiItemDal : IMutasiItemDal
    {
        private readonly DatabaseOptions _opt;

        public MutasiItemDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<MutasiItemModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("MutasiId","MutasiId");
                bcp.AddMap("MutasiItemId", "MutasiItemId");
                bcp.AddMap("NoUrut","NoUrut");
                bcp.AddMap("BrgId","BrgId");
                
                bcp.AddMap("QtyInputStr","QtyInputStr");

                bcp.AddMap("QtyBesar","QtyBesar");
                bcp.AddMap("SatBesar","SatBesar");
                bcp.AddMap("Conversion","Conversion");
                bcp.AddMap("HppBesar", "HppBesar");
                
                bcp.AddMap("QtyKecil","QtyKecil");
                bcp.AddMap("SatKecil","SatKecil");
                bcp.AddMap("HppKecil", "HppKecil");

                bcp.AddMap("StokBesar", "StokBesar");
                bcp.AddMap("StokKecil", "StokKecil");
                
                bcp.AddMap("Qty", "Qty");
                bcp.AddMap("Sat", "Sat");
                bcp.AddMap("Hpp","Hpp");

                bcp.AddMap("QtyDetilStr","QtyDetilStr");
                bcp.AddMap("StokDetilStr","StokDetilStr");
                bcp.AddMap("HppDetilStr","HppDetilStr");
                bcp.AddMap("NilaiSediaan","NilaiSediaan");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_MutasiItem";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IMutasiKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_MutasiItem
                WHERE
                    MutasiId = @MutasiId ";
            
            var dp = new DynamicParameters();
            dp.AddParam("@MutasiId", key.MutasiId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<MutasiItemModel> ListData(IMutasiKey filter)
        {
            const string sql = @"
                SELECT
                    aa.MutasiId, aa.MutasiItemId, aa.NoUrut, aa.BrgId, aa.QtyInputStr, 
                    aa.QtyBesar, aa.SatBesar, aa.Conversion, aa.HppBesar,
                    aa.QtyKecil, aa.SatKecil, aa.HppKecil, 
                    aa.QtyBeli, aa.Sat, aa.Hpp,
                    aa.QtyDetilStr, aa.StokDetilStr, aa.HppDetilStr,
                    aa.NilaiSediaan,
                    ISNULL(bb.BrgName, '') AS BrgName,
                    ISNULL(bb.BrgCode, '') AS BrgCode
                FROM                    
                    BTR_MutasiItem aa
                    LEFT JOIN BTR_Brg bb ON aa.BrgId = bb.BrgId 
                WHERE
                    aa.MutasiId = @MutasiId ";
            var dp = new DynamicParameters();
            dp.AddParam("@MutasiId", filter.MutasiId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<MutasiItemModel>(sql, dp);
            }
        }
    }
}