using btr.application.InventoryContext.ReturJualAgg.Contracts;
using btr.application.SupportContext.UserAgg;
using btr.domain.InventoryContext.ReturJualAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.InventoryContext.ReturJualAgg.Workers
{
    public interface IReturJualWriter : INunaWriter2<ReturJualModel>
    {
    }

    public class ReturJualWriter : IReturJualWriter
    {
        private readonly IReturJualDal _returJualDal;
        private readonly IReturJualItemDal _returJualItemDal;
        private readonly INunaCounterBL _counter;
        private readonly IUserBuilder _userBuilder;

        public ReturJualWriter(IReturJualDal returJualDal,
            IReturJualItemDal returJualItemDal,
            INunaCounterBL counter,
            IUserBuilder userBuilder)
        {
            _returJualDal = returJualDal;
            _returJualItemDal = returJualItemDal;
            _counter = counter;
            _userBuilder = userBuilder;
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
            });

            var db = _returJualDal.GetData(model);
            
            using(var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _returJualDal.Insert(model);
                else
                    _returJualDal.Update(model);

                _returJualItemDal.Delete(model);
                _returJualItemDal.Insert(model.ListItem);
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
