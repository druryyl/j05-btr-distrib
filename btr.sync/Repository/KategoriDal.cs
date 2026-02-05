using Dapper;
using j07_btrade_sync.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync.Repository
{
    public class KategoriDal
    {
        public IEnumerable<KategoriType> ListData()
        {
            const string sql = @"
                SELECT
                    aa.KategoriId, aa.KategoriName, '     ' AS ServerId
                FROM
                    BTR_Kategori aa";

            using (var conn = new SqlConnection(ConnStringHelper.Get()))
            {
                var result = conn.Query<KategoriType>(sql).ToList();
                return result;
            }
    }
    }
}
