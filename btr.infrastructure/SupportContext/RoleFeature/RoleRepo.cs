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
        private readonly IRoleMenuDal _roleMenuDal;

        public RoleTypeRepo(IRoleDal roleDal, 
            IRoleMenuDal roleMenuDal)
        {
            _roleDal = roleDal;
            _roleMenuDal = roleMenuDal;
        }

        public void SaveChanges(RoleType model)
        {
            LoadEntity(model)
                .Match(
                    onSome: _ => _roleDal.Update(RoleDto.FromModel(model)),
                    onNone: () => _roleDal.Insert(RoleDto.FromModel(model)));
            
            _roleMenuDal.Delete(model);
            var listDto = model.ListMenu.Select(x => RoleMenuDto.FromModel(x, model));
            _roleMenuDal.Insert(listDto);
        }

        public void DeleteEntity(IRoleKey key)
        {
            _roleDal.Delete(key);
        }

        public MayBe<RoleType> LoadEntity(IRoleKey key)
        {
            var data = _roleDal.GetData(key);
            var listDto = _roleMenuDal.ListData(key)?.ToList() ?? new List<RoleMenuDto>();
            var listDtl = listDto.Select(x => x.ToModel());
            var model = data?.ToModel(listDtl);
            return MayBe.From(model);
        }

        public IEnumerable<RoleView> ListRole()
        {
            var listDto = _roleDal.ListData()?.ToList() ?? new List<RoleDto>();
            var result = listDto.Select(x => new RoleView(x.RoleId, x.RoleName));
            return result;
        }
    }

}
