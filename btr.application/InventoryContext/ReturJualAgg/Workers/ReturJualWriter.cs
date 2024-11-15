using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.application.SupportContext.UserAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System.Linq;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public interface IReturJualWriter : INunaWriter2<ReturJualModel>
    {
    }

    public class ReturJualWriter : IReturJualWriter
    {
        private readonly IReturJualDal _returJualDal;
        private readonly IReturJualItemDal _returJualItemDal;
        private readonly IReturJualItemDiscDal _returJualItemDiscDal;
        private readonly INunaCounterBL _counter;
        private readonly IUserBuilder _userBuilder;

        public ReturJualWriter(IReturJualDal returJualDal,
            IReturJualItemDal returJualItemDal,
            INunaCounterBL counter,
            IUserBuilder userBuilder,
            IReturJualItemDiscDal returJualItemDiscDal)
        {
            _returJualDal = returJualDal;
            _returJualItemDal = returJualItemDal;
            _counter = counter;
            _userBuilder = userBuilder;
            _returJualItemDiscDal = returJualItemDiscDal;
        }


        public ReturJualModel Save(ReturJualModel model)
        {
            if (model.ReturJualId.IsNullOrEmpty())
                model.ReturJualId = _counter.Generate("RETJ", IDFormatEnum.PREFYYMnnnnnC);


            if (model.ReturJualCode.IsNullOrEmpty())
                model.ReturJualCode = GenerateReturJualCode(model);

            var returJualId = model.ReturJualId;
            model.ListItem.ForEach(x =>
            {
                x.ReturJualId = returJualId;
                x.ReturJualItemId = $"{returJualId}-{x.NoUrut:D2}";
                x.ListDisc.ForEach(y =>
                {
                    y.ReturJualId = x.ReturJualId;
                    y.ReturJualItemId = x.ReturJualItemId;
                    y.ReturJualItemDiscId = $"{x.ReturJualItemId}-{y.NoUrut:D1}";
                });
            });

            var db = _returJualDal.GetData(model);
            var allDiscount = model.ListItem.SelectMany(x => x.ListDisc, (hdr, dtl) => dtl);

            using (var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _returJualDal.Insert(model);
                else
                    _returJualDal.Update(model);

                _returJualItemDal.Delete(model);
                _returJualItemDal.Insert(model.ListItem);

                _returJualItemDiscDal.Delete(model);
                _returJualItemDiscDal.Insert(allDiscount);
                trans.Complete();
            }
            return model;
        }
        private string GenerateReturJualCode(IUserKey userKey)
        {
            var user = _userBuilder.Load(userKey).Build();
            var prefix = $"~{user.Prefix}";
            var result = _counter.Generate(prefix, IDFormatEnum.Pn7);
            result = result.Substring(1);
            return result;
        }

    }
}
