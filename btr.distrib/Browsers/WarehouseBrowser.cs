using btr.application.InventoryContext.WarehouseAgg.UseCases;
using btr.distrib.SharedForm;
using btr.domain.InventoryContext.WarehouseAgg;
using MediatR;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btr.distrib.Browsers
{
    public interface IWarehouseBrowser : IBrowser
    {
    }

    public class WarehouseBrowser : IWarehouseBrowser
    {
        private readonly IMediator _mediator;

        public WarehouseBrowser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Browse(string defaultValue)
        {
            var fallback = Policy<IEnumerable<WarehouseModel>>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new List<WarehouseModel>());
            var query = new ListWarehouseQuery();
            var response = Task.Run(() => fallback.ExecuteAsync(() => _mediator.Send(query)))
                .GetAwaiter()
                .GetResult();

            var listWarehouse = response
                .Select(x => new WarehouseBrowseProjection
                { 
                    Id = x.WarehouseId, 
                    WarehouseName = x.WarehouseName 
                });

            var form = new BrowserForm<WarehouseBrowseProjection, string>(listWarehouse, string.Empty, x => x.WarehouseName);
            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.ReturnedValue;
            else
                return defaultValue;
        }
    }
    public class WarehouseBrowseProjection
    {
        public string Id { get; set; }
        public string WarehouseName { get; set; }
    }

}
