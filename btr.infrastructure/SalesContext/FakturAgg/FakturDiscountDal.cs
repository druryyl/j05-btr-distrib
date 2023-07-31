﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.SalesContext
{
    public class FakturDiscountDal : IFakturDiscountDal
    {
        private readonly DatabaseOptions _opt;

        public FakturDiscountDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<FakturDiscountModel> listModel)
        {
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("FakturItemId", "FakturItemId");
                bcp.AddMap("FakturDiscountId", "FakturDiscountId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("BrgId", "BrgId");
                bcp.AddMap("DiscountProsen", "DiscountProsen");
                bcp.AddMap("DiscountRp", "DiscountRp");

                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "dbo.BTR_FakturDiscount";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(IFakturKey key)
        {
            const string sql = @"
            DELETE FROM 
                BTR_FakturDiscount
            WHERE
                FakturId = @FakturId ";

            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", key.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<FakturDiscountModel> ListData(IFakturKey fakturKey)
        {
            const string sql = @"
            SELECT
                aa.FakturId, aa.FakturItemId, aa.FakturDiscountId,
                aa.NoUrut, aa.BrgId, 
                aa.DiscountProsen, aa.DiscountRp
            FROM 
                BTR_FakturDiscount aa
            WHERE
                aa.FakturId = @FakturId ";
            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", fakturKey.FakturId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<FakturDiscountModel>(sql, dp);
            }
        }
    }
}