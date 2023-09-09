CREATE TABLE [dbo].[BTR_FakturControl]
(
	FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturControl_FakturId DEFAULT(''),
	ControlStatus INT NOT NULL CONSTRAINT DF_BTR_FakturControl_ControlStatus DEFAULT(0),
	ControlDate VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturControl_ControlDate DEFAULT('3000-01-01'),
	Keterangan VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturControl_Keterangan DEFAULT(''),
	UserId VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturControl_UserId DEFAULT(''),

	CONSTRAINT PK_BTR_FakturControl PRIMARY KEY CLUSTERED(FakturId, ControlStatus)
)
