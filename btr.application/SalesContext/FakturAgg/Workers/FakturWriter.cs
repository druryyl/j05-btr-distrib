using System.Linq;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.domain.SalesContext.FakturAgg;
using btr.nuna.Application;
using FluentValidation;
using btr.nuna.Domain;
using btr.application.SupportContext.UserAgg;
using btr.domain.SupportContext.UserAgg;
using System;

namespace btr.application.SalesContext.FakturAgg.Workers
{
    public interface IFakturWriter : INunaWriter2<FakturModel>
    {
    }
    public class FakturWriter : IFakturWriter
    {
        private readonly IFakturDal _fakturDal;
        private readonly IFakturItemDal _fakturItemDal;
        private readonly IFakturCodeOpenDal _fakturCodeOpenDal;
        private readonly IFakturDiscountDal _fakturDiscountDal;
        private readonly IFakturItemKlaimDal _fakturItemKlaimDal;
        private readonly IFakturDiscountKlaimDal _fakturDiscountKlaimDal;

        private readonly INunaCounterBL _counter;
        private readonly IValidator<FakturModel> _validator;
        private readonly IUserBuilder _userBuilder;

        public FakturWriter(IFakturDal fakturDal,
            IFakturItemDal fakturItemDal,
            IFakturDiscountDal fakturDiscountDal,
            INunaCounterBL counter,
            IValidator<FakturModel> validator,
            IUserBuilder userBuilder,
            IFakturCodeOpenDal fakturCodeOpenDal,
            IFakturItemKlaimDal fakturItemKlaimDal,
            IFakturDiscountKlaimDal fakturDiscountKlaimDal)
        {
            _fakturDal = fakturDal;
            _fakturItemDal = fakturItemDal;
            _fakturDiscountDal = fakturDiscountDal;
            _counter = counter;
            _validator = validator;
            _userBuilder = userBuilder;
            _fakturCodeOpenDal = fakturCodeOpenDal;
            _fakturItemKlaimDal = fakturItemKlaimDal;
            _fakturDiscountKlaimDal = fakturDiscountKlaimDal;
        }

        public FakturModel Save(FakturModel model)
        {
            //  VALIDATE
            _validator.ValidateAndThrow(model);

            //  GENERATE-ID
            if (model.FakturId.IsNullOrEmpty())
                model.FakturId = _counter.Generate("FKTR", IDFormatEnum.PREFYYMnnnnnC);

            if (!model.IsVoid)
                if (model.FakturCode.IsNullOrEmpty())
                    model.FakturCode = GenerateFakturCode(model, model.FakturDate);
            
            foreach (var item in model.ListItem)
            {
                item.FakturId = model.FakturId;
                item.FakturItemId = $"{model.FakturId}-{item.NoUrut:D2}";
                foreach (var item2 in item.ListDiscount)
                {
                    item2.FakturId = model.FakturId;
                    item2.FakturItemId = item.FakturItemId;
                    item2.FakturDiscountId = $"{item.FakturItemId}-{item2.NoUrut:D1}";
                }
            }
            var allDiscount = model.ListItem.SelectMany(x => x.ListDiscount, (hdr, dtl) => dtl);

            foreach (var item in model.ListItemKlaim)
            {
                item.FakturId = model.FakturId;
                item.FakturItemId = $"{model.FakturId}-{item.NoUrut:D2}";
                foreach (var item2 in item.ListDiscount)
                {
                    item2.FakturId = model.FakturId;
                    item2.FakturItemId = item.FakturItemId;
                    item2.FakturDiscountId = $"{item.FakturItemId}-{item2.NoUrut:D1}";
                }
            }
            var allDiscountKlaim = model.ListItemKlaim.SelectMany(x => x.ListDiscount, (hdr, dtl) => dtl);

            //  WRITE
            using (var trans = TransHelper.NewScope())
            {
                var db = _fakturDal.GetData((IFakturKey)model);
                if (db is null)
                    _fakturDal.Insert(model);
                else
                    _fakturDal.Update(model);

                _fakturItemDal.Delete(model);
                _fakturDiscountDal.Delete(model);
                _fakturItemKlaimDal.Delete(model);
                _fakturDiscountKlaimDal.Delete(model);

                _fakturItemDal.Insert(model.ListItem);
                _fakturDiscountDal.Insert(allDiscount);
                _fakturItemKlaimDal.Insert(model.ListItemKlaim);
                _fakturDiscountKlaimDal.Insert(allDiscountKlaim);

                _fakturCodeOpenDal.Delete(model.FakturCode);
                trans.Complete();
            }
            return model;
        }

        private string GenerateFakturCode(IUserKey userKey, DateTime tgl)
        {
            var user = _userBuilder.Load(userKey).Build();
            var prefix = user.Prefix;
            var result = string.Empty;
            result = _counter.Generate(prefix, IDFormatEnum.Pn7);
            if (tgl.Year > 2025)

            if (tgl.Year < 2026)
                result = _counter.Generate(prefix, IDFormatEnum.Pn7);
            else
            {
                result = _counter.Generate($"{prefix}{tgl:yy}", IDFormatEnum.Pn7);
                result = result.Remove(3, 2);
            }
            return result;
        }
    }
}