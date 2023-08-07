CREATE TABLE BTR_FakturQtyHarga
(
    FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_FakturId DEFAULT(''),
    FakturItemId VARCHAR(16) NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_FakturItemId DEFAULT(''),
    FakturQtyHargaId VARCHAR(18) NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_FakturQtyHargaId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_QtyHargaNo DEFAULT(0),

    BrgId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_BrgId DEFAULT(''),
    Satuan VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_Satuan DEFAULT(''),
    Conversion INT NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_Conversion DEFAULT(0),
    Qty INT NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_Qty DEFAULT(0),
    HargaJual DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturQtyHarga_HargaJual DEFAULT(0),
    
    CONSTRAINT PK_BTR_FakturQtyHarga PRIMARY KEY CLUSTERED(FakturQtyHargaId) 
)
GO

CREATE INDEX IX_BTR_FakturQtyHarga_FakturId
    ON BTR_FakturQtyHarga (FakturId, FakturQtyHargaId)
GO
