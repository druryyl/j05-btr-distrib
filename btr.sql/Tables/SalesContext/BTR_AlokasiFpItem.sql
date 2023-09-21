CREATE TABLE [dbo].[BTR_AlokasiFpItem]
(
	AlokasiFpId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_AlokasiFpItem_AlokasiFpId DEFAULT(''),
	NoFakturPajak VARCHAR(19) NOT NULL CONSTRAINT DF_BTR_AlokasiFpItem_NoFakturPajak DEFAULT(''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTR_AlokasiFpItem_NoUrut DEFAULT(0),
	FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_AlokasiFpItem_FakturId DEFAULT(''),
	FakturCode VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_AlokasiFpItem_FakturCode DEFAULT(''),

	CONSTRAINT PK_BTR_AlokasiFpItem PRIMARY KEY CLUSTERED (AlokasiFpId, NoUrut)
)
GO

CREATE UNIQUE INDEX UX_BTR_AlokasiFpItem_FakturId 
	ON BTR_AlokasiFpItem (FakturId)
	WHERE FakturId <> ''
GO

CREATE UNIQUE INDEX UX_BTR_AlokasiFpItem_NoFakturPajak
	ON BTR_AlokasiFpItem (NoFakturPajak)
GO

