using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SupportContext.UserAgg
{
    public interface IUserBuilder : INunaBuilder<UserModel>
    {
        IUserBuilder LoadOrCreate(IUserKey userKey);
        IUserBuilder Load(IUserKey userKey);
        IUserBuilder Attach(UserModel user);

        IUserBuilder UserName(string userName);
        IUserBuilder Password(string password);
        IUserBuilder Prefix(string prefix);
    }

    public class UserBuilder : IUserBuilder
    {
        private UserModel _agg;
        private readonly IUserDal _userDal;

        public UserBuilder(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public UserModel Build()
        {
            _agg.RemoveNull();
            return _agg;
        }

        public IUserBuilder LoadOrCreate(IUserKey userKey)
        {
            var user = _userDal.GetData(userKey);
            if (user is null)
                _agg = new UserModel
                {
                    UserId = userKey.UserId
                };
            else
                _agg = user;

            return this;
        }

        public IUserBuilder Load(IUserKey userKey)
        {
            _agg = _userDal.GetData(userKey)
                ?? throw new KeyNotFoundException("UserId not found");
            return this;
        }

        public IUserBuilder UserName(string userName)
        {
            _agg.UserName = userName;
            return this;
        }

        public IUserBuilder Password(string password)
        {
            _agg.Password = password.HashSha256();
            return this;
        }

        public IUserBuilder Attach(UserModel user)
        {
            _agg = user;
            return this;

        }

        public IUserBuilder Prefix(string prefix)
        {
            _agg.Prefix = prefix;
            return this;
        }
    }
}
