using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using btr.application.SupportContext.PrintManagerAgg;
using btr.domain.SupportContext.PrintManagerAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext.DocAgg
{
    public class DocDal : IDocDal
    {
        private readonly DatabaseOptions _opt;

        public DocDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(DocModel model)
        {
            const string sql = @"
                INSERT INTO BTR_Doc(
                    DocId, DocType, DocDate, DocDesc,
                    WarehouseId, DocPrintStatus)
                VALUES (
                    @DocId, @DocType, @DocDate, @DocDesc,
                    @WarehouseId, @DocPrintStatus)";

            var dp = new DynamicParameters();
            dp.AddParam("@DocId", model.DocId, SqlDbType.VarChar);
            dp.AddParam("@DocType", model.DocType, SqlDbType.VarChar);
            dp.AddParam("@DocDate", model.DocDate, SqlDbType.DateTime);
            dp.AddParam("@DocDesc", model.DocDesc, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@DocPrintStatus", model.DocPrintStatus, SqlDbType.Int);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Update(DocModel model)
        {
            const string sql = @"
                UPDATE 
                    BTR_Doc
                SET
                    DocType = @DocType,
                    DocDate = @DocDate,
                    DocDesc = @DocDesc,
                    WarehouseId = @WarehouseId,
                    DocPrintStatus = @DocPrintStatus
                WHERE
                    DocId = @DocId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DocId", model.DocId, SqlDbType.VarChar);
            dp.AddParam("@DocType", model.DocType, SqlDbType.VarChar);
            dp.AddParam("@DocDate", model.DocDate, SqlDbType.DateTime);
            dp.AddParam("@DocDesc", model.DocDesc, SqlDbType.VarChar);
            dp.AddParam("@WarehouseId", model.WarehouseId, SqlDbType.VarChar);
            dp.AddParam("@DocPrintStatus", model.DocPrintStatus, SqlDbType.Int);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public void Delete(IDocKey key)
        {
            const string sql = @"
                DELETE FROM 
                    BTR_Doc
                WHERE
                    DocId = @DocId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DocId", key.DocId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public DocModel GetData(IDocKey key)
        {
            const string sql = @"
                SELECT
                    aa.DocId, aa.DocType, aa.DocDate, aa.DocDesc,
                    aa.WarehouseId, aa.DocPrintStatus,
                    ISNULL(bb.WarehouseName, '') AS WarehouseName
                FROM                
                    BTR_Doc aa      
                    LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                WHERE
                    aa.DocId = @DocId ";

            var dp = new DynamicParameters();
            dp.AddParam("@DocId", key.DocId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.ReadSingle<DocModel>(sql, dp);
            }
        }

        public IEnumerable<DocModel> ListData(Periode periode)
        {
            const string sql = @"
                SELECT
                    aa.DocId, aa.DocType, aa.DocDate, aa.DocDesc,
                    aa.WarehouseId, aa.DocPrintStatus,
                    ISNULL(bb.WarehouseName, '') AS WarehouseName
                FROM                
                    BTR_Doc aa      
                    LEFT JOIN BTR_Warehouse bb ON aa.WarehouseId = bb.WarehouseId
                WHERE
                    aa.DocDate BETWEEN @Tgl1 AND @Tgl2";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", periode.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", periode.Tgl2, SqlDbType.DateTime);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<DocModel>(sql, dp);
            }
        }
    }
}