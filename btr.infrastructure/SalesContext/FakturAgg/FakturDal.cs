﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext
{
    public class FakturDal : IFakturDal
    {
        private readonly DatabaseOptions _opt;

        public FakturDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(FakturModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Faktur(
                FakturId, FakturDate, SalesPersonId, CustomerId,
                WarehouseId, TglRencanaKirim, TermOfPayment, Total,
                DiscountLain, BiayaLain, GrandTotal, UangMuka, KurangBayar, 
                CreateTime, LastUpdate, UserId)
            VALUES(
                @FakturId, @FakturDate, @SalesPersonId, @CustomerId,
                @WarehouseId, @TglRencanaKirim, @TermOfPayment, @Total,
                @DiscountLain, @BiayaLain, @GrandTotal, @UangMuka, @KurangBayar, 
                @CreateTime, @LastUpdate, @UserId) ";

            var dp = new DynamicParameters();

            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", model.FakturDate, SqlDbType.DateTime);

            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@TglRencanaKirim", model.TglRencanaKirim, SqlDbType.DateTime);
            dp.AddParam("@TermOfPayment", model.TermOfPayment, SqlDbType.VarChar);

            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@DiscountLain", model.DiscountLain, SqlDbType.Decimal);
            dp.AddParam("@BiayaLain", model.BiayaLain, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            dp.AddParam("@UangMuka", model.UangMuka, SqlDbType.Decimal);
            dp.AddParam("@KurangBayar", model.KurangBayar, SqlDbType.Decimal);

            dp.AddParam("@CreateTime", model.CreateTime, SqlDbType.DateTime);
            dp.AddParam("@LastUpdate", model.LastUpdate, SqlDbType.DateTime);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(FakturModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Faktur
            SET
                FakturDate = @FakturDate, 
                SalesPersonId = @SalesPersonId, 
                CustomerId = @CustomerId,
                WarehouseId = @WarehouseId, 
                TglRencanaKirim = @TglRencanaKirim, 
                TermOfPayment = @TermOfPayment, 
                Total = @Total,
                DiscountLain = @DiscountLain, 
                BiayaLain = @BiayaLain, 
                GrandTotal = @GrandTotal, 
                UangMuka = @UangMuka, 
                KurangBayar = @KurangBayar, 
                CreateTime = @CreateTime, 
                LastUpdate = @LastUpdate, 
                UserId = @UserId
            WHERE
                FakturId = @FakturId ";

            var dp = new DynamicParameters();

            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", model.FakturDate, SqlDbType.DateTime);

            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@TglRencanaKirim", model.TglRencanaKirim, SqlDbType.DateTime);
            dp.AddParam("@TermOfPayment", model.TermOfPayment, SqlDbType.VarChar);

            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@DiscountLain", model.DiscountLain, SqlDbType.Decimal);
            dp.AddParam("@BiayaLain", model.BiayaLain, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            dp.AddParam("@UangMuka", model.UangMuka, SqlDbType.Decimal);
            dp.AddParam("@KurangBayar", model.KurangBayar, SqlDbType.Decimal);

            dp.AddParam("@CreateTime", model.CreateTime, SqlDbType.DateTime);
            dp.AddParam("@LastUpdate", model.LastUpdate, SqlDbType.DateTime);
            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IFakturKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Faktur
            WHERE
                FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public FakturModel GetData(IFakturKey key)
        {
            const string sql = @"
            SELECT
                aa.FakturId, aa.FakturDate, aa.SalesPersonId, aa.CustomerId,
                aa.WarehouseId, aa.TglRencanaKirim, aa.TermOfPayment, aa.Total,
                aa.DiscountLain, aa.BiayaLain, aa.GrandTotal, aa.UangMuka, aa.KurangBayar, 
                aa.CreateTime, aa.LastUpdate, aa.UserId,
                ISNULL(bb.SalesPersonName, '') AS SalesPersonName,
                ISNULL(cc.CustomerName, '') AS CustomerName,
                ISNULL(cc.Plafond, 0) AS Plafond,
                ISNULL(cc.CreditBalance, 0) AS CreditBalance,
                ISNULL(dd.WarehouseName, '') AS WarehouseName
            FROM 
                BTR_Faktur aa
                LEFT JOIN BTR_SalesPerson bb ON aa.SalesPersonId = bb.SalesPersonId
                LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId
                LEFT JOIN BTR_Warehouse dd on aa.WarehouseId = dd.WarehouseId
            WHERE
                aa.FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<FakturModel>(sql, dp);
            }
        }

        public IEnumerable<FakturModel> ListData(Periode filter)
        {
            const string sql = @"
            SELECT
                aa.FakturId, aa.FakturDate, aa.SalesPersonId, aa.CustomerId,
                aa.WarehouseId, aa.TglRencanaKirim, aa.TermOfPayment, aa.Total,
                aa.DiscountLain, aa.BiayaLain, aa.GrandTotal, aa.UangMuka, aa.KurangBayar, 
                aa.CreateTime, aa.LastUpdate, aa.UserId,
                ISNULL(bb.SalesPersonName, '') AS SalesPersonName,
                ISNULL(cc.CustomerName, '') AS CustomerName,
                ISNULL(cc.Plafond, 0) AS Plafond,
                ISNULL(cc.CreditBalance, 0) AS CreditBalance,
                ISNULL(dd.WarehouseName, '') AS WarehouseName
            FROM 
                BTR_Faktur aa
                LEFT JOIN BTR_SalesPerson bb ON aa.SalesPersonId = bb.SalesPersonId
                LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId
                LEFT JOIN BTR_Warehouse dd on aa.WarehouseId = dd.WarehouseId
            WHERE
                aa.FakturDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturModel>(sql, dp);
            }
        }
    }
}