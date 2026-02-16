using btr.domain.InventoryContext.PackingOrderFeature;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace btr.infrastructure.InventoryContext.PackingOrderFeature
{
    public interface IPackingOrderDal :
        IInsert<PackingOrderDto>,
        IUpdate<PackingOrderDto>,
        IDelete<IPackingOrderKey>,
        IGetData<PackingOrderDto, IPackingOrderKey>,
        IGetData<PackingOrderDto, IFakturKey>,
        IListData<PackingOrderDto, Periode>
    {
    }

    public class PackingOrderDal : IPackingOrderDal
    {
        private readonly DatabaseOptions _opt;

        public PackingOrderDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(PackingOrderDto dto)
        {
            const string sql = @"
                INSERT INTO BTR_PackingOrder(
                    PackingOrderId, PackingOrderDate,
                    CustomerId, CustomerCode, CustomerName, Alamat, NoTelp,
                    Latitude, Longitude, Accuracy,
                    FakturId, FakturCode, FakturDate, AdminName, UploadTimestamp)
                VALUES(
                    @PackingOrderId, @PackingOrderDate,
                    @CustomerId, @CustomerCode, @CustomerName, @Alamat, @NoTelp,
                    @Latitude, @Longitude, @Accuracy,
                    @FakturId, @FakturCode, @FakturDate, @AdminName, @UploadTimestamp)
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", dto.PackingOrderId, SqlDbType.VarChar);
            dp.AddParam("@PackingOrderDate", dto.PackingOrderDate, SqlDbType.DateTime);

            dp.AddParam("@CustomerId", dto.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", dto.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", dto.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@Alamat", dto.Alamat, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", dto.NoTelp, SqlDbType.VarChar);

            dp.AddParam("@Latitude", dto.Latitude, SqlDbType.Float);
            dp.AddParam("@Longitude", dto.Longitude, SqlDbType.Float);
            dp.AddParam("@Accuracy", dto.Accuracy, SqlDbType.Float);

            dp.AddParam("@FakturId", dto.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", dto.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", dto.FakturDate, SqlDbType.DateTime);
            dp.AddParam("@AdminName", dto.AdminName, SqlDbType.VarChar);
            dp.AddParam("@UploadTimestamp", new DateTime(3000,1,1), SqlDbType.DateTime);

            var connStr = ConnStringHelper.Get(_opt);
            using (var conn = new SqlConnection(connStr))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(PackingOrderDto dto)
        {
            const string sql = @"
                UPDATE BTR_PackingOrder
                SET
                    PackingOrderDate = @PackingOrderDate,
                    
                    CustomerId = @CustomerId,
                    CustomerCode = @CustomerCode,
                    CustomerName = @CustomerName,
                    Alamat = @Alamat,
                    NoTelp = @NoTelp,
                    Latitude = @Latitude,
                    Longitude = @Longitude,
                    Accuracy = @Accuracy,

                    FakturId = @FakturId,
                    FakturCode = @FakturCode,
                    FakturDate = @FakturDate,
                    AdminName = @AdminName,
                    UploadTimestamp = @UploadTimestamp
                WHERE
                    PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", dto.PackingOrderId, SqlDbType.VarChar);
            dp.AddParam("@PackingOrderDate", dto.PackingOrderDate, SqlDbType.DateTime);

            dp.AddParam("@CustomerId", dto.CustomerId, SqlDbType.VarChar);
            dp.AddParam("@CustomerCode", dto.CustomerCode, SqlDbType.VarChar);
            dp.AddParam("@CustomerName", dto.CustomerName, SqlDbType.VarChar);
            dp.AddParam("@Alamat", dto.Alamat, SqlDbType.VarChar);
            dp.AddParam("@NoTelp", dto.NoTelp, SqlDbType.VarChar);

            dp.AddParam("@Latitude", dto.Latitude, SqlDbType.Float);
            dp.AddParam("@Longitude", dto.Longitude, SqlDbType.Float);
            dp.AddParam("@Accuracy", dto.Accuracy, SqlDbType.Float);

            dp.AddParam("@FakturId", dto.FakturId, SqlDbType.VarChar);
            dp.AddParam("@FakturCode", dto.FakturCode, SqlDbType.VarChar);
            dp.AddParam("@FakturDate", dto.FakturDate, SqlDbType.DateTime);
            dp.AddParam("@AdminName", dto.AdminName, SqlDbType.VarChar);
            dp.AddParam("@UploadTimestamp", new DateTime(3000, 1, 1), SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IPackingOrderKey key)
        {
            const string sql = @"
                DELETE FROM BTR_PackingOrder
                WHERE PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", key.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public PackingOrderDto GetData(IPackingOrderKey key)
        {
            const string sql = @"
                SELECT
                    PackingOrderId, PackingOrderDate, 
                    CustomerId, CustomerCode, CustomerName, Alamat, NoTelp,
                    Latitude, Longitude, Accuracy,
                    FakturId, FakturCode, FakturDate, AdminName
                FROM BTR_PackingOrder
                WHERE PackingOrderId = @PackingOrderId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingOrderId", key.PackingOrderId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<PackingOrderDto>(sql, dp);
            }
        }

        public IEnumerable<PackingOrderDto> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    PackingOrderId, PackingOrderDate, 
                    CustomerId, CustomerCode, CustomerName, Alamat, NoTelp,
                    Latitude, Longitude, Accuracy,
                    FakturId, FakturCode, FakturDate, AdminName
                FROM BTR_PackingOrder
                WHERE PackingOrderDate BETWEEN @Tgl1 AND @Tgl2
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PackingOrderDto>(sql, dp);
            }
        }

        public PackingOrderDto GetData(IFakturKey key)
        {
            const string sql = @"
                SELECT
                    PackingOrderId, PackingOrderDate,
                    CustomerId, CustomerCode, CustomerName, Alamat, NoTelp,
                    Latitude, Longitude, Accuracy,
                    FakturId, FakturCode, FakturDate, AdminName
                FROM BTR_PackingOrder
                WHERE FakturId = @FakturId
                ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<PackingOrderDto>(sql, dp);
            }
        }
    }
}