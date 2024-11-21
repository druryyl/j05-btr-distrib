using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.FinanceContext.TagihanAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;

namespace btr.infrastructure.FinanceContext.TagihanAgg
{
    public class TagihanFakturDal : ITagihanFakturDal
    {
        private readonly DatabaseOptions _opt;

        public TagihanFakturDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(IEnumerable<TagihanFakturModel> listModel)
        {
            //  insert bulk data to table BTR_TagihanFaktur
            using(var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            using (var bcp = new SqlBulkCopy(conn))
            {
                conn.Open();
                bcp.AddMap("TagihanId", "TagihanId");
                bcp.AddMap("NoUrut", "NoUrut");
                bcp.AddMap("FakturId", "FakturId");
                bcp.AddMap("CustomerId", "CustomerId");
                bcp.AddMap("Nilai", "Nilai");
                
                var fetched = listModel.ToList();
                bcp.BatchSize = fetched.Count;
                bcp.DestinationTableName = "BTR_TagihanFaktur";
                bcp.WriteToServer(fetched.AsDataTable());
            }
        }

        public void Delete(ITagihanKey key)
        {
            //  query delete table BTR_TagihanFaktur by TagihanId
            const string sql = @"
                DELETE FROM BTR_TagihanFaktur
                WHERE TagihanId = @TagihanId";
            
            //  parameter
            var dp = new DynamicParameters();
            dp.AddParam("@TagihanId", key.TagihanId, SqlDbType.VarChar);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                conn.Execute(sql, dp);
            }
        }

        public IEnumerable<TagihanFakturModel> ListData(ITagihanKey filter)
        {
            const string sql = @"
                SELECT
                    aa.TagihanId, aa.NoUrut, aa.FakturId,
                    bb.FakturCode, aa.CustomerId, cc.CustomerName,
                    cc.Address1, aa.Nilai
                FROM
                    BTR_TagihanFaktur aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                WHERE
                    TagihanId = @TagihanId
                ORDER BY
                    NoUrut";
            
            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@TagihanId", filter.TagihanId, SqlDbType.VarChar);
            
            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<TagihanFakturModel>(sql, dp);
            }
        }

        public IEnumerable<TagihanFakturViewDto> ListData(IFakturKey filter)
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.TagihanId, 
                    ISNULL(bb.TagihanDate, GetDate()) TagihanDate,
                    ISNULL(cc.SalesPersonName, '') SalesPersonName
                FROM
                    BTR_TagihanFaktur aa
                    LEFT JOIN BTR_Tagihan bb ON aa.TagihanId = bb.TagihanId
                    LEFT JOIN BTR_SalesPerson cc ON bb.SalesPersonId = cc.SalesPersonId
                WHERE
                    aa.FakturId = @FakturId 
                ORDER BY
                    aa.TagihanId";

            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@FakturId", filter.FakturId, SqlDbType.VarChar);

            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<TagihanFakturViewDto>(sql, dp);
            }
        }
    }
}