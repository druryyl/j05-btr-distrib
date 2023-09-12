using btr.application.SalesContext.FakturAgg.Workers;
using btr.application.SalesContext.FakturControlAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.FakturAgg.UseCases
{
    public class ChangeToCreditFakturRequest : IFakturKey, IUserKey
    {
        public ChangeToCreditFakturRequest(string fakturId, string userId)
        {
            FakturId = fakturId;
            UserId = userId;
        }
        public string FakturId { get; set; }
        public string UserId { get; set; }
    }

    public interface IChangeToCreditFakturWorker : INunaServiceVoid<ChangeToCreditFakturRequest> { }

    public class ChangeToCreditFakturWorker : IChangeToCreditFakturWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturControlBuilder _fakturControlBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly IFakturControlWriter _fakturControlWriter;

        public ChangeToCreditFakturWorker(IFakturBuilder fakturBuilder,
            IFakturControlBuilder fakturControlBuilder,
            IFakturWriter fakturWriter,
            IFakturControlWriter fakturControlWriter)
        {
            _fakturBuilder = fakturBuilder;
            _fakturControlBuilder = fakturControlBuilder;
            _fakturWriter = fakturWriter;
            _fakturControlWriter = fakturControlWriter;
        }

        public void Execute(ChangeToCreditFakturRequest req)
        {
            var faktur = _fakturBuilder
                .Load(req)
                .TermOfPayment(TermOfPaymentEnum.Credit)
                .Build();
            var fakturControl = _fakturControlBuilder
                .LoadOrCreate(req)
                .CancelLunas(req)
                .Build();

            using (var trans = TransHelper.NewScope())
            {
                _fakturWriter.Save(ref faktur);
                _fakturControlWriter.Save(fakturControl);
                trans.Complete();
            }
        }
    }
}
