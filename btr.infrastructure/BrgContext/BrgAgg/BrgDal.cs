﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.BrgContext.BrgAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.KategoriAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.BrgContext.BrgAgg
{
    public class BrgDal : IBrgDal
    {
        private readonly DatabaseOptions _opt;

        public BrgDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(BrgModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Brg(
                    BrgId, BrgName, BrgCode, IsAktif, 
                    SupplierId, KategoriId, Hpp, HppTimestamp)
                VALUES (
                    @BrgId, @BrgName, @BrgCode, @IsAktif, 
                    @SupplierId, @KategoriId, @Hpp, @HppTimestamp)";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@BrgName", model.BrgName, SqlDbType.VarChar);
            dp.AddParam("@BrgCode", model.BrgCode, SqlDbType.VarChar);
            dp.AddParam("@IsAktif", model.IsAktif, SqlDbType.Bit);
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);  
            dp.AddParam("@KategoriId", model.KategoriId, SqlDbType.VarChar);  
            dp.AddParam("@Hpp", model.Hpp, SqlDbType.Decimal);  
            dp.AddParam("@HppTimestamp", model.HppTimestamp, SqlDbType.DateTime); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(BrgModel model)
        {
            const string sql = @"
                UPDATE 
                    BTR_Brg
                SET
                    BrgName = @BrgName,
                    BrgCode = @BrgCode, 
                    IsAktif = @IsAktif, 
                    SupplierId = @SupplierId, 
                    KategoriId = @KategoriId, 
                    Hpp = @Hpp, 
                    HppTimestamp = @HppTimestamp
                WHERE
                    BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", model.BrgId, SqlDbType.VarChar);
            dp.AddParam("@BrgName", model.BrgName, SqlDbType.VarChar);
            dp.AddParam("@BrgCode", model.BrgCode, SqlDbType.VarChar);
            dp.AddParam("@IsAktif", model.IsAktif, SqlDbType.Bit);
            dp.AddParam("@SupplierId", model.SupplierId, SqlDbType.VarChar);
            dp.AddParam("@KategoriId", model.KategoriId, SqlDbType.VarChar);
            dp.AddParam("@Hpp", model.Hpp, SqlDbType.Decimal);
            dp.AddParam("@HppTimestamp", model.HppTimestamp, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IBrgKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_Brg
                WHERE
                    BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", key.BrgId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public BrgModel GetData(IBrgKey key)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.BrgName, aa.BrgCode, aa.IsAktif, 
                    aa.SupplierId, aa.KategoriId, aa.Hpp, aa.HppTimestamp,
                    ISNULL(bb.SupplierName, '') SupplierName,
                    ISNULL(cc.JenisBrgName, '') JenisBrgName,
                    ISNULL(dd.KategoriName, '') KategoriName
                FROM
                    BTR_Brg aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_JenisBrg cc ON aa.JenisBrgId = cc.JenisBrgId
                    LEFT JOIN BTR_Kategori dd ON aa.KategoriId = dd.KategoriId
                WHERE
                    BrgId = @BrgId ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgId", key.BrgId, SqlDbType.VarChar);

            BrgModel result;
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                result = conn.ReadSingle<BrgModel>(sql, dp);
            }

            return result;
        }

        public IEnumerable<BrgModel> ListData()
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.BrgName, aa.BrgCode, aa.IsAktif, 
                    aa.SupplierId, aa.KategoriId, aa.Hpp, aa.HppTimestamp,
                    ISNULL(bb.SupplierName, '') SupplierName,
                    ISNULL(cc.JenisBrgName, '') JenisBrgName,
                    ISNULL(dd.KategoriName, '') KategoriName
                FROM
                    BTR_Brg aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_JenisBrg cc ON aa.JenisBrgId = cc.JenisBrgId
                    LEFT JOIN BTR_Kategori dd ON aa.KategoriId = dd.KategoriId ";

            IEnumerable<BrgModel> result;
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                result = conn.Read<BrgModel>(sql);
            }

            return result;
        }

        public BrgModel GetData(string key)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.BrgName, aa.BrgCode, aa.IsAktif, 
                    aa.SupplierId, aa.KategoriId, aa.Hpp, aa.HppTimestamp,
                    ISNULL(bb.SupplierName, '') AS SupplierName,
                    ISNULL(cc.JenisBrgName, '') AS JenisBrgName,
                    ISNULL(dd.KategoriName, '') AS KategoriName
                FROM
                    BTR_Brg aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_JenisBrg cc ON aa.JenisBrgId = cc.JenisBrgId
                    LEFT JOIN BTR_Kategori dd ON aa.KategoriId = dd.KategoriId
                WHERE
                    BrgCode = @BrgCode ";

            var dp = new DynamicParameters();
            dp.AddParam("@BrgCode", key, SqlDbType.VarChar);

            BrgModel result;
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                result = conn.ReadSingle<BrgModel>(sql, dp);
            }
            return result;
        }

        public IEnumerable<BrgModel> ListData(IKategoriKey filter)
        {
            const string sql = @"
                SELECT
                    aa.BrgId, aa.BrgName, aa.BrgCode, aa.IsAktif, 
                    aa.SupplierId, aa.KategoriId, aa.Hpp, aa.HppTimestamp,
                    ISNULL(bb.SupplierName, '') AS SupplierName,
                    ISNULL(cc.JenisBrgName, '') AS JenisBrgName,
                    ISNULL(dd.KategoriName, '') AS KategoriName
                FROM
                    BTR_Brg aa
                    LEFT JOIN BTR_Supplier bb ON aa.SupplierId = bb.SupplierId
                    LEFT JOIN BTR_JenisBrg cc ON aa.JenisBrgId = cc.JenisBrgId
                    LEFT JOIN BTR_Kategori dd ON aa.KategoriId = dd.KategoriId
                WHERE
                    aa.KategoriId = @KategoriId ";

            var dp = new DynamicParameters();
            dp.AddParam("@KategoriId", filter.KategoriId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<BrgModel>(sql, dp);
            }
        }
    }
}