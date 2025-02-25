using btr.domain.FinanceContext.TagihanAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.FinanceContext.TagihanAgg
{
    public class TagihanPrintOutDto
    {
        public static readonly string[] hari = { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };

        public TagihanPrintOutDto(TagihanModel tagihan)
        {
            Sales = $"SALES: {tagihan.SalesPersonName}";
            Tgl = $"Tanggal: {hari[(int)tagihan.TagihanDate.DayOfWeek]}, {tagihan.TagihanDate:dd-MMM-yyyy}";

            ListItem = new List<TagihanPrintOutItemDto>();
            decimal totalTagihan = 0;
            decimal totalTerbayar = 0;
            decimal totalSisa = 0;
            foreach (var item in tagihan.ListFaktur)
            {
                var sisa = item.NilaiTotal - item.NilaiTerbayar;
                totalTagihan += item.NilaiTotal;
                totalTerbayar += item.NilaiTerbayar;
                totalSisa += sisa;
                var newItem = new TagihanPrintOutItemDto()
                {
                    No = item.NoUrut,
                    FakturCode = item.FakturCode,
                    Customer = item.CustomerName,
                    FakturDate = $"{item.FakturDate:dd-MM-yyyy}",
                    Total = $"{item.NilaiTotal:N0}",
                    TerBayar = $"{item.NilaiTerbayar}",
                    Sisa = $"{sisa:N0}"   
                };
                ListItem.Add(newItem);
            }
            TotalTagihan = $"{totalTagihan:N0}";
            TotalTerBayar = $"{totalTerbayar:N0}";
            TotalSisa = $"{totalSisa:N0}";

        }
        public string Sales { get; set; }
        public string Tgl { get; set; }
        public string TotalTagihan { get; set; }
        public string TotalTerBayar { get; set; }
        public string TotalSisa { get; set; }
        public List<TagihanPrintOutItemDto> ListItem { get; set; }

    }
}