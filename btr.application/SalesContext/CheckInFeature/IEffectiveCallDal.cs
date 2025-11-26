using btr.nuna.Domain;
using btr.nuna.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SalesContext.CheckInFeature
{
    public interface IEffectiveCallDal :
        IListData<EffectiveCallView, Periode>
    {
    }

    public class EffectiveCallView
    {
        public string CheckInId { get; set; }
        public string CheckInDate { get; set; }
        public string UserEmail { get; set; }
        public string CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int OrderCount { get; set; }
        public bool IsEffectiveCall
        {
            get
            {
                return OrderCount > 0 ? true : false;
            }
        }
    }
}
