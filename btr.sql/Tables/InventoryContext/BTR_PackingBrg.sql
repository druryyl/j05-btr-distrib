CREATE TABLE [dbo].[BTR_PackingBrg]
(
        PackingId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PackingBrg_PackingId DEFAULT(''),
        FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PackingBrg_FakturId DEFAULT(''),
        SupplierId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_PackingBrg_SupplierId DEFAULT(''),
        BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_PackingBrg_BrgId DEFAULT(''),
        QtyBesar INT NOT NULL CONSTRAINT DF_BTR_PackingBrg_QtyBesar DEFAULT(0),
        SatBesar VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_PackingBrg_SatBesar DEFAULT(''),
        QtyKecil INT NOT NULL CONSTRAINT DF_BTR_PackingBrg_QtyKecil DEFAULT(0),
        SatKecil VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_PackingBrg_SatKecil DEFAULT(''),
        HargaJual DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PackingBrg_HargaJual DEFAULT(0),

        CONSTRAINT PK_BTR_PackingBrg PRIMARY KEY CLUSTERED(PackingId, SupplierId, BrgId, FakturId)
)
