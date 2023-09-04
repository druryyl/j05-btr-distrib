using btr.domain.SupportContext.ParamUser;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SupportContext.ParamUserAgg
{
    public interface IParamUserDal :
        IInsertBulk<ParamUserModel>,
        IDelete<IUserKey>,
        IListData<ParamUserModel, IUserKey>
    {
    }
}