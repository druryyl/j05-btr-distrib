using btr.application.FinanceContext.FpKeluaragAgg;
using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using btr.nuna.Infrastructure;
using Dapper;
using System.Data;

namespace btr.infrastructure.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranFakturDal : IFpKeluaranFakturDal
    {
        private readonly DatabaseOptions _opt;

        public FpKeluaranFakturDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<FpKeluaranFakturModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("FpKeluaranFakturId", "FpKeluaranFakturId"); 
                bcp.AddMap("FpKeluaranId","FpKeluaranId"); 
                bcp.AddMap("FakturId","FakturId"); 
                bcp.AddMap("Baris","Baris"); 
                bcp.AddMap("TanggalFaktur","TanggalFaktur"); 
                bcp.AddMap("JenisFaktur","JenisFaktur"); 
                bcp.AddMap("KodeTransaksi","KodeTransaksi"); 
                bcp.AddMap("KeteranganTambahan","KeteranganTambahan"); 
                bcp.AddMap("DokumenPendukung","DokumenPendukung"); 
                bcp.AddMap("Referensi","Referensi"); 
                bcp.AddMap("CapFasilitas","CapFasilitas"); 
                bcp.AddMap("IdTkuPenjual","IdTkuPenjual"); 
                bcp.AddMap("NpwpNikPembeli","NpwpNikPembeli"); 
                bcp.AddMap("JenisIdPembeli","JenisIdPembeli"); 
                bcp.AddMap("NegaraPembeli","NegaraPembeli"); 
                bcp.AddMap("NomorDokumenPembeli","NomorDokumenPembeli"); 
                bcp.AddMap("NamaPembeli","NamaPembeli"); 
                bcp.AddMap("AlamatPembeli","AlamatPembeli"); 
                bcp.AddMap("EmailPembeli","EmailPembeli"); 
                bcp.AddMap("IdTkuPembeli","IdTkuPembeli"); 

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_FpKeluaranFaktur";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IFpKeluaranKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_FpKeluaranFaktur
                WHERE
                    FpKeluaranId = @FpKeluaranId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", key.FpKeluaranId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<FpKeluaranFakturModel> ListData(IFpKeluaranKey filter)
        {
            const string sql = @"
                SELECT
                    FpKeluaranFakturId, FpKeluaranId, FakturId, Baris, 
                    TanggalFaktur, JenisFaktur, KodeTransaksi, KeteranganTambahan, 
                    DokumenPendukung, Referensi, CapFasilitas, IdTkuPenjual, 
                    NpwpNikPembeli, JenisIdPembeli, NegaraPembeli, 
                    NomorDokumenPembeli, NamaPembeli, 
                    AlamatPembeli, EmailPembeli, IdTkuPembeli
                FROM
                    BTR_FpKeluaranFaktur
                WHERE
                    FpKeluaranId = @FpKeluaranId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", filter.FpKeluaranId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FpKeluaranFakturModel>(sql, dp);
            }
        }
    }
}
