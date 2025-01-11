CREATE TABLE [dbo].[BTR_FpKeluaranFaktur]
(
    FpKeluaranFakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_FpKeluaranFakturId  DEFAULT(''),
    FpKeluaranId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_FpKeluaranId  DEFAULT(''),
    FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_FakturId  DEFAULT(''),
    Baris VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_Baris  DEFAULT(''),
    
    TanggalFaktur VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_TanggalFaktur  DEFAULT(''),
    JenisFaktur VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_JenisFaktur  DEFAULT(''),
    KodeTransaksi VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_KodeTransaksi  DEFAULT(''),
    
    KeteranganTambahan VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_KeteranganTambahan  DEFAULT(''),
    DokumenPendukung VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_DokumenPendukung  DEFAULT(''),
    Referensi VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_Referensi  DEFAULT(''),
    CapFasilitas VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_CapFasilitas  DEFAULT(''),
    IdTkuPenjual VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_IdTkuPenjual  DEFAULT(''),
    
    NpwpNikPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NpwpNikPembeli  DEFAULT(''),
    JenisIdPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_JenisIdPembeli  DEFAULT(''),
    NegaraPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NegaraPembeli  DEFAULT(''),
    NomorDokumenPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NomorDokumenPembeli  DEFAULT(''),
    
    NamaPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NamaPembeli  DEFAULT(''),
    AlamatPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_AlamatPembeli  DEFAULT(''),
    EmailPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_EmailPembeli  DEFAULT(''),
    IdTkuPembeli VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_IdTkuPembeli  DEFAULT(''),

    CONSTRAINT PK_BTR_FpKeluaranFaktur PRIMARY KEY (FpKeluaranFakturId),
)
GO

CREATE INDEX IX_BTR_FpKeluaranFaktur_FpKeluaranId ON [dbo].[BTR_FpKeluaranFaktur] (FpKeluaranId, FpKeluaranFakturId)
GO

