using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using btr.nuna.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace btr.infrastructure.FinanceContext.PiutangAgg
{
    public class PiutangDal : IPiutangDal
    {
        private readonly DatabaseOptions _opt;

        public PiutangDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(PiutangModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Piutang(
                    PiutangId, PiutangDate, DueDate, CustomerId, 
                    Total, Terbayar, Sisa)
                VALUES(
                    @PiutangId, @PiutangDate, @DueDate, @CustomerId, 
                    @Total, @Terbayar, @Sisa)";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", model.PiutangId, SqlDbType.VarChar); 
            dp.AddParam("@PiutangDate", model.PiutangDate, SqlDbType.DateTime); 
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime); 
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar); 
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal); 
            dp.AddParam("@Terbayar", model.Terbayar, SqlDbType.Decimal);
            dp.AddParam("@Sisa", model.Sisa, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(PiutangModel model)
        {
            const string sql = @"
                UPDATE
                    BTR_Piutang
                SET
                    PiutangDate = @PiutangDate, 
                    DueDate = @DueDate, 
                    CustomerId = @CustomerId, 
                    Total = @Total, 
                    Terbayar = @Terbayar, 
                    Sisa = @Sisa
                WHERE
                    PiutangId = @PiutangId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", model.PiutangId, SqlDbType.VarChar);
            dp.AddParam("@PiutangDate", model.PiutangDate, SqlDbType.DateTime);
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@Terbayar", model.Terbayar, SqlDbType.Decimal);
            dp.AddParam("@Sisa", model.Sisa, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IPiutangKey key)
        {
            const string sql = @"
                DELETE FROM
                    BTR_Piutang
                WHERE
                    PiutangId = @PiutangId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", key.PiutangId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public PiutangModel GetData(IPiutangKey key)
        {
            const string sql = @"
                SELECT
                    aa.PiutangId, aa.PiutangDate, aa.DueDate, aa.CustomerId, 
                    aa.Total, aa.Terbayar, aa.Sisa,
                    ISNULL(bb.CustomerName, '') AS CustomerName
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                WHERE
                    PiutangId = @PiutangId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", key.PiutangId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<PiutangModel>(sql, dp);
            }
        }

        public IEnumerable<PiutangModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.PiutangId, aa.PiutangDate, aa.DueDate, aa.CustomerId, 
                    aa.Total, aa.Terbayar, aa.Sisa,
                    ISNULL(bb.CustomerName, '') AS CustomerName
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                WHERE
                    PiutangDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangModel>(sql, dp);
            }
        }
    }
}
