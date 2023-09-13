CREATE TABLE [dbo].[BTR_PackingFaktur]
(
	PackingId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PackingFaktur_PackingId DEFAULT(''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTR_PackingFaktur_NoUrut DEFAULT(0),
	FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PackingFaktur_FakturId DEFAULT(''),

	CONSTRAINT PK_BTR_PackingFaktur PRIMARY KEY CLUSTERED(PackingId, NoUrut)
)
GO

CREATE UNIQUE INDEX UX_BTR_PackingFaktur_FakturId
	ON BTR_PackingFaktur (PackingId, FakturId)
	WITH(FILLFACTOR=90)
GO


