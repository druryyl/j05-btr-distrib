using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.MutasiAgg
{
    public class MutasiDal : IMutasiDal
    {
        private readonly DatabaseOptions _opt;

        public MutasiDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(MutasiModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Mutasi (
                    MutasiId, MutasiDate, JenisMutasi, WarehouseId, 
                    Keterangan, NilaiSediaan,
                    UserId, CreateTime, LastUpdate, VoidDate, UserIdVoid)
                VALUES(
                    @MutasiId, @MutasiDate, @JenisMutasi, @WarehouseId, 
                    @Keterangan, @NilaiSediaan,
                    @UserId, @CreateTime, @LastUpdate, @VoidDate, @UserIdVoid)";

            var dp = new DynamicParameters();
            dp.AddParam("@MutasiId", model.MutasiId, SqlDbType.VarChar); 
            dp.AddParam("@MutasiDate", model.MutasiDate, SqlDbType.DateTime); 
            dp.AddParam("@JenisMutasi", model.JenisMutasi, SqlDbType.Int);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@Keterangan", model.Keterangan, SqlDbType.VarChar);
            dp.AddParam("@NilaiSediaan", model.NilaiSediaan, SqlDbType.Decimal);  

            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar); 
            dp.AddParam("@CreateTime", model.CreateTime, SqlDbType.DateTime); 
            dp.AddParam("@LastUpdate", model.LastUpdate, SqlDbType.DateTime); 
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime); 
            dp.AddParam("@UserIdVoid", model.UserIdVoid, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(MutasiModel model)
        {
            const string sql = @"
                UPDATE  
                    BTR_Mutasi
                SET
                    MutasiDate = @MutasiDate, 
                    JenisMutasi = @JenisMutasi, 
                    WarehouseId = @WarehouseId, 
                    Keterangan = @Keterangan,
                    NilaiSediaan = @NilaiSediaan, 

                    UserId = @UserId,
                    CreateTime = @CreateTime,
                    LastUpdate = @LastUpdate,
                    VoidDate = @VoidDate,
                    UserIdVoid = @UserIdVoid
                WHERE
                    MutasiId = @MutasiId";

            var dp = new DynamicParameters();

            dp.AddParam("@MutasiId", model.MutasiId, SqlDbType.VarChar); 
            dp.AddParam("@MutasiDate", model.MutasiDate, SqlDbType.DateTime);
            dp.AddParam("@JenisMutasi", model.JenisMutasi, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@Keterangan", model.Keterangan, SqlDbType.VarChar);
            dp.AddParam("@NilaiSediaan", model.NilaiSediaan, SqlDbType.Decimal);  

            dp.AddParam("@UserId", model.UserId, SqlDbType.VarChar); 
            dp.AddParam("@CreateTime", model.CreateTime, SqlDbType.DateTime); 
            dp.AddParam("@LastUpdate", model.LastUpdate, SqlDbType.DateTime); 
            dp.AddParam("@VoidDate", model.VoidDate, SqlDbType.DateTime); 
            dp.AddParam("@UserIdVoid", model.UserIdVoid, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IMutasiKey key)
        {
            const string sql = @"
                DELETE FROM  
                    BTR_Mutasi
                WHERE
                    MutasiId = @MutasiId";

            var dp = new DynamicParameters();
            dp.AddParam("@MutasiId", key.MutasiId, SqlDbType.VarChar); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public MutasiModel GetData(IMutasiKey key)
        {
            const string sql = @"
                SELECT
                    aa.MutasiId, aa.MutasiDate, aa.JenisMutasi, 
                    aa.Keterangan, aa.WarehouseId, aa.NilaiSediaan, 
                    aa.CreateTime, aa.LastUpdate, aa.UserId, 
                    aa.VoidDate, aa.UserIdVoid,
                    ISNULL(bb.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_Mutasi aa
                    LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                WHERE
                    MutasiId = @MutasiId ";

            var dp = new DynamicParameters();
            dp.AddParam("@MutasiId", key.MutasiId, SqlDbType.VarChar); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<MutasiModel>(sql, dp);
            }
        }

        public IEnumerable<MutasiModel> ListData(Periode filter)
        {
            const string sql = @"
                SELECT
                    aa.MutasiId, aa.MutasiDate, aa.JenisMutasi, 
                    aa.Keterangan, aa.WarehouseId, aa.NilaiSediaan, 
                    aa.CreateTime, aa.LastUpdate, aa.UserId, 
                    aa.VoidDate, aa.UserIdVoid,
                    ISNULL(bb.WarehouseName, '') AS WarehouseName
                FROM
                    BTR_Mutasi aa
                    LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                WHERE
                    MutasiDate BETWEEN @Tgl1 AND @Tgl2 ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime); 
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime); 

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<MutasiModel>(sql, dp);
            }
        }
    }
}