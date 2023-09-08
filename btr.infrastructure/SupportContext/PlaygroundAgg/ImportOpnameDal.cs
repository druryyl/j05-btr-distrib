using btr.application.SupportContext.PlaygroundAgg;
using btr.domain.SupportContext.PlaygroundAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace btr.infrastructure.SupportContext.PlaygroundAgg
{
    public class ImportOpnameDal : IImportOpnameDal
    {
        private readonly DatabaseOptions _opt;

        public ImportOpnameDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<ImportOpnameModel> ListData()
        {
            const string sql = @"
                SELECT Gudang, BrgCode, BrgName, Qty, Satuan, Nilai
                FROM DUMMY_ImportOpname ";
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<ImportOpnameModel>(sql);
            }
        }
    }
}
