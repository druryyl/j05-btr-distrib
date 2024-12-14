using btr.application.SalesContext.DriverFakturRpt;
using btr.domain.InventoryContext.DriverAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SalesContext.DriverFakturRpt
{
    public class DriverFakturInfoDal : IDriverFakturInfoDal
    {
        private readonly DatabaseOptions _opt;

        public DriverFakturInfoDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public IEnumerable<DriverFakturDto> ListData(Periode filter1, IDriverKey filter2)
        {
            const string sql = @"
                SELECT
                    aa.FakturId, aa.FakturCode, aa.CustomerId,
                    ISNULL(bb.DriverId, '') DriverId,
                    ISNULL(bb.DriverName, '') DriverName,
                    ISNULL(cc.CustomerName, '') CustomerName,
                    ISNULL(cc.Address1, '') Address,
                    ISNULL(cc.Kota, '') Kota
                FROM
                    BTR_Faktur aa
                    LEFT JOIN BTR_Driver bb ON aa.DriverId = bb.DriverId
                    LEFT JOIN BTR_Customer cc ON aa.CustomerId = cc.CustomerId
                WHERE
                    aa.FakturDate BETWEEN @Tgl1 AND @Tgl2
                    AND aa.DriverId = @DriverId ";

            var dp = new DynamicParameters();
            dp.AddParam("@Tgl1", filter1.Tgl1, SqlDbType.DateTime);
            dp.AddParam("@Tgl2", filter1.Tgl2, SqlDbType.DateTime);
            dp.AddParam("@DriverId", filter2.DriverId, SqlDbType.VarChar);

            using (var conn = new SqlConnection(ConnStringHelper.Get(_opt)))
            {
                return conn.Read<DriverFakturDto>(sql, dp);
            }
        }
    }
}
