CREATE TABLE [dbo].[BTR_FpKeluaranFaktur]
(
    FpKeluaranFakturId VARCHAR(17) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_FpKeluaranFakturId  DEFAULT(''),
    FpKeluaranId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_FpKeluaranId  DEFAULT(''),
    FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_FakturId  DEFAULT(''),
    Baris INT NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_Baris  DEFAULT(0),
    
    TanggalFaktur DATETIME NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_TanggalFaktur  DEFAULT('3000-01-01'),
    JenisFaktur VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_JenisFaktur  DEFAULT(''),
    KodeTransaksi VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_KodeTransaksi  DEFAULT(''),
    
    KeteranganTambahan VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_KeteranganTambahan  DEFAULT(''),
    DokumenPendukung VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_DokumenPendukung  DEFAULT(''),
    Referensi VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_Referensi  DEFAULT(''),
    CapFasilitas VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_CapFasilitas  DEFAULT(''),
    IdTkuPenjual VARCHAR(22) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_IdTkuPenjual  DEFAULT(''),
    
    NpwpNikPembeli VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NpwpNikPembeli  DEFAULT(''),
    JenisIdPembeli VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_JenisIdPembeli  DEFAULT(''),
    NegaraPembeli VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NegaraPembeli  DEFAULT(''),
    NomorDokumenPembeli VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NomorDokumenPembeli  DEFAULT(''),
    
    NamaPembeli VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_NamaPembeli  DEFAULT(''),
    AlamatPembeli VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_AlamatPembeli  DEFAULT(''),
    EmailPembeli VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_EmailPembeli  DEFAULT(''),
    IdTkuPembeli VARCHAR(22) NOT NULL CONSTRAINT DF_BTR_FpKeluaranFaktur_IdTkuPembeli  DEFAULT(''),

    CONSTRAINT PK_BTR_FpKeluaranFaktur PRIMARY KEY (FpKeluaranFakturId),
)
GO

CREATE INDEX IX_BTR_FpKeluaranFaktur_FpKeluaranId ON [dbo].[BTR_FpKeluaranFaktur] (FpKeluaranId, FpKeluaranFakturId)
GO

