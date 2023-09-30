CREATE TABLE BTR_MutasiItem(
    MutasiId VARCHAR(13) NOT NULL BTR_MutasiItem_MutasiId DEFAULT(''),
    MutasiItemId VARCHAR(16) NOT NULL BTR_MutasiItem_MutasiItemId DEFAULT(''),
    NoUrut INT NOT NULL BTR_MutasiItem_NoUrut DEFAULT(0),
    BrgId VARCHAR(6) NOT NULL BTR_MutasiItem_BrgId DEFAULT(''),
    
    QtyInputStr VARCHAR(50) NOT NULL BTR_MutasiItem_QtyInputStr DEFAULT(''),
    QtyBesar INT NOT NULL BTR_MutasiItem_QtyBesar DEFAULT(0),
    SatBesar VARCHAR(10) NOT NULL BTR_MutasiItem_SatBesar DEFAULT(''),
    Conversion INT NOT NULL BTR_MutasiItem_Conversion DEFAULT(0),
    HppBesar DECIMAL(18,2) NOT NULL BTR_MutasiItem_HppBesar DEFAULT(0),
    
    QtyKecil INT NOT NULL BTR_MutasiItem_QtyKecil DEFAULT(0),
    SatKecil VARCHAR(13) NOT NULL BTR_MutasiItem_SatKecil DEFAULT(''),
    HppKecil DECIMAL(18,2) NOT NULL BTR_MutasiItem_HppKecil DEFAULT(0),
    
    StokBesar INT NOT NULL BTR_MutasiItem_StokBesar DEFAULT(0),
    StokKecil INT NOT NULL BTR_MutasiItem_StokKecil DEFAULT(0),
    
    Qty INT NOT NULL BTR_MutasiItem_Qty DEFAULT(0),
    Sat VARCHAR(10) NOT NULL BTR_MutasiItem_Sat DEFAULT(''),
    Hpp DECIMAL(18,2) NOT NULL BTR_MutasiItem_Hpp DEFAULT(0),
    
    QtyDetilStr VARCHAR(64) NOT NULL BTR_MutasiItem_QtyDetilStr DEFAULT(''),
    StokDetilStr VARCHAR(64) NOT NULL BTR_MutasiItem_StokDetilStr DEFAULT(''),
    HppDetilStr VARCHAR(64) NOT NULL BTR_MutasiItem_HppDetilStr DEFAULT(''),
    NilaiSediaan DECIMAL(18,2) NOT NULL BTR_MutasiItem_NilaiSediaan DEFAULT(0),
    
    CONSTRAINT PK_BTR_MutasiItem PRIMARY KEY CLUSTERED(MutasiItemId),
)
GO

CREATE INDEX IX_BTR_MutasiItem_MutasiId
    ON BTR_MutasiItem(MutasiId, MutasiItemId)
    WITH(FILLFACTOR=95)
GO