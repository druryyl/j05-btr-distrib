using System.Collections.Generic;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.InventoryContext.MutasiAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using Dawn;
using MediatR;

namespace btr.application.InventoryContext.MutasiAgg
{
    public class SaveMutasiRequest : IMutasiKey,
        IWarehouseKey, IUserKey
    {
        public string MutasiId { get; set; }
        public string MutasiDate { get; set; }
        public string WarehouseId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<SaveMutasiRequestItem> ListBrg { get; set; }
    }

    public class SaveMutasiRequestItem : IBrgKey
    {
        public string BrgId { get; set; }
        public string QtyString { get; set; }
    }

    public interface ISaveMutasiWorker : INunaService<MutasiModel, SaveMutasiRequest> { }

    public class SaveMutasiWorker : ISaveMutasiWorker
    {
        private readonly IMutasiBuilder _mutasiBuilder;
        private readonly IMutasiWriter _mutasiWriter;
        private readonly IMediator _mediator;
        //private readonly IGenStokMutasiWorker _genStokWorker;

        public SaveMutasiWorker(IMutasiBuilder mutasiBuilder,
            IMutasiWriter mutasiWriter,
            IMediator mediator)
        {
            _mutasiBuilder = mutasiBuilder;
            _mutasiWriter = mutasiWriter;
            _mediator = mediator;
            //_genStokWorker = genStokWorker;
        }

        public MutasiModel Execute(SaveMutasiRequest req)
        {
            //  GUARD
            Guard.Argument(() => req).NotNull()
                .Member(x => x.MutasiDate, y => y.ValidDate("yyyy-MM-dd"))
                .Member(x => x.WarehouseId, y => y.NotEmpty());

            //  PROSES FAKTUR
            MutasiModel result;
            using (var trans = TransHelper.NewScope())
            {
                result = SaveMutasi(req);

                //var genStokReq = new GenStokMutasiRequest(result.MutasiId);
                //_genStokWorker.Execute(genStokReq);

                trans.Complete();
            }

            //  RESULT
            return result;
        }

        private MutasiModel SaveMutasi(SaveMutasiRequest req)
        {
            //  BUILD
            MutasiModel result;
            if (req.MutasiId.Length == 0)
            {
                result = _mutasiBuilder.CreateNew(req).Build();
            }
            else
            {
                result = _mutasiBuilder.Load(req).Build();
                result.ListItem.Clear();
            }

            result = _mutasiBuilder
                .Attach(result)
                .Warehouse(req)
                .User(req)
                .Build();

            foreach (var item in req.ListBrg)
            {
                result = _mutasiBuilder
                    .Attach(result)
                    .AddItem(item, item.QtyString)
                    .Build();
            }
            result = _mutasiBuilder
                .Attach(result)
                .CalcTotal()
                .Build();

            //  APPLY
            _ = _mutasiWriter.Save(result);
            _mediator.Publish(new SavedMutasiEvent(req, result));
            return result;
        }
    }

    public class SavedMutasiEvent : INotification
    {
        public SavedMutasiEvent(SaveMutasiRequest request, MutasiModel aggregate)
        {
            Request = request;
            Aggregate = aggregate;
        }
        public SaveMutasiRequest Request { get; }
        public MutasiModel Aggregate { get; }
    }
}
