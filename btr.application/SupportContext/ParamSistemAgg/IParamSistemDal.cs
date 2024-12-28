using btr.domain.SupportContext.ParamSistemAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SupportContext.ParamSistemAgg
{
    public interface IParamSistemDal :
        IInsert<ParamSistemModel>,
        IUpdate<ParamSistemModel>,
        IDelete<ParamSistemModel>,
        IGetData<ParamSistemModel, IParamSistemKey>,
        IListData<ParamSistemModel>
    {
    }
}
