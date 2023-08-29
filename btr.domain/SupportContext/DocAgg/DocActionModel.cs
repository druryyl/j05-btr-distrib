using btr.domain.SupportContext.PrintManagerAgg;
using System;

namespace btr.domain.SupportContext.DocAgg
{
    public class DocActionModel : IDocKey
    {
        public string DocId { get; set; }
        public DateTime ActionDate { get; set; }
        public string Action { get; set; }
    }
}
