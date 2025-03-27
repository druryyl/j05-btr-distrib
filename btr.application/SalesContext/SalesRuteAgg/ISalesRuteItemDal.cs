using btr.domain.SalesContext.SalesPersonAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SalesContext.SalesPersonAgg
{
    public interface ISalesRuteItemDal :
        IInsertBulk<SalesRuteItemModel>,
        IDelete<ISalesRuteKey>,
        IListData<SalesRuteItemModel, ISalesRuteKey>
    { }

    public interface ISalesRuteItemViewDal :
        IListData<SalesRuteItemViewDto, ISalesPersonKey>
    {}

    public class SalesRuteItemViewDto
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string HariRuteId { get; set; }
        public string HariRuteName { get; set; }
        public string ShortName { get; set; }
    }



}
