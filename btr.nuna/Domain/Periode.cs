using System;
using System.Collections.Generic;
using System.Linq;

namespace btr.nuna.Domain
{
    public class Periode
    {
        public Periode(DateTime tgl)
        {
            Tgl1 = tgl.Date;
            Tgl2 = tgl.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
        public Periode(DateTime tgl1, DateTime tgl2)
        {
            Tgl1 = tgl1.Date;
            Tgl2 = tgl2.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }


        public DateTime Tgl1 { get; }
        public DateTime Tgl2 { get; }
    }
}