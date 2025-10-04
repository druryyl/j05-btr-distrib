using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using btr.application.FinanceContext.TagihanAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
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
                bcp.AddMap("NilaiTotal", "NilaiTotal");
                bcp.AddMap("NilaiTerbayar", "NilaiTerbayar");
                bcp.AddMap("NilaiTagih", "NilaiTagih");
                bcp.AddMap("IsTandaTerima", "IsTandaTerima");
                bcp.AddMap("Keterangan", "Keterangan");
                bcp.AddMap("TandaTerimaDate", "TandaTerimaDate");
                bcp.AddMap("IsTagihUlang", "IsTagihUlang");

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
                    aa.CustomerId, aa.NilaiTotal, aa.NilaiTerbayar, aa.NilaiTagih,
                    aa.IsTandaTerima, aa.Keterangan, aa.TandaTerimaDate, 
                    aa.IsTagihUlang,
                    ISNULL(bb.FakturCode, '') AS FakturCode,
                    ISNULL(bb.FakturDate, '3000-01-01') AS FakturDate,
                    ISNULL(cc.CustomerName, '') CustomerName, 
                    ISNULL(cc.Address1, '') Alamat
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
                    ISNULL(cc.SalesPersonName, '') SalesPersonName,
                    ISNULL(dd.Address1, '') Alamat
                FROM
                    BTR_TagihanFaktur aa
                    LEFT JOIN BTR_Tagihan bb ON aa.TagihanId = bb.TagihanId
                    LEFT JOIN BTR_SalesPerson cc ON bb.SalesPersonId = cc.SalesPersonId
                    LEFT JOIN BTR_Customer dd ON aa.CustomerId = dd.CustomerId
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

        public IEnumerable<TandaTerimaTagihanViewDto> ListData(Periode filter)
        {
            const string sql = @"
                SELECT  
                    aa.TagihanId, aa.FakturId, 
                    aa.CustomerId, aa.NilaiTotal, aa.NilaiTerbayar, aa.NilaiTagih,
                    aa.IsTandaTerima, aa.Keterangan, aa.TandaTerimaDate, 
                    aa.IsTagihUlang,
                    ISNULL(bb.FakturCode, '') AS FakturCode,
                    ISNULL(bb.FakturDate, '3000-01-01') AS FakturDate,
                    ISNULL(bb.SalesPersonId, '') AS SalesPersonId,      
                    ISNULL(cc.CustomerName, '') CustomerName, 
                    ISNULL(cc.Address1, '') Alamat,
                    ISNULL(dd.SalesPersonName, '') AS SalesPersonName
                FROM
                    BTR_TagihanFaktur aa
                    LEFT JOIN BTR_Faktur bb ON aa.FakturId = bb.FakturId
                    LEFT JOIN BTR_Customer cc ON bb.CustomerId = cc.CustomerId
                    LEFT JOIN BTR_SalesPerson dd ON bb.SalesPersonId = dd.SalesPersonId
                WHERE
                    bb.FakturDate BETWEEN @Tgl1 AND @Tgl2
                ORDER BY
                    NoUrut";

            // parameter
            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter.Tgl2, SqlDbType.DateTime);


            //  execute query
            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<TandaTerimaTagihanViewDto>(sql, dp);
            }
        }
    }
}