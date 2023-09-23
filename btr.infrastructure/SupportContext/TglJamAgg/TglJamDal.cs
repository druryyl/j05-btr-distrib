using btr.application.SupportContext.TglJamAgg;
using btr.infrastructure.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;

namespace btr.infrastructure.SupportContext.TglJamAgg
{
    public class TglJamDal : ITglJamDal
    {
        private readonly DatabaseOptions _opt;

        public TglJamDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public DateTime Now 
        { 
            get 
            {
                DateTime result;
                const string sql = @"SELECT GETDATE() TglJam";
                using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        result = Convert.ToDateTime(dr["TglJam"]);
                    }
                }
                return result;
            }
        }
    }
}
