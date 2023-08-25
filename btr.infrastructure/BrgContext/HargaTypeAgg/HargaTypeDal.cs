using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.BrgContext.HargaTypeAgg;
using btr.domain.BrgContext.HargaTypeAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.BrgContext.HargaTypeAgg
{
    public class HargaTypeDal : IHargaTypeDal
    {
        private readonly DatabaseOptions _opt;

        public HargaTypeDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(HargaTypeModel model)
        {
            const string sql = @"
                INSERT INTO BTR_HargaType (HargaTypeId, HargaTypeName)
                VALUES (@HargaTypeId, @HargaTypeName)";

            var dp = new DynamicParameters();
            dp.AddParam("@HargaTypeId", model.HargaTypeId, SqlDbType.VarChar);
            dp.AddParam("@HargaTypeName", model.HargaTypeName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(HargaTypeModel model)
        {
            const string sql = @"
                UPDATE  
                    BTR_HargaType 
                SET     
                    HargaTypeName = @HargaTypeName
                WHERE
                    HargaTypeId= @HargaTypeId ";

            var dp = new DynamicParameters();
            dp.AddParam("@HargaTypeId", model.HargaTypeId, SqlDbType.VarChar);
            dp.AddParam("@HargaTypeName", model.HargaTypeName, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IHargaTypeKey key)
        {
            const string sql = @"
                DELETE FROM  
                    BTR_HargaType 
                WHERE
                    HargaTypeId= @HargaTypeId ";

            var dp = new DynamicParameters();
            dp.AddParam("@HargaTypeId", key.HargaTypeId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public HargaTypeModel GetData(IHargaTypeKey key)
        {
            const string sql = @"
                SELECT
                    HargaTypeId, HargaTypeName
                FROM
                    BTR_HargaType 
                WHERE
                    HargaTypeId= @HargaTypeId ";

            var dp = new DynamicParameters();
            dp.AddParam("@HargaTypeId", key.HargaTypeId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<HargaTypeModel>(sql, dp);
            }
        }

        public IEnumerable<HargaTypeModel> ListData()
        {
            const string sql = @"
                SELECT
                    HargaTypeId, HargaTypeName
                FROM
                    BTR_HargaType  ";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<HargaTypeModel>(sql);
            }
        }
    }
}
