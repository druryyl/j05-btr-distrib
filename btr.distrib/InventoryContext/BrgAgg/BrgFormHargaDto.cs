using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormHargaDto
    {
        private decimal harga;

        public string TypeId { get; set; }
        public string Name { get; private set; }
        public double Margin { get; private set; }
        public decimal Harga
        {
            get => harga;
            set
            {
                harga = value;
                CalcMargin();
            }
        }
        public string Keterangan { get; private set; }
        public decimal Hpp { get; private set; }


        public void SetName(string name) => Name = name;
        public void SetKeterangan(string keterangan) => Keterangan = keterangan;
        public void SetHpp(decimal hpp)
        {
            Hpp = hpp;
            CalcMargin();
        } 

        public void CalcMargin()
        {
            var selisih = Harga - Hpp;
            Margin = Hpp != 0 ? (double)(selisih / Hpp * 100) : 0;
        }

        public void Clear()
        {
            Harga = 0;
            Keterangan = string.Empty;
        }
    }
}
