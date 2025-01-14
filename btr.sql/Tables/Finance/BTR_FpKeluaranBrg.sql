CREATE TABLE [dbo].[BTR_FpKeluaranBrg]
(
    FpKeluaranBrgId VARCHAR(21) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_FpKeluaranBrgId DEFAULT(''), 
    FpKeluaranFakturId VARCHAR(17) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_FpKeluaranFakturId DEFAULT(''), 
    FpKeluaranId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_FpKeluaranId DEFAULT(''), 
    FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_FakturId DEFAULT(''), 

    Baris INT NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_Baris DEFAULT(0), 
    BarangJasa VARCHAR(1) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_BarangJasa DEFAULT(''),
    KodeBarangJasa VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_KodeBarangJasa DEFAULT(''), 
    NamaBarangJasa VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_NamaBarangJasa DEFAULT(''), 
    NamaSatuanUkur VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_NamaSatuanUkur DEFAULT(''), 
    HargaSatuan DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_HargaSatuan DEFAULT(0), 
    JumlahBarangJasa INT NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_JumlahBarangJasa DEFAULT(0), 
    TotalDiskon DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_TotalDiskon DEFAULT(0), 
    Dpp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_Dpp DEFAULT(0), 
    DppLain DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_DppLain DEFAULT(0), 
    TarifPpn DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_TarifPpn DEFAULT(0), 
    Ppn DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_Ppn DEFAULT(0), 
    TarifPpnBm DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_TarifPpnBm DEFAULT(0), 
    PpnBm DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FpKeluaranBrg_PpnBm DEFAULT(0),

    CONSTRAINT PK_BTR_FpKeluaranBrg PRIMARY KEY CLUSTERED (FpKeluaranBrgId)
)
GO

CREATE INDEX IX_BTR_FpKeluaranBrg_FpKeluaranId
    ON [dbo].[BTR_FpKeluaranBrg] (FpKeluaranId, FpKeluaranBrgId)
GO

