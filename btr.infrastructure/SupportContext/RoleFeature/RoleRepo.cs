using btr.application.SupportContext.RoleFeature;
using btr.domain.SupportContext.RoleFeature;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.SupportContext.RoleFeature
{
    public class RoleTypeRepo : IRoleRepo
    {
        private readonly IRoleDal _roleDal;

        public RoleTypeRepo(IRoleDal roleDal)
        {
            _roleDal = roleDal;
        }

        public void SaveChanges(RoleType model)
        {
            LoadEntity(model)
                .Match(
                    onSome: _ => _roleDal.Update(RoleDto.FromModel(model)),
                    onNone: () => _roleDal.Insert(RoleDto.FromModel(model)));
        }

        public void DeleteEntity(IRoleKey key)
        {
            _roleDal.Delete(key);
        }

        public MayBe<RoleType> LoadEntity(IRoleKey key)
        {
            var data = _roleDal.GetData(key);
            var model = data?.ToModel();
            return MayBe.From(model);
        }

        public IEnumerable<RoleType> ListRole()
        {
            var listDto = _roleDal.ListData()?.ToList() ?? new List<RoleDto>();
            var result = listDto.Select(x => x.ToModel());
            return result;
        }
    }
}
