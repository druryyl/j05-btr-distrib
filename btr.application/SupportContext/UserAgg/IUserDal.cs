using btr.domain.SupportContext.UserAgg;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
