using btr.domain.SupportContext.DocAgg;
using System;
using System.Collections.Generic;

namespace btr.domain.SupportContext.PrintManagerAgg
{

    public class DocModel : IDocKey
    {
        public DocModel()
        {
        }
        public DocModel(string id) => DocId = id;

        public string DocId { get; set; }
        public string DocType { get; set; }
        public string DocDesc { get; set; }
        public DateTime DocDate { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public DocPrintStatusEnum DocPrintStatus { get; set; }
        public List<DocActionModel> ListAction { get; set; }
    }
}
