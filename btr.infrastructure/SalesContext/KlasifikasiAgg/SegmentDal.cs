using btr.domain.SalesContext.KlasifikasiAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace btr.infrastructure.SalesContext.KlasifikasiAgg
{
    public interface ISegmentDal :
        IInsert<SegmentType>,
        IUpdate<SegmentType>,
        IDelete<ISegmentKey>,
        IGetData<SegmentType, ISegmentKey>,
        IListData<SegmentType>
    {
    }
    public class SegmentDal : ISegmentDal
    {
        private readonly DatabaseOptions _opt;

        public SegmentDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(SegmentType model)
        {
            const string sql = @"
                INSERT INTO BTR_Segment (SegmentId, SegmentName) 
                VALUES (@SegmentId, @SegmentName)";

            var dp = new DynamicParameters();
            dp.AddParam("@SegmentId", model.SegmentId, SqlDbType.VarChar);
            dp.AddParam("@SegmentName", model.SegmentName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }
        public void Update(SegmentType model)
        {
            const string sql = @"
                UPDATE BTR_Segment 
                SET SegmentName = @SegmentName
                WHERE SegmentId = @SegmentId";

            var dp = new DynamicParameters();
            dp.AddParam("@SegmentId", model.SegmentId, SqlDbType.VarChar);
            dp.AddParam("@SegmentName", model.SegmentName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }

        }
        public void Delete(ISegmentKey key)
        {
            const string sql = @"
                    DELETE FROM BTR_Segment 
                    WHERE SegmentId = @SegmentId";

            var dp = new DynamicParameters();
            dp.AddParam("@SegmentId", key.SegmentId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }
        public SegmentType GetData(ISegmentKey key)
        {
            const string sql = @"
                SELECT SegmentId, SegmentName
                FROM BTR_Segment 
                WHERE SegmentId = @SegmentId";
            var dp = new DynamicParameters();
            dp.AddParam("@SegmentId", key.SegmentId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<SegmentType>(sql, dp).FirstOrDefault();
            }

        }
        public IEnumerable<SegmentType> ListData()
        {
            const string sql = @"
                SELECT SegmentId, SegmentName
                FROM BTR_Segment";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Query<SegmentType>(sql).ToList();
            }
        }
    }
}
