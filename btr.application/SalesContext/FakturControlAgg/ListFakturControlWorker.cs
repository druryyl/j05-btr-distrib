using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.FakturControlAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using Mapster;
using System.Collections.Generic;
using System.Linq;

namespace btr.application.SalesContext.FakturControlAgg
{
    public interface IListFaktorControlWorker : INunaService<IEnumerable<FakturControlModel>, Periode>
    {
    }
    
    public class ListFakturControlWorker : IListFaktorControlWorker
    {
        private readonly IFakturDal _fakturDal;
        private readonly IFakturControlStatusDal _fakturControlStatusDal;

        public ListFakturControlWorker(IFakturDal fakturDal, IFakturControlStatusDal fakturControlStatusDal)
        {
            _fakturDal = fakturDal;
            _fakturControlStatusDal = fakturControlStatusDal;
        }

        public IEnumerable<FakturControlModel> Execute(Periode req)
        {
            var listFaktur = _fakturDal.ListData(req)?.ToList()
                ?? new List<FakturModel>();
            var listStatus = _fakturControlStatusDal.ListData(req)?.ToList()
                ?? new List<FakturControlStatusModel>();
            var result = listFaktur.Adapt<List<FakturControlModel>>();

            foreach (var item in result)
            {
                item.ListStatus = new List<FakturControlStatusModel>();
                item.ListStatus.AddRange(listStatus.Where(x => x.FakturId == item.FakturId).ToList());
            }
            return result;
        }
    }
}
