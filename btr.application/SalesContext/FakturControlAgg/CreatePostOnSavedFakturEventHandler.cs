﻿using btr.domain.SupportContext.UserAgg;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.FakturAgg.UseCases;

namespace btr.application.SalesContext.FakturControlAgg
{
    public class CreatePostOnSavedFakturEventHandler : INotificationHandler<SavedFakturEvent>
    {
        private readonly IFakturControlBuilder _builder;
        private readonly IFakturControlWriter _writer;

        public CreatePostOnSavedFakturEventHandler(IFakturControlWriter writer, IFakturControlBuilder builder)
        {
            _writer = writer;
            _builder = builder;
        }

        public Task Handle(SavedFakturEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Aggregate.IsVoid)
                return Task.FromResult(Unit.Value);

            var fakturControl = _builder.
                LoadOrCreate(notification.Aggregate)
                .Posted(new UserModel(notification.Aggregate.UserId))
                .Build();
            _ = _writer.Save(fakturControl);
            return Task.FromResult(Unit.Value);
        }
    }
}
