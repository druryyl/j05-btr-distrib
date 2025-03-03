CREATE  TABLE BTR_FakturItemKlaim(
    FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_FakturId DEFAULT(''),
    FakturItemId VARCHAR(16) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_FakturItemId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_ItemNo DEFAULT(''),
    
    BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_BrgId DEFAULT(''),
    BrgCode VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_BrgCode DEFAULT(''),
    StokHargaStr VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_StokHargaStr DEFAULT(''),
    QtyInputStr VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_QtyInputStr DEFAULT(''),
    QtyDetilStr VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_QtyDetilStr DEFAULT(''),
    HrgInputStr VARCHAR(30) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_HrgInputStr DEFAULT(''),

    QtyBesar INT NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_QtyBesar DEFAULT(0),
    SatBesar VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_SatBesar DEFAULT(''),
    Conversion INT NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_Conversion DEFAULT(1),
    HrgSatBesar DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_HargaSatBesar DEFAULT(0),

    QtyKecil INT NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_QtyKecil DEFAULT(0),
    SatKecil VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_SatKecil DEFAULT(''),
    HrgSatKecil DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_HrgSatKecil DEFAULT(''),

    QtyJual INT NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_QtyJual DEFAULT(0),
    HrgSat DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_HargaSatuan DEFAULT(0),
    SubTotal DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_SubTotal DEFAULT(0),
    
    QtyBonus INT NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_QtyBonus DEFAULT(0),
    QtyPotStok INT NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_QtyPotStok DEFAULT(0),
    
    DiscInputStr VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_DiscInputStr DEFAULT(''),
    DiscDetilStr VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_DiscDetilStr DEFAULT(''),
    DiscRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_DiscRp DEFAULT(0),
    
    DppProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_DppProsen DEFAULT(0),
    DppRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_DppRp DEFAULT(0),
    PpnProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_PpnProsen DEFAULT(0),
    PpnRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_PpnRp DEFAULT(0),
    Total DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturItemKlaim_Total DEFAULT(0),

    CONSTRAINT PK_BTR_FakturItemKlaim PRIMARY KEY CLUSTERED(FakturItemId)
)
GO

CREATE INDEX IX_BTR_FakturItemKlaim_FakturId
    ON BTR_FakturItemKlaim (FakturId, FakturItemId)
GO

