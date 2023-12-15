CREATE TABLE [dbo].[BTR_TagihanFaktur]
(
    TagihanId  VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_TagihanId DEFAULT(''),
    NoUrut  INT NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_NoUrut DEFAULT(0),
    FakturId  VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_FakturId DEFAULT(''),
    CustomerId  VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_CustomerId DEFAULT(''),
    Nilai  DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_TagihanFaktur_Nilai DEFAULT(''),

    CONSTRAINT PK_BTR_TagihanFaktur PRIMARY KEY CLUSTERED(TagihanId, NoUrut)
)
