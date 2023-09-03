using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SupportContext.UserAgg
{
    public interface IUserWriter : INunaWriter<UserModel>
    {

    }
    public class UserWriter : IUserWriter
    {
        private readonly IUserDal _userDal;
        private readonly IValidator<UserModel> _validator;

        public UserWriter(IUserDal userDal, IValidator<UserModel> validator)
        {
            _userDal = userDal;
            _validator = validator;
        }

        public void Save(ref UserModel model)
        {
            _validator.ValidateAndThrow(model);
            var db = _userDal.GetData(model);
            if (db is null)
                _userDal.Insert(model);
            else
                _userDal.Update(model);

        }
    }
}
