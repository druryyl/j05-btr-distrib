CREATE TABLE [dbo].[BTR_ReturBalancePost]
(
	ReturJualId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_ReturJualBalancePost_ReturJualId DEFAULT(''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTR_ReturJualBalancePost_NoUrut DEFAULT(0),
	PostDate DATETIME NOT NULl CONSTRAINT DF_BTR_ReturJualBalancePost_PostDate DEFAULT('3000-01-01'),
	UserId VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_ReturJualBalancePost_UserId DEFAULT(''),

	FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_ReturJualBalancePost_FakturId DEFAULT(''),
	IsHeapFaktur BIT NOT NULL CONSTRAINT DF_BTR_ReturBalancePost_IsHeapFaktur DEFAULT(''),
	NilaiFaktur DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturJualBalancePost_NilaiFaktur DEFAULT(0),
	NilaiPotong DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturJualBalancePost_NilaiPotong DEFAULT(0),
	NilaiPost DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturJualBalancePost_NilaiPost DEFAULT(0),

	CONSTRAINT PK_BTR_ReturBalancePost PRIMARY KEY CLUSTERED(ReturJualId, NoUrut)
)
