using System.Linq;
using btr.application.PurchaseContext.ReturBeliFeature;
using btr.application.SupportContext.UserAgg;
using btr.domain.PurchaseContext.ReturBeliFeature;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.PurchaseContext.ReturBeliAgg
{
    public interface IReturBeliWriter : INunaWriter2<ReturBeliModel>
    {
    }
    public class ReturBeliWriter : IReturBeliWriter
    {
        private readonly IReturBeliDal _invoiceDal;
        private readonly IReturBeliItemDal _invoiceItemDal;
        private readonly IReturBeliDiscDal _invoiceDiscDal;
        private readonly INunaCounterBL _counter;
        private readonly IValidator<ReturBeliModel> _validator;
        private readonly IUserBuilder _userBuilder;

        public ReturBeliWriter(IReturBeliDal invoiceDal,
            IReturBeliItemDal invoiceItemDal,
            IReturBeliDiscDal invoiceDiscDal,
            INunaCounterBL counter,
            IValidator<ReturBeliModel> validator,
            IUserBuilder userBuilder)
        {
            _invoiceDal = invoiceDal;
            _invoiceItemDal = invoiceItemDal;
            _invoiceDiscDal = invoiceDiscDal;
            _counter = counter;
            _validator = validator;
            _userBuilder = userBuilder;
        }

        public ReturBeliModel Save(ReturBeliModel model)
        {
            //  VALIDATE
            _validator.ValidateAndThrow(model);

            //  GENERATE-ID
            if (model.ReturBeliId.IsNullOrEmpty())
                model.ReturBeliId = _counter.Generate("INVC", IDFormatEnum.PREFYYMnnnnnC);

            foreach (var item in model.ListItem)
            {
                item.ReturBeliId = model.ReturBeliId;
                item.ReturBeliItemId = $"{model.ReturBeliId}-{item.NoUrut:D3}";
                foreach (var item2 in item.ListDisc)
                {
                    item2.ReturBeliId = model.ReturBeliId;
                    item2.ReturBeliItemId = item.ReturBeliItemId;
                    item2.ReturBeliDiscId = $"{item.ReturBeliItemId}-{item2.NoUrut:D1}";
                }
            }
            var allDiscount = model.ListItem.SelectMany(x => x.ListDisc, (hdr, dtl) => dtl);

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                var db = _invoiceDal.GetData(model);
                if (db is null)
                    _invoiceDal.Insert(model);
                else
                    _invoiceDal.Update(model);

                _invoiceItemDal.Delete(model);
                _invoiceDiscDal.Delete(model);

                _invoiceItemDal.Insert(model.ListItem);
                _invoiceDiscDal.Insert(allDiscount);

                trans.Complete();
            }
            return model;
        }
    }
}