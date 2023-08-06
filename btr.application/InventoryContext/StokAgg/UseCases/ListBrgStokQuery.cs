using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using btr.application.InventoryContext.BrgAgg.Contracts;
using btr.application.InventoryContext.StokAgg.Contracts;
using btr.domain.InventoryContext.BrgAgg;
using btr.domain.InventoryContext.StokAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using Dawn;
using MediatR;

namespace btr.application.InventoryContext.StokAgg.UseCases
{
    public class ListBrgStokQuery : IRequest<IEnumerable<ListBrgStokResponse>>, IWarehouseKey
    {
        public ListBrgStokQuery(string brgName, string warehouseId)
        {
            BrgName = brgName;
            WarehouseId = warehouseId;
        }
        public string BrgName { get; }
        public string WarehouseId { get; }
    }

public class ListBrgStokResponse
{
    public string BrgId { get; set; }
    public string BrgName { get; set; }
    public int Qty { get; set; }
}

public class ListBrgStokHandler : IRequestHandler<ListBrgStokQuery, IEnumerable<ListBrgStokResponse>>
{
    private readonly IStokDal _stokDal;
    private readonly IBrgDal _brgDal;

    public ListBrgStokHandler(IStokDal stokDal, 
        IBrgDal brgDal)
    {
        _stokDal = stokDal;
        _brgDal = brgDal;
    }

    public Task<IEnumerable<ListBrgStokResponse>> Handle(ListBrgStokQuery request, CancellationToken cancellationToken)
    {
        //  GUARD
        Guard.Argument(() => request).NotNull()
            .Member(x => x.BrgName, y => y.NotEmpty())
            .Member(x => x.WarehouseId, y => y.NotEmpty());
        
        //  QUERY
        //      ambil list BrgId atas BrgName yg dipassingkan
        var warehouseKey = new WarehouseModel(request.WarehouseId);
        var listBrgKey = ListBrgKey(request.BrgName);
        var listStok = ListStok(listBrgKey, warehouseKey);
        
        //  RESPONSES
        var result =
            from c in listStok
            group c by new { c.BrgId, c.BrgName }
            into g
            select new ListBrgStokResponse
            {
                BrgId = g.Key.BrgId,
                BrgName = g.Key.BrgName,
                Qty = g.Sum(x => x.Qty)
            };
        return Task.FromResult(result);
    }

    private IEnumerable<IBrgKey> ListBrgKey(string keyword)
    {
        var allBrg = _brgDal.ListData()?.ToList()
            ?? throw new KeyNotFoundException("Brg not found");
        var keywords = keyword.ToLower().Split(' ');
        var result = allBrg
            .Where(x => keywords.All(word => x.BrgName.ToLower().Contains(word)))
            .Select(x => new BrgModel(x.BrgId))
            .ToList();
        return result;
    }

    private IEnumerable<StokModel> ListStok(IEnumerable<IBrgKey> listBrg, IWarehouseKey warehouse)
    {
        var result = new List<StokModel>();
        foreach (var item in listBrg.ToList())
        {
            result.AddRange(_stokDal.ListData(item, warehouse));
        }
        return result;
    }

}
}
