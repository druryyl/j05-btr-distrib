using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.domain.FinanceContext.FpKeluaranAgg
{
    public class FpKeluaranModel : IFpKeluaranKey
    {
        public FpKeluaranModel()
        {
            ListFaktur = new List<FpKeluaranFakturModel>();
        }
        public FpKeluaranModel(string id)
        {
            FpKeluaranId = id;
        }
        public string FpKeluaranId { get; set; }
        public DateTime FpKeluaranDate { get; set; }
        public string Keterangan { get; set; }
        public string UserId { get; set; }
        public decimal TotalPpn { get; set; }
        public int FakturCount { get; set; }

        public void CalculateTotalPpn()
        {
            ListFaktur.ForEach(x => x.CalculatePpn());
            TotalPpn = ListFaktur.Sum(x => x.Ppn);
        }
        public void CalculateFakturCount()
        {
            FakturCount = ListFaktur.Count;
        }
        public void GenKeterangan()
        {
            if (ListFaktur.Count == 0) return;
            //  generate string keterangan yang berisi tanggal terbesar dan terkecil faktur
            var minDate = ListFaktur.Min(x => x.TanggalFaktur);
            var maxDate = ListFaktur.Max(x => x.TanggalFaktur);
            if (minDate == maxDate)
                Keterangan = $"Faktur Jual Tgl {minDate:dd-MM-yyyy}";
            else
                Keterangan = $"Faktur Jual Tgl {minDate:dd-MM-yyyy} s/d {maxDate:dd-MM-yyyy}";

        }

        public List<FpKeluaranFakturModel> ListFaktur { get; set; }
    }
}
