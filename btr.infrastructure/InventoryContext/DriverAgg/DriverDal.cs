using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.DriverAgg;
using btr.domain.InventoryContext.DriverAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.DriverAgg
{
    public class DriverDal : IDriverDal
    {
        private readonly DatabaseOptions _opt;

        public DriverDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(DriverModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Driver(
                DriverId, DriverName, IsAktif)
            VALUES (
                @DriverId, @DriverName, @IsAktif)";

            var dp = new DynamicParameters();
            dp.AddParam("@DriverId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@DriverName", model.DriverName, SqlDbType.VarChar);
            dp.AddParam("@IsAktif", model.IsAktif, SqlDbType.Bit);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(DriverModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Driver
            SET
                DriverName = @DriverName,
                IsAktif = @IsAktif
            WHERE
                DriverId = @DriverId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DriverId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@DriverName", model.DriverName, SqlDbType.VarChar);
            dp.AddParam("@IsAktif", model.IsAktif, SqlDbType.Bit);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IDriverKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Driver
            WHERE
                DriverId = @DriverId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DriverId", key.DriverId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public DriverModel GetData(IDriverKey key)
        {
            const string sql = @"
            SELECT
                DriverId, DriverName, IsAktif
            FROM
                BTR_Driver
            WHERE
                DriverId = @DriverId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DriverId", key.DriverId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<DriverModel>(sql, dp);
            }
        }

        public IEnumerable<DriverModel> ListData()
        {
            const string sql = @"
            SELECT
                DriverId, DriverName, IsAktif
            FROM
                BTR_Driver
            WHERE
                IsAktif = 1";

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<DriverModel>(sql);
            }
        }
    }
}