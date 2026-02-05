using Dapper;
using j07_btrade_sync.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Repository
{
    public class SalesPersonDal
    {
        public IEnumerable<SalesPersonType> ListData()
        {
            const string sql = @"
                SELECT
                    aa.SalesPersonId, aa.SalesPersonCode, aa.SalesPersonName, '     ' AS ServerId
                FROM
                    BTR_SalesPerson aa";
            using (var conn = new System.Data.SqlClient.SqlConnection(ConnStringHelper.Get()))
            {
                var result = conn.Query<SalesPersonType>(sql).ToList();
                return result;
            }
        }
    }
}
