using btr.application.FinanceContext.TagihanAgg;
using btr.domain.FinanceContext.TagihanAgg;
using btr.infrastructure.Helpers;
using btr.nuna.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.infrastructure.FinanceContext.TagihanAgg
{
    public class TagihanDal : ITagihanDal
    {
        private readonly DatabaseOptions _opt;

        public TagihanDal(IOptions<DatabaseOptions> opt)
        {
            _opt = opt.Value;
        }

        public void Insert(TagihanModel model)
        {
            //  query insert table BTR_Tagihan from model TagihanModel

        }

        public void Update(TagihanModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(TagihanModel key)
        {
            throw new NotImplementedException();
        }

        public TagihanModel GetData(ITagihanKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TagihanModel> ListData(Periode filter)
        {
            throw new NotImplementedException();
        }
    }
}
