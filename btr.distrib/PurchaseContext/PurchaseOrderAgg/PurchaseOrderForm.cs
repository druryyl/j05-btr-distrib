using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btr.distrib.PurchaseContext.PurchaseOrderAgg
{
    public partial class PurchaseOrderForm : Form
    {
        private readonly IMediator _mediator;

        public PurchaseOrderForm(IMediator mediator)
        {
            InitializeComponent();
            _mediator = mediator;

            SupplierIdText.Validated += SupplierIdText_Validated;
        }

        #region SUPPLIER
        private void SupplierIdText_Validated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void ValidateSupplier(string supplierId)
        {
            var query = new GetSupplierQ
        }

        #endregion
    }
}
