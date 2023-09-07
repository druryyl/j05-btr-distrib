using btr.domain.SupportContext.UserAgg;
using btr.nuna.Infrastructure;

namespace btr.application.SupportContext.UserAgg
{
    public interface IUserDal :
        IInsert<UserModel>,
        IUpdate<UserModel>,
        IDelete<IUserKey>,
        IGetData<UserModel, IUserKey>,
        IListData<UserModel>
    {
    }
}
