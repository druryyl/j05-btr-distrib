CREATE TABLE [dbo].[BTR_FakturControlStatus]
(
	FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturControl_FakturId DEFAULT(''),
	FakturDate DATETIME NOT NULL CONSTRAINT DF_BTR_FakturControl_FakturDate DEFAULT('3000-01-01'),
	StatusFaktur INT NOT NULL CONSTRAINT DF_BTR_FakturControl_FakturStatus DEFAULT(0),
	StatusDate VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturControl_StatusDate DEFAULT('3000-01-01'),
	Keterangan VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturControl_Keterangan DEFAULT(''),
	UserId VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturControl_UserId DEFAULT(''),

	CONSTRAINT PK_BTR_FakturControlStatus PRIMARY KEY CLUSTERED(FakturId, StatusFaktur)
)
