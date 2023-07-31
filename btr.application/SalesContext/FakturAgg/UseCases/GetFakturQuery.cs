﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using btr.application.SalesContext.FakturAgg.Workers;
using btr.domain.SalesContext.FakturAgg;
using Dawn;
using Mapster;
using MediatR;

namespace btr.application.SalesContext.FakturAgg.UseCases
{
    public class GetFakturQuery : IRequest<GetFakturResponse>, IFakturKey
    {
        public GetFakturQuery(string id) => FakturId = id;
        public string FakturId { get; }
    }

    public class GetFakturResponse
    {
        public string FakturId { get; set; }
        public string FakturDate { get; set; }

        public string SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double Plafond { get; set; }
        public double CreditBalance { get; set; }

        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string TglRencanaKirim { get; set; }

        public string TermOfPayment { get; set; }

        public double Total { get; set; }
        public double DiscountLain { get; set; }
        public double BiayaLain { get; set; }
        public double GrandTotal { get; set; }

        public double UangMuka { get; set; }
        public double KurangBayar { get; set; }

        public string CreateTime { get; set; }
        public string LastUpdate { get; set; }
        public string UserId { get; set; }

        public List<GetFakturResponseItem> ListItem { get; set; }
    }

    public class GetFakturResponseItem
    {
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public string BrgName { get; set; }

        public int AvailableQty { get; set; }
        public int Qty { get; set; }
        public double HargaJual { get; set; }
        public double SubTotal { get; set; }
        public double DiscountRp { get; set; }
        public double PpnProsen { get; set; }
        public double PpnRp { get; set; }
        public double Total { get; set; }

        public List<GetFakturResponseQtyHarga> ListQtyHarga { get; set; }
        public List<GetFakturResponseDiscount> ListDiscount { get; set; }
    }

    public class GetFakturResponseQtyHarga
    {
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public string FakturQtyHargaId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public string Satuan { get; set; }
        public int Conversion { get; set; }
        public int Qty { get; set; }
        public double HargaJual { get; set; }
    }

    public class GetFakturResponseDiscount
    {
        public string FakturId { get; set; }
        public string FakturItemId { get; set; }
        public string FakturDiscountId { get; set; }
        public int NoUrut { get; set; }

        public string BrgId { get; set; }
        public double DiscountProsen { get; set; }
        public double DiscountRp { get; set; }
    }

    public class GetFakturHandler : IRequestHandler<GetFakturQuery, GetFakturResponse>
    {
        private FakturModel _aggRoot = new FakturModel();
        private readonly IFakturBuilder _builder;

        public GetFakturHandler(IFakturBuilder builder)
        {
            _builder = builder;
        }

        public Task<GetFakturResponse> Handle(GetFakturQuery request, CancellationToken cancellationToken)
        {
            //  GUARD
            Guard.Argument(() => request)
                .Member(x => x.FakturId, y => y.NotEmpty());

            //  BUILD
            _aggRoot = _builder
                .Load(request)
                .Build();

            //  RESPONSE
            var result = GenResponse();
            return Task.FromResult(result);
        }

        private GetFakturResponse GenResponse()
        {
            var result = _aggRoot.Adapt<GetFakturResponse>();
            result.FakturDate = _aggRoot.FakturDate.ToString("yyyy-MM-dd");
            result.TglRencanaKirim = _aggRoot.TglRencanaKirim.ToString("yyyy-MM-dd");
            result.CreateTime = _aggRoot.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            result.LastUpdate = _aggRoot.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
    }
}