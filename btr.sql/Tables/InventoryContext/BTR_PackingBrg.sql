CREATE TABLE [dbo].[BTR_PackingBrg]
(
        PackingId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PackingBrg_PackingId DEFAULT(''),
        SupplierId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_PackingBrg_SupplierId DEFAULT(''),
        NoUrut INT NOT NULL CONSTRAINT DF_BTR_PackingBrg_NoUrut DEFAULT(0),
        BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_PackingBrg_BrgId DEFAULT(''),
        QtyKecil INT NOT NULL CONSTRAINT DF_BTR_PackingBrg_QtyKecil DEFAULT(0),
        SatuanKecil VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_PackingBrg_SatuanKecil DEFAULT(''),
        QtyBesar INT NOT NULL CONSTRAINT DF_BTR_PackingBrg_QtyBesar DEFAULT(0),
        SatuanBesar VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_PackingBrg_SatuanBesar DEFAULT(''),
        HargaJual DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PackingBrg_HargaJual DEFAULT(0),

        CONSTRAINT PK_BTR_PackingBrg PRIMARY KEY CLUSTERED(PackingId, SupplierId, BrgId)
)
