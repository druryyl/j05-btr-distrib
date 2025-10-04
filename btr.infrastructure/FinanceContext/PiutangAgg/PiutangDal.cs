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
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.SalesPersonAgg;

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
                    StatusPiutang, Total, Potongan, Terbayar, Sisa)
                VALUES(
                    @PiutangId, @PiutangDate, @DueDate, @CustomerId, 
                    @StatusPiutang, @Total, @Potongan, @Terbayar, @Sisa)";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", model.PiutangId, SqlDbType.VarChar); 
            dp.AddParam("@PiutangDate", model.PiutangDate, SqlDbType.DateTime); 
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime); 
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar); 
            dp.AddParam("@StatusPiutang", (int)model.StatusPiutang, SqlDbType.Int);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal); 
            dp.AddParam("@Potongan", model.Potongan, SqlDbType.Decimal); 
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
                    StatusPiutang = @StatusPiutang,
                    Total = @Total, 
                    Potongan = @Potongan,
                    Terbayar = @Terbayar, 
                    Sisa = @Sisa
                WHERE
                    PiutangId = @PiutangId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PiutangId", model.PiutangId, SqlDbType.VarChar);
            dp.AddParam("@PiutangDate", model.PiutangDate, SqlDbType.DateTime);
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@StatusPiutang", (int)model.StatusPiutang, SqlDbType.Int);
            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@Potongan", model.Potongan, SqlDbType.Decimal);
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
                    aa.PiutangId, aa.PiutangDate, aa.DueDate, aa.CustomerId, aa.StatusPiutang,
                    aa.Total, aa.Potongan, aa.Terbayar, aa.Sisa,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(bb.CustomerCode, '') AS CustomerCode,
                    ISNULL(bb.Address1, '') AS Address,
                    ISNULL(cc.FakturCode, '') AS FakturCode
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Faktur cc ON aa.PiutangId = cc.FakturId
                WHERE
                    aa.PiutangId = @PiutangId ";

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
                    aa.PiutangId, aa.PiutangDate, aa.DueDate, aa.CustomerId, aa.StatusPiutang,
                    aa.Total, aa.Potongan, aa.Terbayar, aa.Sisa,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(bb.CustomerCode, '') AS CustomerCode,
                    ISNULL(bb.Address1, '') AS Address,
                    ISNULL(cc.FakturCode, '') AS FakturCode
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Faktur cc ON aa.PiutangId = cc.FakturId
                WHERE
                    aa.PiutangDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangModel>(sql, dp);
            }
        }

        public IEnumerable<PiutangModel> ListData(IEnumerable<IPiutangKey> filter)
        {
            const string sql = @"
                SELECT
                    aa.PiutangId, aa.PiutangDate, aa.DueDate, aa.CustomerId, aa.StatusPiutang,
                    aa.Total, aa.Potongan, aa.Terbayar, aa.Sisa,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(bb.CustomerCode, '') AS CustomerCode,
                    ISNULL(bb.Address1, '') AS Address,
                    ISNULL(cc.FakturCode, '') AS FakturCode
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Faktur cc ON aa.PiutangId = cc.FakturId
                WHERE
                    aa.PiutangId IN @listPiutangId";

            var result = new List<PiutangModel>();
            var batchSize = 2000;

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                foreach (var batch in filter.Select(x => x.PiutangId).Chunk(batchSize))
                {
                    var dp = new DynamicParameters();
                    dp.Add("@listPiutangId", batch);
                    result.AddRange(conn.Read<PiutangModel>(sql, dp));
                }
            }
            return result;
        }

        public IEnumerable<PiutangModel> ListData(ICustomerKey filter, Periode periode)
        {
            const string sql = @"
                SELECT
                    aa.PiutangId, aa.PiutangDate, aa.DueDate, aa.CustomerId, aa.StatusPiutang,
                    aa.Total, aa.Potongan, aa.Terbayar, aa.Sisa,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(bb.CustomerCode, '') AS CustomerCode,
                    ISNULL(bb.Address1, '') AS Address,
                    ISNULL(cc.FakturCode, '') AS FakturCode
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Faktur cc ON aa.PiutangId = cc.FakturId
                WHERE
                    aa.CustomerId = @CustomerId 
                    AND aa.PiutangDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@CustomerId", filter.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@Tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", periode.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangModel>(sql, dp);
            }
        }

        public IEnumerable<PiutangModel> ListData(ISalesPersonKey filter, Periode periode)
        {
            const string sql = @"
                SELECT
                    aa.PiutangId, aa.PiutangDate, aa.DueDate, aa.CustomerId, aa.StatusPiutang,
                    aa.Total, aa.Potongan, aa.Terbayar, aa.Sisa,
                    ISNULL(bb.CustomerName, '') AS CustomerName,
                    ISNULL(bb.CustomerCode, '') AS CustomerCode,
                    ISNULL(bb.Address1, '') AS Address,
                    ISNULL(cc.FakturCode, '') AS FakturCode
                FROM
                    BTR_Piutang aa
                    LEFT JOIN BTR_Customer bb ON aa.CustomerId = bb.CustomerId
                    LEFT JOIN BTR_Faktur cc ON aa.PiutangId = cc.FakturId
                WHERE
                    cc.SalesPersonId = @SalesId 
                    AND aa.PiutangDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@SalesId ", filter.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@Tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", periode.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PiutangModel>(sql, dp);
            }
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            while (source.Any())
            {
                yield return source.Take(chunkSize);
                source = source.Skip(chunkSize);
            }
        }
    }
}
