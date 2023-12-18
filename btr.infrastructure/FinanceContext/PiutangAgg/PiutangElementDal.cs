using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.FinanceContext.PiutangAgg
{
    public class PiutangElementDal : IPiutangElementDal
    {
        private readonly DatabaseOptions _opt;

        public PiutangElementDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<PiutangElementModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();

                bcp.AddMap("PiutangId","PiutangId");
                bcp.AddMap("NoUrut","NoUrut");
                bcp.AddMap("ElementTag","ElementTag");
                bcp.AddMap("ElementName","ElementName");
                bcp.AddMap("NilaiPlus","NilaiPlus");
                bcp.AddMap("NilaiMinus", "NilaiMinus");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_PiutangElement";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IPiutangKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_PiutangElement
                WHERE
                    PiutangId = @PiutangId";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", key.PiutangId, System.Data.SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<PiutangElementModel> ListData(IPiutangKey filter)
        {
            const string sql = @"
                SELECT
                    PiutangId, NoUrut, ElementTag, ElementName, NilaiPlus, NilaiMinus
                FROM
                    BTR_PiutangElement 
                WHERE
                    PiutangId = @PiutangId";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", filter.PiutangId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangElementModel>(sql, dp);
            }
        }
    }
}
