using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext.FakturAgg
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
                FakturId, FakturDate, FakturCode, SalesPersonId, CustomerId, HargaTypeId,
                WarehouseId, TglRencanaKirim, TermOfPayment, DueDate, Total,
                Discount, Tax, GrandTotal, UangMuka, KurangBayar, NoFakturPajak,
                CreateTime, LastUpdate, UserId)
            VALUES(
                @FakturId, @FakturDate, @FakturCode, @SalesPersonId, @CustomerId, @HargaTypeId,
                @WarehouseId, @TglRencanaKirim, @TermOfPayment, @DueDate, @Total,
                @Discount, @Tax, @GrandTotal, @UangMuka, @KurangBayar, @NoFakturPajak,
                @CreateTime, @LastUpdate, @UserId) ";

            var dp = new DynamicParameters();

            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", model.FakturDate, SqlDbType.DateTime);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@HargaTypeId", model.HargaTypeId, SqlDbType.VarChar);

            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@TglRencanaKirim", model.TglRencanaKirim, SqlDbType.DateTime);
            dp.AddParam("@TermOfPayment", model.TermOfPayment, SqlDbType.Int);
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime);

            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@Discount", model.Discount, SqlDbType.Decimal);
            dp.AddParam("@Tax", model.Tax, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            dp.AddParam("@UangMuka", model.UangMuka, SqlDbType.Decimal);
            dp.AddParam("@KurangBayar", model.KurangBayar, SqlDbType.Decimal);
            dp.AddParam("@NoFakturPajak", model.NoFakturPajak, SqlDbType.VarChar);

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
                FakturCode = @FakturCode,
                FakturDate = @FakturDate, 
                SalesPersonId = @SalesPersonId, 
                CustomerId = @CustomerId,
                HargaTypeId = @HargaTypeId,
                WarehouseId = @WarehouseId, 
                TglRencanaKirim = @TglRencanaKirim, 
                TermOfPayment = @TermOfPayment, 
                DueDate = @DueDate,
                Total = @Total,
                Discount = @Discount, 
                Tax = @Tax,
                GrandTotal = @GrandTotal, 
                UangMuka = @UangMuka, 
                KurangBayar = @KurangBayar, 
                NoFakturPajak = @NoFakturPajak, 
                CreateTime = @CreateTime, 
                LastUpdate = @LastUpdate, 
                UserId = @UserId
            WHERE
                FakturId = @FakturId ";

            var dp = new DynamicParameters();

            dp.AddParam("@FakturId", model.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", model.FakturDate, SqlDbType.DateTime);
            dp.AddParam("@FakturCode", model.FakturCode, SqlDbType.VarChar);

            dp.AddParam("@SalesPersonId", model.SalesPersonId, SqlDbType.VarChar);
            dp.AddParam("@CustomerId", model.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@HargaTypeId", model.HargaTypeId, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@TglRencanaKirim", model.TglRencanaKirim, SqlDbType.DateTime);
            dp.AddParam("@TermOfPayment", model.TermOfPayment, SqlDbType.Int);
            dp.AddParam("@DueDate", model.DueDate, SqlDbType.DateTime);

            dp.AddParam("@Total", model.Total, SqlDbType.Decimal);
            dp.AddParam("@Discount", model.Discount, SqlDbType.Decimal);
            dp.AddParam("@Tax", model.Tax, SqlDbType.Decimal);
            dp.AddParam("@GrandTotal", model.GrandTotal, SqlDbType.Decimal);
            dp.AddParam("@UangMuka", model.UangMuka, SqlDbType.Decimal);
            dp.AddParam("@KurangBayar", model.KurangBayar, SqlDbType.Decimal);
            dp.AddParam("@NoFakturPajak", model.NoFakturPajak, SqlDbType.VarChar);

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
                aa.FakturId, aa.FakturDate, aa.FakturCode, aa.SalesPersonId, aa.CustomerId, aa.HargaTypeId,
                aa.WarehouseId, aa.TglRencanaKirim, aa.TermOfPayment, aa.DueDate, aa.Total,
                aa.Discount, aa.Tax, aa.GrandTotal, aa.UangMuka, aa.KurangBayar, aa.NoFakturPajak,
                aa.CreateTime, aa.LastUpdate, aa.UserId,
                ISNULL(bb.SalesPersonName, '') AS SalesPersonName,
                ISNULL(cc.CustomerName, '') AS CustomerName,
                ISNULL(cc.CustomerCode, '') AS CustomerCode,
                ISNULL(cc.Npwp, '') AS Npwp,
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
                aa.FakturId, aa.FakturDate, aa.FakturCode, aa.SalesPersonId, aa.CustomerId, aa.HargaTypeId,
                aa.WarehouseId, aa.TglRencanaKirim, aa.TermOfPayment, aa.DueDate, aa.Total,
                aa.Discount, aa.Tax, aa.GrandTotal, aa.UangMuka, aa.KurangBayar, aa.NoFakturPajak,
                aa.CreateTime, aa.LastUpdate, aa.UserId,
                ISNULL(bb.SalesPersonName, '') AS SalesPersonName,
                ISNULL(cc.CustomerName, '') AS CustomerName,
                ISNULL(cc.CustomerCode, '') AS CustomerCode,
                ISNULL(cc.Npwp, '') AS Npwp,
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