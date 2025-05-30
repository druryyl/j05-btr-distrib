﻿using btr.application.FinanceContext.FpKeluaragAgg;
using btr.domain.FinanceContext.FpKeluaranAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranDal : IFpKeluaranDal
    {
        private readonly DatabaseOptions _opt;

        public FpKeluaranDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(FpKeluaranModel model)
        {
            const string sql = @"
                INSERT INTO BTR_FpKeluaran
                    (FpKeluaranId, FpKeluaranDate, UserDate,
                    Keterangan, UserId, FakturCount, TotalPpn)
                VALUES
                    (@FpKeluaranId, @FpKeluaranDate, @UserDate,
                    @Keterangan, @UserId, @FakturCount, @TotalPpn)";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", model.FpKeluaranId, SqlDbType.VarChar);
            dp.AddParam("@FpKeluaranDate", model.FpKeluaranDate, SqlDbType.DateTime);
            dp.AddParam("@UserDate", model.UserDate, SqlDbType.DateTime);
            dp.AddParam("@Keterangan", model.Keterangan, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@FakturCount", model.FakturCount, SqlDbType.Int);
            dp.AddParam("@TotalPpn", model.TotalPpn, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(FpKeluaranModel model)
        {
            const string sql = @"
                UPDATE 
                    BTR_FpKeluaran
                SET
                    FpKeluaranDate = @FpKeluaranDate,
                    UserDate = @UserDate,
                    Keterangan = @Keterangan,
                    UserId = @UserId,
                    FakturCount = @FakturCount,
                    TotalPpn = @TotalPpn    
                WHERE
                    FpKeluaranId = @FpKeluaranId";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", model.FpKeluaranId, SqlDbType.VarChar);
            dp.AddParam("@FpKeluaranDate", model.FpKeluaranDate, SqlDbType.DateTime);
            dp.AddParam("@UserDate", model.UserDate, SqlDbType.DateTime);
            dp.AddParam("@Keterangan", model.Keterangan, SqlDbType.VarChar);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);
            dp.AddParam("@FakturCount", model.FakturCount, SqlDbType.Int);
            dp.AddParam("@TotalPpn", model.TotalPpn, SqlDbType.Decimal);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(FpKeluaranModel key)
        {
            const string sql = @"
                DELETE FROM 
                    FpKeluaran
                WHERE
                    FpKeluaranId = @FpKeluaranId";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", key.FpKeluaranId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public FpKeluaranModel GetData(IFpKeluaranKey key)
        {
            const string sql = @"
                SELECT
                    FpKeluaranId, FpKeluaranDate, UserDate,
                    Keterangan, UserId, FakturCount, TotalPpn
                FROM
                    BTR_FpKeluaran
                WHERE
                    FpKeluaranId = @FpKeluaranId";

            var dp = new DynamicParameters();
            dp.AddParam("@FpKeluaranId", key.FpKeluaranId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<FpKeluaranModel>(sql, dp);
            }
        }

        public IEnumerable<FpKeluaranModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    FpKeluaranId, FpKeluaranDate, UserDate, 
                    Keterangan, UserId, FakturCount, TotalPpn
                FROM
                    BTR_FpKeluaran
                WHERE
                    FpKeluaranDate BETWEEN @StartDate AND @EndDate";

            var dp = new DynamicParameters();
            dp.AddParam("@StartDate", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@EndDate", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FpKeluaranModel>(sql, dp);
            }
        }
    }
}
