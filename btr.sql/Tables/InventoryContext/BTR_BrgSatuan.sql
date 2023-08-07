CREATE TABLE BTR_BrgSatuanHarga(
    BrgId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_BrgSatuanHarga_BrgId DEFAULT(''), 
    Satuan VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_BrgSatuanHarga_Satuan DEFAULT(''),
    Conversion INT NOT NULL CONSTRAINT DF_BTR_BrgSatuanHarga_Conversion DEFAULT(0),
    HargaJual DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_BrgSatuanHarga_HargaJual DEFAULT(0),
    
    CONSTRAINT PK_BrgSatuan PRIMARY KEY CLUSTERED (BrgId, Satuan)
)