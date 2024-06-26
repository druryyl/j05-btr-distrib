﻿CREATE TABLE BTR_StokMutasi(
    StokId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_StokMutasi_StokId DEFAULT(''),
    StokMutasiId VARCHAR(18) NOT NULL CONSTRAINT DF_BTR_StokMutasi_StokMutasiId DEFAULT(''),
    BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_StokMutasi_BrgId  DEFAULT(''),
    WarehouseId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_StokMutasi_WarehouseId DEFAULT(''),

    ReffId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_StokMutasi_ReffId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_StokMutasi_NoUrut DEFAULT(0),
    JenisMutasi VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_StokMutasi_JenisMutasi DEFAULT(''),
    MutasiDate DATETIME NOT NULL CONSTRAINT DF_BTR_StokMutasi_MutasiDate DEFAULT('3000-01-01'),
    PencatatanDate DATETIME NOT NULL CONSTRAINT DF_BTR_StokMutasi_PencatatanDate DEFAULT('3000-01-01'),

    QtyIn INT NOT NULL CONSTRAINT DF_BTR_StokMutasi_QtyIn DEFAULT(0),
    QtyOut INT NOT NULL CONSTRAINT DF_BTR_StokMutasi_QtyOut DEFAULT(0),
    HargaJual DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_StokMutasi_HargaJual DEFAULT(0),
    Keterangan VARCHAR(128) NOT NULL CONSTRAINT DF_BTR_StokMutasi_Keterangan DEFAULT(''),
    
    CONSTRAINT PK_BTR_StokMutasi PRIMARY KEY CLUSTERED (StokMutasiId)
)
GO

CREATE INDEX IX_BTR_StokMutasi_StokId
    ON BTR_StokMutasi (StokId, StokMutasiId)
    WITH(FILLFACTOR=95)                                   
GO


CREATE INDEX IX_BTR_StokMutasi_ReffId
    ON BTR_StokMutasi (ReffId, StokMutasiId)
    WITH(FILLFACTOR=95)
GO
