using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPenerimaanPelunasanSalesDal : IListData<PenerimaanPelunasanSalesDto, Periode>
    {
    }

    public class PenerimaanPelunasanSalesDto
    {
        public string SalesName { get; set; }
        public DateTime LunasDate { get; set; }

        public decimal BayarTunai { get; set; }
        public decimal BayarGiro { get; set; }
        public decimal Retur { get; set; }
        public decimal Potongan { get; set; }
        public decimal MateraiAdmin { get; set; }

        public decimal TotalBayar{ get; set; }
    }
}
