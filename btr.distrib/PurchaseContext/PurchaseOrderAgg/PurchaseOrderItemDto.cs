using btr.domain.BrgContext.BrgAgg;
using System;

namespace btr.distrib.PurchaseContext.PurchaseOrderAgg
{
    public class PurchaseOrderItemDto : IBrgKey
    {
        private int qty;
        private decimal harga;
        private decimal disc;
        private decimal tax;
        
        public event EventHandler Calculated;

        public string BrgId { get; set; }
        public string BrgName { get; private set; }
        public int Qty 
        { 
            get => qty; 
            set 
            { 
                qty = value; 
                Calculate(); 
            }
        }
        public string Satuan { get; private set; }
        public decimal Harga 
        { 
            get => harga;
            set 
            {
                harga = value; 
                Calculate();
            }
        }
        public decimal SubTotal { get; private set; }
        public decimal Disc
        {
            get => disc; set
            {
                disc = value;
                Calculate();
            }
        }
        public decimal DiscRp { get; private set; }
        public decimal Tax
        {
            get => tax; set
            {
                tax = value;
                Calculate();
            }
        }
        public decimal TaxRp { get; private set; }
        public decimal Total { get; private set; }

        public void SetBrgName(string name) => BrgName = name;
        public void SetSatuan(string satuan) => Satuan = satuan;

        private void Calculate()
        {
            SubTotal = Qty * Harga;
            DiscRp = SubTotal * Disc / 100;
            TaxRp = SubTotal * Tax / 100;
            Total = SubTotal - DiscRp + TaxRp;
        }
        protected virtual void OnCalculated()
        {
            Calculated?.Invoke(this, EventArgs.Empty);
        }
    }
}
