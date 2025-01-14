using btr.application.FinanceContext.FpKeluaragAgg;
using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Infrastructure;
using Dapper;

namespace btr.infrastructure.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranBrgDal : IFpKeluaranBrgDal
    {
        private readonly DatabaseOptions _opt;

        public FpKeluaranBrgDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<FpKeluaranBrgModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("FpKeluaranBrgId", "FpKeluaranBrgId");
                bcp.AddMap("FpKeluaranFakturId", "FpKeluaranFakturId");
                bcp.AddMap("FpKeluaranId", "FpKeluaranId");
                bcp.AddMap("FakturId", "FakturId");

                bcp.AddMap("Baris", "Baris");
                bcp.AddMap("BarangJasa", "BarangJasa");
                bcp.AddMap("KodeBarangJasa", "KodeBarangJasa");
                bcp.AddMap("NamaBarangJasa", "NamaBarangJasa");
                bcp.AddMap("NamaSatuanUkur", "NamaSatuanUkur");
                bcp.AddMap("HargaSatuan", "HargaSatuan");
                bcp.AddMap("JumlahBarangJasa", "JumlahBarangJasa");
                bcp.AddMap("TotalDiskon", "TotalDiskon");
                bcp.AddMap("Dpp", "Dpp");
                bcp.AddMap("DppLain", "DppLain");
                bcp.AddMap("TarifPpn", "TarifPpn");
                bcp.AddMap("Ppn", "Ppn");
                bcp.AddMap("TarifPpnBm", "TarifPpnBm");
                bcp.AddMap("PpnBm", "PpnBm");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_FpKeluaranBrg";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IFpKeluaranKey key)
        {
            const string sql = @"
                DELETE FROM BTR_FpKeluaranBrg
                WHERE FpKeluaranId = @FpKeluaranId";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", key.FpKeluaranId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<FpKeluaranBrgModel> ListData(IFpKeluaranKey filter)
        {
            const string sql = @"
                SELECT
                    FpKeluaranBrgModelId, FpKeluaranFakturId, FpKeluaranId, FakturId, 
                    Baris, BarangJasa, KodeBarangJasa, NamaBarangJasa, NamaSatuanUkur, HargaSatuan, 
                    JumlahBarangJasa, TotalDiskon, Dpp, DppLain, TarifPpn, Ppn, TarifPpnBm, PpnBm
                FROM
                    BTR_FpKeluaranBrg
                WHERE
                    FpKeluaranId = @FpKeluaranId";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", filter.FpKeluaranId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FpKeluaranBrgModel>(sql, dp);
            }
        }
    }
}
