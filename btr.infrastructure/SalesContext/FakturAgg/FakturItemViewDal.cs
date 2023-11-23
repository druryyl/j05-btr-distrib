using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.FakturAgg
{
    public class FakturItemViewDal : IFakturItemViewDal
    {
        private readonly DatabaseOptions _opt;

        public FakturItemViewDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<FakturItemView> ListData(ICustomerKey customerKey, 
            IBrgKey brgKey, Periode periode)
        {
            //  QUERY
            const string sql = @"
                SELECT
                    aa.FakturId, bb.FakturDate, aa.BrgId, aa.HargaJual
                FROM
                    BTR_FakturItem aa
                    INNER JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                WHERE
                    bb.FakturDate BETWEEN @Tgl1 AND @Tgl2 
                    AND CustomerId = @CustomerId
                    AND BrgId = @BrgId ";
            
            //  assign parameter using dapper
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", periode.Tgl2, SqlDbType.DateTime);
            dp.AddParam("@CustomerId", customerKey.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@BrgId", brgKey.BrgId, SqlDbType.VarChar);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturItemView>(sql, dp);
            }
        }
    }
}