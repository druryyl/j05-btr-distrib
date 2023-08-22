using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.InventoryContext.BrgAgg
{
    public class BrgFormHargaDto
    {
        public string TypeId { get; set; }
        public string Name { get; private set; }
        public decimal Hpp { get; private set; }
        public double Margin { get; set; }
        public decimal Harga { get; set; }
        public string Keterangan { get; private set; }


        public void SetName(string name) => Name = name;
        public void SetKeterangan(string keterangan) => Keterangan = keterangan;

    }
}
