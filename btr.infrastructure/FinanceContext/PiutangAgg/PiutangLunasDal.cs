using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace btr.infrastructure.FinanceContext.PiutangAgg
{
    public class PiutangLunasDal : IPiutangLunasDal
    {
        private readonly DatabaseOptions _opt;

        public PiutangLunasDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<PiutangLunasModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();

                bcp.AddMap("PiutangId", "PiutangId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("PelunasanId", "PelunasanId");
                bcp.AddMap("LunasDate", "LunasDate");
                bcp.AddMap("TagihanId", "TagihanId");
                bcp.AddMap("Nilai", "Nilai");
                bcp.AddMap("JenisLunas", "JenisLunas");
                bcp.AddMap("JatuhTempoBg", "JatuhTempoBg");
                bcp.AddMap("NoRekBg", "NoRekBg");
                bcp.AddMap("NamaBank", "NamaBank");
                bcp.AddMap("AtasNamaBank", "AtasNamaBank");


                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_PiutangLunas";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IPiutangKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_PiutangLunas
                WHERE
                    PiutangId = @PiutangId";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", key.PiutangId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<PiutangLunasModel> ListData(IPiutangKey filter)
        {
            const string sql = @"
                SELECT
                    PiutangId, NoUrut, PelunasanId, LunasDate, 
                    TagihanId, Nilai, JenisLunas,
                    JatuhTempoBg, NoRekBg, NamaBank, AtasNamaBank
                FROM
                    BTR_PiutangLunas 
                WHERE
                    PiutangId = @PiutangId";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", filter.PiutangId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangLunasModel>(sql, dp);
            }
        }
    }
}
