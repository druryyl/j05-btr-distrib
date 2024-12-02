CREATE TABLE [dbo].[BTR_TagihanFaktur]
(
    TagihanId  VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_TagihanId DEFAULT(''),
    NoUrut  INT NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_NoUrut DEFAULT(0),
    FakturId  VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_FakturId DEFAULT(''),
    CustomerId  VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_CustomerId DEFAULT(''),
    NilaiTotal DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_NilaiTotal DEFAULT(0),
    NilaiTerbayar  DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_NilaiTerbayar DEFAULT(0),
    NilaiTagih DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_NilaiTagih DEFAULT(0),

    CONSTRAINT PK_BTR_TagihanFaktur PRIMARY KEY CLUSTERED(TagihanId, NoUrut)
)
