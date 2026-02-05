using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace j07_btrade_sync
{
    public class BrgType
    {
        public BrgType(string brgId, string brgCode, string brgName, string kategoriName,
            string satBesar, string satKecil, int konversi, 
            decimal hrgSat, int stok, string serverId)
        {
            BrgId = brgId;
            BrgCode = brgCode;
            BrgName = brgName;
            KategoriName = kategoriName;
            SatBesar = satBesar;
            SatKecil = satKecil;
            Konversi = konversi;
            HrgSat = hrgSat;
            Stok = stok;
            ServerId = serverId;
        }
        public string BrgId { get; set; }
        public string BrgCode { get; set; }
        public string BrgName { get; set; }
        public string KategoriName { get; set; }
        public string SatBesar { get; set; }
        public string SatKecil { get; set; }
        public int Konversi { get; set; }
        public decimal HrgSat { get; set; }
        public int Stok { get; set; }
        public string ServerId { get; set; }
    }

    public interface IBrgKey
    {
        string BrgId { get; }
    }
}
