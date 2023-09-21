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
    public class ChangeToCashFakturRequest : IFakturKey, IUserKey
    {
        public ChangeToCashFakturRequest(string fakturId, string userId)
        {
            FakturId = fakturId;
            UserId = userId;
        }
        public string FakturId { get; set; }
        public string UserId { get; set; }
    }

    public interface IChangeToCashFakturWorker : INunaServiceVoid<ChangeToCashFakturRequest> { }

    public class ChangeToCashFakturWorker : IChangeToCashFakturWorker
    {
        private readonly IFakturBuilder _fakturBuilder;
        private readonly IFakturControlBuilder _fakturControlBuilder;
        private readonly IFakturWriter _fakturWriter;
        private readonly IFakturControlWriter _fakturControlWriter;

        public ChangeToCashFakturWorker(IFakturBuilder fakturBuilder, 
            IFakturControlBuilder fakturControlBuilder, 
            IFakturWriter fakturWriter, 
            IFakturControlWriter fakturControlWriter)
        {
            _fakturBuilder = fakturBuilder;
            _fakturControlBuilder = fakturControlBuilder;
            _fakturWriter = fakturWriter;
            _fakturControlWriter = fakturControlWriter;
        }

        public void Execute(ChangeToCashFakturRequest req)
        {
            var faktur = _fakturBuilder
                .Load(req)
                .TermOfPayment(TermOfPaymentEnum.Cash)
                .Build();
            var fakturControl = _fakturControlBuilder
                .LoadOrCreate(req)
                .Lunas(req)
                .Build();

            using (var trans = TransHelper.NewScope())
            {
                _ = _fakturWriter.Save(faktur);
                _fakturControlWriter.Save(fakturControl);
                trans.Complete();
            }
        }
    }
}
