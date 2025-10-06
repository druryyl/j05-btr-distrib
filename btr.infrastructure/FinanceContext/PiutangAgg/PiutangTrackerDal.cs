using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
namespace btr.infrastructure.FinanceContext.PiutangAgg
{
    public class PiutangTrackerDal : IPiutangTrackerDal
    {
        private readonly DatabaseOptions _opt;
        public PiutangTrackerDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }
        public IEnumerable<PiutangTrackerDto> ListData(IPiutangKey filter)
        {
            const string sql = @"
                SELECT 'Catat Piutang' AS Keterangan, PiutangDate AS Tgl, PiutangId AS ReffId, 
                        Total AS Piutang, 0 AS Tagihan, 0 AS Pelunasan, 0 as Flag
                FROM BTR_Piutang
                WHERE PiutangId = @FakturId
                UNION ALL

                SELECT 'Potongan' AS Keterangan, PiutangDate AS Tgl, PiutangId AS ReffId, 
                        -Potongan AS Piutang, 0 AS Tagihan, 0 AS Pelunasan, 0 as Flag
                FROM BTR_Piutang
                WHERE PiutangId = @FakturId
                UNION ALL

                SELECT 'Tagihan' AS Keterangan, aa.TagihanDate AS Tgl, aa.TagihanId AS ReffId, 
                        0 as Piutang, bb.NilaiTagih AS Tagihan, 0 Pelunasan, 1 as Flag
                FROM BTR_Tagihan aa
                INNER JOIN BTR_TagihanFaktur bb ON aa.TagihanId = bb.TagihanId
                WHERE bb.FakturId = @FakturId   
                UNION ALL

                SELECT 'Tanda Terima' AS Keterangan, aa.TandaTerimaDate AS Tgl, aa.TagihanId AS ReffId, 
                        0 as Piutang, 0 AS Tagihan, 0 Pelunasan, 1 as Flag
                FROM BTR_TagihanFaktur aa 
                WHERE IsTandaTerima = 1
                AND aa.FakturId = @FakturId
                UNION ALL

                SELECT 'Tagih Ulang' AS Keterangan, aa.TagihanDate AS Tgl, aa.TagihanId AS ReffId, 
                        0 as Piutang, 0 AS Tagihan, 0 Pelunasan, 1 as Flag
                FROM BTR_Tagihan aa
                INNER JOIN BTR_TagihanFaktur bb ON aa.TagihanId = bb.TagihanId
                WHERE IsTagihUlang = 1
                AND bb.FakturId = @FakturId
                UNION ALL

                SELECT 'Pelunasan' AS Keterangan, aa.LunasDate AS Tgl, aa.PelunasanId AS ReffId, 
                    0 AS Piutang, 0 as Tagihan, aa.Nilai AS Pelunasan, 1 as Flag
                FROM BTR_PiutangLunas aa
                WHERE aa.PiutangId = @FakturId

                ORDER BY Flag, Tgl";

            var dp = new DynamicParameters();
            dp.Add("@FakturId", filter.PiutangId);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangTrackerDto>(sql, dp);
            }
        }
    }
}
