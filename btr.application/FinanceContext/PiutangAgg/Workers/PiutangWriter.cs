using btr.application.FinanceContext.PiutangAgg.Contracts;
using btr.domain.FinanceContext.PiutangAgg;
using btr.nuna.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.FinanceContext.PiutangAgg.Workers
{
    public interface IPiutangWriter : INunaWriter<PiutangModel>
    {
        void Delete(IPiutangKey piutangKey);
    }
    public class PiutangWriter : IPiutangWriter
    {
        private readonly IPiutangDal _piutangDal;
        private readonly IPiutangElementDal _piutangElementDal;
        private readonly IPiutangLunasDal _piutangLunasDal;

        public PiutangWriter(IPiutangDal piutangDal, 
            IPiutangElementDal piutangElementDal, 
            IPiutangLunasDal piutangLunasDal)
        {
            _piutangDal = piutangDal;
            _piutangElementDal = piutangElementDal;
            _piutangLunasDal = piutangLunasDal;
        }

        public void Save(ref PiutangModel model)
        {
            if (model.PiutangId.Length == 0)
                throw new ArgumentException("PiutangId invalid");

            foreach(var item in model.ListElement)
                item.PiutangId = model.PiutangId;
            foreach (var item in model.ListLunas)
                item.PiutangId = model.PiutangId;

            var db = _piutangDal.GetData(model);

            using (var trans = TransHelper.NewScope())
            {
                if (db is null)
                    _piutangDal.Insert(model);
                else
                    _piutangDal.Update(model);

                _piutangElementDal.Delete(model);
                _piutangElementDal.Insert(model.ListElement);
                _piutangLunasDal.Delete(model);
                _piutangLunasDal.Insert(model.ListLunas);
                trans.Complete();
            }
        }

        public void Delete(IPiutangKey piutangKey)
        {
            using (var trans = TransHelper.NewScope())
            {
                _piutangDal.Delete(piutangKey);
                _piutangElementDal.Delete(piutangKey);
                _piutangLunasDal.Delete(piutangKey);
                trans.Complete();
            }
        }
    }
}
