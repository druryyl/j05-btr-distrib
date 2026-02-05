using Dapper;
using j07_btrade_sync.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Repository
{
    public class WilayahDal
    {
        public IEnumerable<WilayahType> ListData()
        {
            const string sql = @"
                SELECT
                    aa.WilayahId, aa.WilayahName, '     ' AS ServerId
                FROM
                    BTR_Wilayah aa";
            using (var conn = new System.Data.SqlClient.SqlConnection(ConnStringHelper.Get()))
            {
                var result = conn.Query<WilayahType>(sql).ToList();
                return result;
            }
        }
    }
}
