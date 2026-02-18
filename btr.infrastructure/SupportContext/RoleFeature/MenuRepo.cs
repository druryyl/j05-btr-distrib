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
    public class MenuTypeRepo : IMenuRepo
    {
        private readonly IMenuDal _menuDal;

        public MenuTypeRepo(IMenuDal menuDal)
        {
            _menuDal = menuDal;
        }

        public void SaveChanges(MenuType model)
        {
            LoadEntity(model)
                .Match(
                    onSome: _ => _menuDal.Update(MenuDto.FromModel(model)),
                    onNone: () => _menuDal.Insert(MenuDto.FromModel(model)));
        }

        public void DeleteEntity(IMenuKey key)
        {
            _menuDal.Delete(key);
        }

        public MayBe<MenuType> LoadEntity(IMenuKey key)
        {
            var data = _menuDal.GetData(key);
            var model = data?.ToModel();
            return MayBe.From(model);
        }

        public IEnumerable<MenuType> ListData()
        {
            var listDto = _menuDal.ListData()?.ToList() ?? new List<MenuDto>();
            var result = listDto.Select(x => x.ToModel());
            return result;
        }
    }
}
