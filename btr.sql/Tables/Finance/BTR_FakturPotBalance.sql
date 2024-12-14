CREATE TABLE [dbo].[BTR_FakturPotBalance]
(
	FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FAkturPotBalance_FakturId DEFAULT(''),
	IsHeapFaktur BIT NOT NULL CONSTRAINT DF_BTR_FakturPotBalance_IsHeapFaktur DEFAULT(0),
	NilaiFaktur DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FAkturPotBalance_NilaiFaktur DEFAULT(0),
	NilaiPotong DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FAkturPotBalance_NilaiPotoing DEFAULT(0),
	NilaiSumPost DECIMAL(18,2) NOT NULl CONSTRAINT DF_BTR_FAkturPotBalance_NilaiSumPost DEFAULT(0),

	CONSTRAINT PK_BTR_FakturPotBalance PRIMARY KEY CLUSTERED(FakturId)
)
