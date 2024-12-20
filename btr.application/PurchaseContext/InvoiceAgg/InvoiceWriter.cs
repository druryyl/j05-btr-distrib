using System.Linq;
using btr.application.SupportContext.UserAgg;
using btr.domain.PurchaseContext.InvoiceAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using FluentValidation;

namespace btr.application.PurchaseContext.InvoiceAgg
{
    public interface IInvoiceWriter : INunaWriter2<InvoiceModel>
    {
    }
    public class InvoiceWriter : IInvoiceWriter
    {
        private readonly IInvoiceDal _invoiceDal;
        private readonly IInvoiceItemDal _invoiceItemDal;
        private readonly IInvoiceDiscDal _invoiceDiscDal;
        private readonly INunaCounterBL _counter;
        private readonly IValidator<InvoiceModel> _validator;
        private readonly IUserBuilder _userBuilder;

        public InvoiceWriter(IInvoiceDal invoiceDal,
            IInvoiceItemDal invoiceItemDal,
            IInvoiceDiscDal invoiceDiscDal,
            INunaCounterBL counter,
            IValidator<InvoiceModel> validator,
            IUserBuilder userBuilder)
        {
            _invoiceDal = invoiceDal;
            _invoiceItemDal = invoiceItemDal;
            _invoiceDiscDal = invoiceDiscDal;
            _counter = counter;
            _validator = validator;
            _userBuilder = userBuilder;
        }

        public InvoiceModel Save(InvoiceModel model)
        {
            //  VALIDATE
            _validator.ValidateAndThrow(model);

            //  GENERATE-ID
            if (model.InvoiceId.IsNullOrEmpty())
                model.InvoiceId = _counter.Generate("INVC", IDFormatEnum.PREFYYMnnnnnC);

            foreach (var item in model.ListItem)
            {
                item.InvoiceId = model.InvoiceId;
                item.InvoiceItemId = $"{model.InvoiceId}-{item.NoUrut:D3}";
                foreach (var item2 in item.ListDisc)
                {
                    item2.InvoiceId = model.InvoiceId;
                    item2.InvoiceItemId = item.InvoiceItemId;
                    item2.InvoiceDiscId = $"{item.InvoiceItemId}-{item2.NoUrut:D1}";
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