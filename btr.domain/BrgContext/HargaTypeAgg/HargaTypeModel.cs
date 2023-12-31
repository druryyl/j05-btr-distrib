﻿namespace btr.domain.BrgContext.HargaTypeAgg
{
    public class HargaTypeModel : IHargaTypeKey
    {
        public HargaTypeModel()
        {
        }

        public HargaTypeModel(string id) => HargaTypeId = id;

        public string HargaTypeId { get; set; }
        public string HargaTypeName { get; set; }
        public decimal Margin { get; set; }
    }
}