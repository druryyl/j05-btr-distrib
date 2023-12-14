using btr.application.FinanceContext.TagihanAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.nuna.Infrastructure;
using Dapper;

namespace btr.infrastructure.FinanceContext.TagihanAgg
{
    public class TagihanDal : ITagihanDal
    {
        private readonly DatabaseOptions _opt;

        public TagihanDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(TagihanModel model)
        {
            //  query insert table BTR_Tagihan from model TagihanModel
            const string sql = @"
                INSERT INTO BTR_Tagihan
                (
                    TagihanId,
                    TagihanDate,
                    SalesPersonId,
                    TotalTagihan
                )
                VALUES
                (
                    @TagihanId,
                    @TagihanDate,
                    @SalesPersonId,
                    @TotalTagihan
                )";
            
            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@TagihanId", model.TagihanId, SqlDbType.VarChar);
            dp.AddParam("@TagihanDate", model.TagihanDate, SqlDbType.DateTime);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@TotalTagihan", model.TotalTagihan, SqlDbType.Decimal);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(TagihanModel model)
        {
            //  query update BTR_Tagihan
            const string sql = @"
                UPDATE BTR_Tagihan
                SET
                    TagihanDate = @TagihanDate,
                    SalesPersonId = @SalesPersonId,
                    TotalTagihan = @TotalTagihan
                WHERE
                    TagihanId = @TagihanId";
            
            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@TagihanId", model.TagihanId, SqlDbType.VarChar);
            dp.AddParam("@TagihanDate", model.TagihanDate, SqlDbType.DateTime);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@TotalTagihan", model.TotalTagihan, SqlDbType.Decimal);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(TagihanModel key)
        {
            //  delete BTR_Tagihan
            const string sql = @"
                DELETE FROM BTR_Tagihan
                WHERE
                    TagihanId = @TagihanId";
            
            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@TagihanId", key.TagihanId, SqlDbType.VarChar);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public TagihanModel GetData(ITagihanKey key)
        {
            //  query get data from BTR_Tagihan
            const string sql = @"
                SELECT
                    TagihanId,
                    TagihanDate,
                    SalesPersonId,
                    TotalTagihan
                FROM
                    BTR_Tagihan
                WHERE
                    TagihanId = @TagihanId";
            
            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@TagihanId", key.TagihanId, SqlDbType.VarChar);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<TagihanModel>(sql, dp);
            }
        }

        public IEnumerable<TagihanModel> ListData(Periode filter)
        {
            // query list data from BTR_Tagihan
            const string sql = @"
                SELECT
                    TagihanId,
                    TagihanDate,
                    SalesPersonId,
                    TotalTagihan
                FROM
                    BTR_Tagihan
                WHERE
                    TagihanDate BETWEEN @Tgl1 AND @Tgl2";
            
            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<TagihanModel>(sql, dp);
            }
        }
    }
}
