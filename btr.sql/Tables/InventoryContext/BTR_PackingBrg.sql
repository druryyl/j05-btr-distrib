CREATE TABLE [dbo].[BTR_PackingFakturBrg]
(
        PackingId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_PackingId DEFAULT(''),
        FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_PackingId DEFAULT(''),
        SupplierId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_SupplierId DEFAULT(''),
        BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_BrgId DEFAULT(''),
        QtyBesar INT NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_QtyBesar DEFAULT(0),
        SatuanBesar VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_SatuanBesar DEFAULT(''),
        QtyKecil INT NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_QtyKecil DEFAULT(0),
        SatuanKecil VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_SatuanKecil DEFAULT(''),
        HargaJual DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PackingFakturBrg_HargaJual DEFAULT(0),

        CONSTRAINT PK_BTR_PackingBrg PRIMARY KEY CLUSTERED(PackingId, SupplierId, BrgId)
)
