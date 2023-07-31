﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.InventoryContext.StokAgg.Contracts;
using btr.domain.InventoryContext.StokAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.InventoryContext
{
    public class StokMutasiDal : IStokMutasiDal
    {
        private readonly DatabaseOptions _opt;

        public StokMutasiDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<StokMutasiModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("StokId", "StokId");
                bcp.AddMap("StokMutasiId", "StokMutasiId");
                bcp.AddMap("MutasiId", "MutasiId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("MutasiDate", "MutasiDate");
                bcp.AddMap("QtyIn", "QtyIn");
                bcp.AddMap("QtyOut", "QtyOut");
                bcp.AddMap("HargaJual", "HargaJual");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_StokMutasi";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IStokKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_StokMutasi
            WHERE
                StokId = @StokId ";

            var dp = new DynamicParameters();
            dp.AddParam("@StokId", key.StokId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<StokMutasiModel> ListData(IStokKey stok)
        {
            const string sql = @"
            SELECT
                StokId, StokMutasiId, MutasiId, NoUrut, 
                MutasiDate, QtyIn, QtyOut, HargaJual
            FROM 
                BTR_StokMutasi aa
            WHERE
                aa.StokId = @StokId ";
            var dp = new DynamicParameters();
            dp.AddParam("@StokId", stok.StokId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<StokMutasiModel>(sql, dp);
            }
        }
    }
}