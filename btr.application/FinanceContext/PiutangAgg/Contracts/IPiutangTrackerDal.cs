using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.Contracts
{
    public interface IPiutangTrackerDal :
        IListData<PiutangTrackerDto, IPiutangKey>
    {
    }

    public class PiutangTrackerDto
    {
        public DateTime Tgl { get; set; }
        public string Keterangan { get; set; }
        public string ReffId { get; set; }
        public decimal Piutang { get; set; }
        public decimal Tagihan { get; set; }
        public decimal Pelunasan{ get; set; }

    }
}
