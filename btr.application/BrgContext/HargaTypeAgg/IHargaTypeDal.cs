using btr.domain.BrgContext.HargaTypeAgg;
using btr.nuna.Infrastructure;

namespace btr.application.BrgContext.HargaTypeAgg
{
    public interface IHargaTypeDal :
        IInsert<HargaTypeModel>,
        IUpdate<HargaTypeModel>,
        IDelete<IHargaTypeKey>,
        IGetData<HargaTypeModel, IHargaTypeKey>,
        IListData<HargaTypeModel>
    {
    }
}