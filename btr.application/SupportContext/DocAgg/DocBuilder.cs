using btr.application.InventoryContext.WarehouseAgg.Contracts;
using btr.application.SupportContext.PrintManagerAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SupportContext.DocAgg;
using btr.domain.SupportContext.PrintManagerAgg;
using btr.nuna.Application;
using btr.nuna.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.application.SupportContext.DocAgg
{
    public interface IDocBuilder : INunaBuilder<DocModel>
    {
        IDocBuilder LoadOrCreate(IDocKey docKey);
        IDocBuilder Attach(DocModel doc);

        IDocBuilder Warehouse(IWarehouseKey warehouseKey);
        IDocBuilder Cancel();
        IDocBuilder Print();
        IDocBuilder Queue();
    }

    public class DocBuilder : IDocBuilder
    {
        private DocModel _agg;
        private readonly IDocDal _docDal;
        private readonly IDocActionDal _docActionDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly DateTimeProvider _dateTime;

        public DocBuilder(IDocDal docDal,
            IDocActionDal docActionDal,
            IWarehouseDal warehouseDal,
            DateTimeProvider dateTime)
        {
            _docDal = docDal;
            _docActionDal = docActionDal;
            _warehouseDal = warehouseDal;
            _dateTime = dateTime;
        }


        public DocModel Build()
        {
            _agg.RemoveNull();
            return _agg; ;
        }

        public IDocBuilder LoadOrCreate(IDocKey docKey)
        {
            var db = _docDal.GetData(docKey);
            if (db == null)
                Create_(docKey);
            else
                Load_(db);
            return this;
        }

        private void Create_(IDocKey docKey)
        {
            _agg = new DocModel
            {
                DocId = docKey.DocId,
                DocDate = _dateTime.Now,
                DocPrintStatus = DocPrintStatusEnum.Queued,
            };
            var prefix = docKey.DocId.Substring(0, 4);

            switch (prefix)
            {
                case "FKTR":
                    _agg.DocType = "FAKTUR";
                    break;
                default:
                    _agg.DocType = "UNKNOWN";
                    break;
            }
            _agg.ListAction = new List<DocActionModel>();
            return;
        }

        private void Load_(DocModel doc)
        {
            _agg = doc;
            _agg.ListAction = _docActionDal.ListData(doc)?.ToList()
                ?? new List<DocActionModel>();
        }

        public IDocBuilder Attach(DocModel doc)
        {
            _agg = doc;
            return this;
        }

        public IDocBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                ?? throw new KeyNotFoundException("WarehousId invalid");
            _agg.WarehouseId = warehouse.WarehouseId;
            _agg.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IDocBuilder Cancel()
        {
            var newAction = new DocActionModel
            {
                Action = "Cancel",
                ActionDate = _dateTime.Now,
            };
            _agg.ListAction.Add(newAction);
            _agg.DocPrintStatus = DocPrintStatusEnum.Canceled;
            return this;
        }

        public IDocBuilder Print()
        {
            var newAction = new DocActionModel
            {
                Action = "Print",
                ActionDate = _dateTime.Now,
            };
            _agg.ListAction.Add(newAction);
            _agg.DocPrintStatus = DocPrintStatusEnum.Printed;
            return this;
        }
        public IDocBuilder Queue()
        {
            var newAction = new DocActionModel
            {
                Action = "Queue",
                ActionDate = _dateTime.Now,
            };
            _agg.ListAction.Add(newAction);
            _agg.DocPrintStatus = DocPrintStatusEnum.Queued;
            return this;
        }
    }
}