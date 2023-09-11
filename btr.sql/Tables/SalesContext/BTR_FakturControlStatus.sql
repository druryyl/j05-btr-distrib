CREATE TABLE [dbo].[BTR_FakturControlStatus]
(
	FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturControlStatus_FakturId DEFAULT(''),
	FakturDate DATETIME NOT NULL CONSTRAINT DF_BTR_FakturControlStatus_FakturDate DEFAULT('3000-01-01'),
	StatusFaktur INT NOT NULL CONSTRAINT DF_BTR_FakturControlStatus_FakturStatus DEFAULT(0),
	StatusDate DATETIME NOT NULL CONSTRAINT DF_BTR_FakturControlStatus_StatusDate DEFAULT('3000-01-01'),
	Keterangan VARCHAR(255) NOT NULL CONSTRAINT DF_BTR_FakturControlStatus_Keterangan DEFAULT(''),
	UserId VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturControlStatus_UserId DEFAULT(''),

	CONSTRAINT PK_BTR_FakturControlStatus PRIMARY KEY CLUSTERED(FakturId, StatusFaktur)
)
