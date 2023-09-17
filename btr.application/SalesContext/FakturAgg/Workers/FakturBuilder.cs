using System;
using System.Collections.Generic;
using System.Linq;
using btr.application.BrgContext.BrgAgg;
using btr.application.InventoryContext.WarehouseAgg;
using btr.application.SalesContext.CustomerAgg.Contracts;
using btr.application.SalesContext.FakturAgg.Contracts;
using btr.application.SalesContext.SalesPersonAgg.Contracts;
using btr.application.SupportContext.TglJamAgg;
using btr.domain.BrgContext.BrgAgg;
using btr.domain.BrgContext.HargaTypeAgg;
using btr.domain.InventoryContext.WarehouseAgg;
using btr.domain.SalesContext.CustomerAgg;
using btr.domain.SalesContext.FakturAgg;
using btr.domain.SalesContext.SalesPersonAgg;
using btr.domain.SupportContext.UserAgg;
using btr.nuna.Application;
using btr.nuna.Domain;

namespace btr.application.SalesContext.FakturAgg.Workers
{
    public interface IFakturBuilder : INunaBuilder<FakturModel>
    {
        IFakturBuilder CreateNew(IUserKey userKey);
        IFakturBuilder Load(IFakturKey fakturKey);
        IFakturBuilder Attach(FakturModel faktur);
        IFakturBuilder FakturDate(DateTime fakturDate);
        IFakturBuilder Customer(ICustomerKey customerKey);
        IFakturBuilder SalesPerson(ISalesPersonKey salesPersonKey);
        IFakturBuilder Warehouse(IWarehouseKey warehouseKey);
        IFakturBuilder TglRencanaKirim(DateTime tglRencanaKirim);
        IFakturBuilder TermOfPayment(TermOfPaymentEnum termOfPayment);
        IFakturBuilder DueDate(DateTime dueDate);
        IFakturBuilder Void(IUserKey userKey);
        IFakturBuilder ReActivate(IUserKey userKey);

        IFakturBuilder User(IUserKey user);
        IFakturBuilder AddItem(IBrgKey brgKey, string stokHrgStr, string qtyString, string discountString, decimal ppnProsen);
        IFakturBuilder ClearItem();
        IFakturBuilder CalcTotal();
    }

    public class FakturBuilder : IFakturBuilder
    {
        private FakturModel _aggRoot = new FakturModel();
        private readonly IFakturDal _fakturDal;
        private readonly IFakturItemDal _fakturItemDal;
        private readonly IFakturQtyHargaDal _fakturQtyHargaDal;
        private readonly IFakturDiscountDal _fakturDiscountDal;

        private readonly ICustomerDal _customerDal;
        private readonly ISalesPersonDal _salesPersonDal;
        private readonly IWarehouseDal _warehouseDal;
        private readonly IBrgBuilder _brgBuilder;
        private readonly ITglJamDal _dateTime;

        public FakturBuilder(IFakturDal fakturDal,
            IFakturItemDal fakturItemDal,
            IFakturQtyHargaDal fakturQtyHargaDal,
            IFakturDiscountDal fakturDiscountDal,
            ICustomerDal customerDal,
            ISalesPersonDal salesPersonDal,
            IWarehouseDal warehouseDal,
            IBrgBuilder brgBuilder,
            ITglJamDal dateTime)
        {
            _fakturDal = fakturDal;
            _fakturItemDal = fakturItemDal;
            _fakturQtyHargaDal = fakturQtyHargaDal;
            _fakturDiscountDal = fakturDiscountDal;

            _customerDal = customerDal;
            _salesPersonDal = salesPersonDal;
            _warehouseDal = warehouseDal;
            _brgBuilder = brgBuilder;
            _dateTime = dateTime;
        }

        public FakturModel Build()
        {
            _aggRoot.RemoveNull();
            return _aggRoot;
        }

        public IFakturBuilder CreateNew(IUserKey userKey)
        {
            _aggRoot = new FakturModel
            {
                CreateTime = _dateTime.Now(),
                LastUpdate = new DateTime(3000, 1, 1),
                UserId = userKey.UserId,
                VoidDate = new DateTime(3000, 1, 1),
                UserIdVoid = userKey.UserId,
                ListItem = new List<FakturItemModel>(),


            };
            return this;
        }

        public IFakturBuilder Load(IFakturKey fakturKey)
        {
            _aggRoot = _fakturDal.GetData(fakturKey)
                       ?? throw new KeyNotFoundException($"Faktur not found ({fakturKey.FakturId})");
            _aggRoot.ListItem = _fakturItemDal.ListData(fakturKey)?.ToList()
                                ?? new List<FakturItemModel>();
            

            var allQtyHarga = _fakturQtyHargaDal.ListData(fakturKey)?.ToList()
                              ?? new List<FakturQtyHargaModel>();
            var allDiscount = _fakturDiscountDal.ListData(fakturKey)?.ToList()
                              ?? new List<FakturDiscountModel>();

            foreach (var item in _aggRoot.ListItem)
            {
                var brg = _brgBuilder.Load(item).Build();
                item.ListQtyHarga = Get3JenisQty(item, allQtyHarga.Where(x => x.FakturItemId == item.FakturItemId).ToList());
                item.ListDiscount = allDiscount.Where(x => x.FakturItemId == item.FakturItemId).ToList();
            }

            return this;
        }

        private List<FakturQtyHargaModel> Get3JenisQty(FakturItemModel item, List<FakturQtyHargaModel> existing)
        {
            var brg = _brgBuilder.Load(item).Build();
            //  qty besar
            var qtyBesarExisting = existing.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Besar);
            var qtyBesarDb = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1);
            var qtyBesar = qtyBesarExisting ?? 
                (qtyBesarDb is null ? null :
                    new FakturQtyHargaModel
                    {
                        BrgId = item.BrgId,
                        JenisQty = JenisQtyFakturEnum.Besar,
                        Satuan = qtyBesarDb.Satuan,
                        HargaSatuan = brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == _aggRoot.HargaTypeId)?.Harga ?? 0 * qtyBesarDb.Conversion,
                        Conversion = qtyBesarDb.Conversion,
                        Qty = 0,
                        SubTotal = 0
                    });
            //  qty kecil
            var qtyKecilExisting = existing.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Kecil);
            var qtyKecilDb = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1);
            var qtyKecil = qtyKecilExisting ??
                (qtyKecilDb is null ? null :
                    new FakturQtyHargaModel
                    {
                        BrgId = item.BrgId,
                        JenisQty = JenisQtyFakturEnum.Kecil,
                        Satuan = qtyKecilDb.Satuan,
                        HargaSatuan = brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == _aggRoot.HargaTypeId)?.Harga ?? 0,
                        Conversion = 1,
                        Qty = 0,
                        SubTotal = 0
                    });
            //  qty bonus
            var qtyBonusExisting = existing.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Bonus);
            var qtyBonus = qtyBonusExisting ??
                (qtyKecilDb is null ? null :
                    new FakturQtyHargaModel
                    {
                        BrgId = item.BrgId,
                        JenisQty = JenisQtyFakturEnum.Kecil,
                        Satuan = qtyKecilDb.Satuan,
                        HargaSatuan = brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == _aggRoot.HargaTypeId)?.Harga ?? 0,
                        Conversion = 1,
                        Qty = 0,
                        SubTotal = 0
                    });

            var result = new List<FakturQtyHargaModel>
            {
                qtyBesar, qtyKecil, qtyBonus
            };
            return result;
        }

        public IFakturBuilder Attach(FakturModel faktur)
        {
            _aggRoot = faktur;
            return this;
        }

        public IFakturBuilder FakturDate(DateTime fakturDate)
        {
            TimeSpan timespan = DateTime.Now - DateTime.Now.Date;
            _aggRoot.FakturDate = fakturDate
                .AddHours(timespan.Hours)
                .AddMinutes(timespan.Minutes)
                .AddSeconds(timespan.Seconds);
            return this;
        }

        public IFakturBuilder Customer(ICustomerKey customerKey)
        {
            var customer = _customerDal.GetData(customerKey)
                           ?? throw new KeyNotFoundException($"CustomerId not found ({customerKey.CustomerId})");
            _aggRoot.CustomerId = customer.CustomerId;
            _aggRoot.CustomerName = customer.CustomerName;
            _aggRoot.Plafond = customer.Plafond;
            _aggRoot.CreditBalance = customer.CreditBalance;
            _aggRoot.HargaTypeId = customer.HargaTypeId;
            return this;
        }

        public IFakturBuilder SalesPerson(ISalesPersonKey salesPersonKey)
        {
            var salesPerson = _salesPersonDal.GetData(salesPersonKey)
                              ?? throw new KeyNotFoundException(
                                  $"SalesPersonId not found ({salesPersonKey.SalesPersonId})");
            _aggRoot.SalesPersonId = salesPerson.SalesPersonId;
            _aggRoot.SalesPersonName = salesPerson.SalesPersonName;
            return this;
        }

        public IFakturBuilder Warehouse(IWarehouseKey warehouseKey)
        {
            var warehouse = _warehouseDal.GetData(warehouseKey)
                            ?? throw new KeyNotFoundException($"WarehouseId not found ({warehouseKey.WarehouseId})");
            _aggRoot.WarehouseId = warehouse.WarehouseId;
            _aggRoot.WarehouseName = warehouse.WarehouseName;
            return this;
        }

        public IFakturBuilder TglRencanaKirim(DateTime tglRencanaKirim)
        {
            _aggRoot.TglRencanaKirim = tglRencanaKirim;
            return this;
        }

        public IFakturBuilder AddItem(IBrgKey brgKey, string stokHrgStr, string qtyInputStr,
            string discInputStr, decimal ppnProsen)
        {
            var noUrutMax = _aggRoot.ListItem
                .DefaultIfEmpty(new FakturItemModel() { NoUrut = 0 })
                .Max(x => x.NoUrut);
            var noUrut = noUrutMax + 1;

            var brg = _brgBuilder.Load(brgKey).Build();
            var listQtyHarga = GenListStokHarga(brg, qtyInputStr, new HargaTypeModel(_aggRoot.HargaTypeId)).ToList();
            var newItem = new FakturItemModel
            {
                NoUrut = noUrut,
                BrgId = brgKey.BrgId,
                BrgName = brg.BrgName,
                BrgCode = brg.BrgCode,
                Conversion = brg.ListSatuan.Max(x => x.Conversion),
                StokHargaStr = stokHrgStr ?? string.Empty, 
                QtyInputStr = qtyInputStr ?? string.Empty,
                DiscInputStr = discInputStr ?? string.Empty,
                
            };
            var subTotal = listQtyHarga.Sum(x => x.SubTotal);
            var listDisc = GenListDiscount(brgKey.BrgId, subTotal, discInputStr).ToList();
            
            newItem.ListQtyHarga = listQtyHarga;
            newItem.QtyDetilStr = GenQtyDetilStr(listQtyHarga, brg);
            newItem.QtyPotStok = GenQtyPotStok(listQtyHarga, brg);
            newItem.QtyJual = GenQtyJual(listQtyHarga, brg);
            
            newItem.HargaSatuan = listQtyHarga.First(x => x.JenisQty == JenisQtyFakturEnum.Kecil).HargaSatuan;
            newItem.SubTotal = newItem.QtyJual * newItem.HargaSatuan;
            
            newItem.DiscDetilStr = GenDiscDetilStr(listDisc);
            newItem.DiscRp = listDisc.Sum(x => x.DiscRp);
            newItem.ListDiscount = listDisc;

            newItem.PpnProsen = ppnProsen;
            newItem.PpnRp = newItem.SubTotal * ppnProsen / 100;
            newItem.Total = newItem.SubTotal - newItem.DiscRp + newItem.PpnRp;

            newItem.ListQtyHarga.RemoveAll(x => x.Qty == 0);
            _aggRoot.ListItem.Add(newItem);
            return this;
        }

        private static string GenDiscDetilStr(IReadOnlyCollection<FakturDiscountModel> listDisc)
        {
            var listString = new List<string>
            {
                $"{listDisc.FirstOrDefault(x => x.NoUrut == 1)?.DiscRp:N0 ?? string.Empty}",
                $"{listDisc.FirstOrDefault(x => x.NoUrut == 2)?.DiscRp:N0 ?? string.Empty}",
                $"{listDisc.FirstOrDefault(x => x.NoUrut == 3)?.DiscRp:N0 ?? string.Empty}",
                $"{listDisc.FirstOrDefault(x => x.NoUrut == 4)?.DiscRp:N0 ?? string.Empty}"
            };
            var result = string.Join(Environment.NewLine, listString);
            return result;
        }
        
        private static int GenQtyPotStok(IReadOnlyCollection<FakturQtyHargaModel> listQty, BrgModel brg)
        {
            var qtyBesar = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Besar)?.Qty ?? 0;
            var qtyKecil = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Kecil)?.Qty ?? 0;
            var qtyBonus = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Bonus)?.Qty ?? 0;

            var conversion = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Conversion ?? 1;

            var result = (qtyBesar * conversion) + qtyKecil + qtyBonus;
            return result;
        }
        private static int GenQtyJual(IReadOnlyCollection<FakturQtyHargaModel> listQty, BrgModel brg)
        {
            var qtyBesar = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Besar)?.Qty ?? 0;
            var qtyKecil = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Kecil)?.Qty ?? 0;
            
            var conversion = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1)?.Conversion ?? 1;
            
            var result = (qtyBesar * conversion) + qtyKecil;
            return result;
        }

        private static string GenQtyDetilStr(IReadOnlyCollection<FakturQtyHargaModel> listQty, BrgModel brg)
        {
            string result = string.Empty;

            //  qty besar
            var qtyBesar = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Besar);
            var satBesar = brg.ListSatuan.FirstOrDefault(x => x.Conversion > 1);
            if (qtyBesar != null)
                result += $"{qtyBesar.Qty} {qtyBesar.Satuan}";
            else
                result += satBesar is null ? string.Empty : $"0 {satBesar.Satuan}";

            //  qty kecil
            var qtyKecil = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Kecil);
            var satKecil = brg.ListSatuan.FirstOrDefault(x => x.Conversion == 1);
            if (qtyKecil != null)
                result += $"{Environment.NewLine}{qtyKecil.Qty} {qtyKecil.Satuan}";
            else
                result += satKecil is null ? string.Empty : $"{Environment.NewLine}0 {satKecil.Satuan}";

            //  qty bonus
            var qtyBonus = listQty.FirstOrDefault(x => x.JenisQty == JenisQtyFakturEnum.Bonus);
            if (qtyBonus != null)
                result += $"{Environment.NewLine}{qtyBonus.Qty} {qtyBonus.Satuan}";
            else
                result += satKecil is null ? string.Empty : $"{Environment.NewLine}0 {satKecil.Satuan}";

            return result;
        }
        
        private static IEnumerable<FakturQtyHargaModel> GenListStokHarga(BrgModel brg, string qtyString, IHargaTypeKey hargaTypeId)
        {
            var result = new List<FakturQtyHargaModel>();
            var qtys = ParseStringMultiNumber(qtyString, 3);
            var satuanBesar = brg.ListSatuan.OrderBy(x => x.Conversion).Last();
            var satuanKecil = brg.ListSatuan.OrderBy(x => x.Conversion).First();

            var hrg = brg.ListHarga.FirstOrDefault(x => x.HargaTypeId == hargaTypeId.HargaTypeId)?.Harga ?? 0;
            var hrgBesar = hrg * satuanBesar.Conversion;
            var hrgKecil = hrg;

            result.Add(new FakturQtyHargaModel(JenisQtyFakturEnum.Besar, brg.BrgId, satuanBesar.Satuan, hrgBesar,
                satuanBesar.Conversion, (int)qtys[0], hrgBesar * qtys[0]));
            result.Add(new FakturQtyHargaModel(JenisQtyFakturEnum.Kecil, brg.BrgId, satuanKecil.Satuan, hrgKecil,
                satuanKecil.Conversion, (int)qtys[1], hrgKecil * qtys[1]));
            result.Add(new FakturQtyHargaModel(JenisQtyFakturEnum.Bonus, brg.BrgId, satuanKecil.Satuan, 0,
                satuanKecil.Conversion, (int)qtys[2], 0));
            //result.RemoveAll(x => x.Qty == 0);

            return result;
        }

        private static IEnumerable<FakturDiscountModel> GenListDiscount(string brgId, decimal subTotal,
            string disccountString)
        {
            var discs = ParseStringMultiNumber(disccountString, 4);

            var discRp = new decimal[4];
            discRp[0] = subTotal * discs[0] / 100;
            var newSubTotal = subTotal - discRp[0];
            discRp[1] = newSubTotal * discs[1] / 100;
            newSubTotal -= discRp[1];
            discRp[2] = newSubTotal * discs[2] / 100;
            newSubTotal -= discRp[2];
            discRp[3] = newSubTotal * discs[3] / 100;

            var result = new List<FakturDiscountModel>
            {
                new FakturDiscountModel(1, brgId, discs[0], discRp[0]),
                new FakturDiscountModel(2, brgId, discs[1], discRp[1]),
                new FakturDiscountModel(3, brgId, discs[2], discRp[2]),
                new FakturDiscountModel(4, brgId, discs[3], discRp[3])
            };
            result.RemoveAll(x => x.DiscProsen == 0);
            return result;
        }

        private static List<decimal> ParseStringMultiNumber(string str, int size)
        {
            if (str is null)
                str = string.Empty;

            var result = new List<decimal>();
            for (var i = 0; i < size; i++)
                result.Add(0);

            var resultStr = (str == string.Empty ? "0" : str).Split(';').ToList();

            var x = 0;
            foreach (var item in resultStr.TakeWhile(item => x < result.Count))
            {
                if (decimal.TryParse(item, out var temp))
                    result[x] = temp;
                x++;
            }

            return result;
        }

        public IFakturBuilder CalcTotal()
        {
            _aggRoot.Total = _aggRoot.ListItem.Sum(x => x.SubTotal);
            _aggRoot.Discount = _aggRoot.ListItem.Sum(x => x.DiscRp);
            _aggRoot.Tax = _aggRoot.ListItem.Sum(x => x.PpnRp);
            _aggRoot.GrandTotal = _aggRoot.ListItem.Sum(x => x.Total);
            _aggRoot.KurangBayar = _aggRoot.GrandTotal - _aggRoot.UangMuka;
            return this;
        }

        public IFakturBuilder ClearItem()
        {
            _aggRoot.ListItem.Clear();
            return this;
        }

        public IFakturBuilder User(IUserKey user)
        {
            _aggRoot.UserId = user.UserId;
            return this;
        }

        public IFakturBuilder TermOfPayment(TermOfPaymentEnum termOfPayment)
        {
            _aggRoot.TermOfPayment = termOfPayment;
            return this;
        }

        public IFakturBuilder DueDate(DateTime dueDate)
        {
            _aggRoot.DueDate = dueDate;
            return this;
        }

        public IFakturBuilder Void(IUserKey userKey)
        {
            _aggRoot.VoidDate = _dateTime.Now();
            _aggRoot.UserIdVoid = userKey.UserId;
            return this;
        }

        public IFakturBuilder ReActivate(IUserKey userKey)
        {
            _aggRoot.VoidDate = new DateTime(3000,1,1);
            _aggRoot.UserIdVoid = string.Empty;
            return this;
        }
    }
}