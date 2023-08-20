using btr.application.PurchaseContext.SupplierAgg.UseCases;
using btr.distrib.SharedForm;
using btr.domain.PurchaseContext.SupplierAgg;
using MediatR;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.Browsers
{
    public interface ISupplierBrowser : IBrowser
    {
    }

    public class SupplierBrowser : ISupplierBrowser
    {
        private readonly IMediator _mediator;

        public SupplierBrowser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Browse(string defaultValue)
        {
            var fallback = Policy<IEnumerable<SupplierModel>>
                .Handle<KeyNotFoundException>().Or<ArgumentException>()
                .FallbackAsync(new List<SupplierModel>());
            var query = new ListSupplierQuery();
            var response = Task.Run(() => fallback.ExecuteAsync(() => _mediator.Send(query)))
                .GetAwaiter()
                .GetResult();

            var listSupplier = response
                .Select(x => new SupplierBrowseProjection
                { 
                    Id = x.SupplierId, 
                    SupplierName = x.SupplierName 
                });

            var form = new BrowserForm<SupplierBrowseProjection, string>(listSupplier, string.Empty, x => x.SupplierName);
            var dialogResult = form.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                return form.ReturnedValue;
            else
                return defaultValue;
        }
    }

    public class SupplierBrowseProjection
    {
        public string Id { get; set; }
        public string SupplierName { get; set; }
    }
}
