using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.PackingAgg;
using btr.domain.InventoryContext.PackingAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.PackingAgg
{
    public class PackingDal : IPackingDal
    {
        private readonly DatabaseOptions _opt;

        public PackingDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(PackingModel model)
        {
            const string sql = @"
            INSERT INTO BTR_Packing(
                PackingId, PackingDate, WarehouseId, 
                DriverId, DeliveryDate, Route)
            VALUES (
                @PackingId, @PackingDate, @WarehouseId, 
                @DriverId, @DeliveryDate, @Route)";

            var @dp = new DynamicParameters();
            dp.AddParam("@PackingId", model.PackingId, SqlDbType.VarChar);
            dp.AddParam("@PackingDate", model.PackingDate, SqlDbType.DateTime);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@DriverId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@DeliveryDate", model.DeliveryDate, SqlDbType.DateTime);
            dp.AddParam("@TglAwalFaktur", model.TglAwalFaktur, SqlDbType.DateTime);
            dp.AddParam("@TglAkhirFaktur", model.TglAkhirFaktur, SqlDbType.DateTime);
            dp.AddParam("@KeywordSearch", model.KeywordSearch, SqlDbType.DateTime);
            

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(PackingModel model)
        {
            const string sql = @"
            UPDATE 
                BTR_Packing
            SET
                PackingDate = @PackingDate,
                WarehouseId = @WarehouseId,
                DriverId = @DriverId,
                DeliveryDate = @DeliveryDate,
                TglAKhirFaktur = @TglAkhirFaktur,
                TglAwalFaktur = @TglAwalFaktur,
                KeywordSearch = @KeywordSearch
            WHERE
                PackingId = @PackingId ";

            var @dp = new DynamicParameters();
            dp.AddParam("@PackingId", model.PackingId, SqlDbType.VarChar);
            dp.AddParam("@PackingDate", model.PackingDate, SqlDbType.DateTime);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);

            dp.AddParam("@DriverId", model.DriverId, SqlDbType.VarChar);
            dp.AddParam("@DeliveryDate", model.DeliveryDate, SqlDbType.DateTime);

            dp.AddParam("@TglAwalFaktur", model.TglAwalFaktur, SqlDbType.DateTime);
            dp.AddParam("@TglAkhirFaktur", model.TglAkhirFaktur, SqlDbType.DateTime);
            dp.AddParam("@KeywordSearch", model.KeywordSearch, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IPackingKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_Packing
            WHERE
                PackingId = @PackingId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", key.PackingId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public PackingModel GetData(IPackingKey key)
        {
            const string sql = @"
            SELECT
                aa.PackingId, aa.PackingDate, aa.WarehouseId,
                aa.DriverId, aa.DeliveryDate, 
                aa.TglAwalFaktur, aa.TglAKhirFaktur, aa.KeywordSearch,
                ISNULL(bb.WarehouseName, '') AS WarehouseName,
                ISNULL(cc.DriverName, '') AS DriverName
            FROM
                BTR_Packing aa
                LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                LEFT JOIN BTR_Driver Cc ON aa.DriverId = cc.DriverId
            WHERE
                PackingId = @PackingId ";

            var dp = new DynamicParameters();
            dp.AddParam("@PackingId", key.PackingId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<PackingModel>(sql, dp);
            }
        }

        public IEnumerable<PackingModel> ListData(Periode periode)
        {
            const string sql = @"
            SELECT
                aa.PackingId, aa.PackingDate, aa.WarehouseId,
                aa.DriverId, aa.DeliveryDate, 
                aa.TglAwalFaktur, aa.TglAKhirFaktur, aa.KeywordSearch,
                ISNULL(bb.WarehouseName, '') AS WarehouseName,
                ISNULL(cc.DriverName, '') AS DriverName
            FROM
                BTR_Packing aa
                LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                LEFT JOIN BTR_Driver Cc ON aa.DriverId = cc.DriverId
            WHERE
                aa.DeliveryDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", periode.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<PackingModel>(sql, dp);
            }
        }
    }
}