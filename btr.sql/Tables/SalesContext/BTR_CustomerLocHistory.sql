CREATE TABLE [dbo].[BTR_CustomerLocHist]
(
	LocHistId VARCHAR(26) NOT NULL CONSTRAINT DF_BTR_CustomerLocHist_LocHistId DEFAULT(''),
	CustomerId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_CustomerLocHist_CustomerId DEFAULT(''),
	ChangeDate DATETIME NOT NULL CONSTRAINT DF_BTR_CustomerLocHist_ChangeDate DEFAULT('3000-01-01'),
	Latitude FLOAT NOT NULL CONSTRAINT DF_BTR_CustomerLocHist_Latitude DEFAULT(0),
	Longitude FLOAT NOT NULL CONSTRAINT DF_BTR_CustomerLocHist_Longitude DEFAULT(0),
	Accuracy FLOAT NOT NULL CONSTRAINT DF_BTR_CustomerLocHist_Accuracy DEFAULT(0),
	ChangeUser VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_CustomerLocHist_ChangeUser DEFAULT(''),

	CONSTRAINT PK_BTR_CustomerLocHist PRIMARY KEY CLUSTERED (LocHistId)
)
GO

CREATE INDEX IX_BTR_CustomerLocHist_CustomerId
	ON BTR_CustomerLocHist(CustomerId, ChangeDate, LocHistId)
	WITH(FILLFACTOR=80)
GO
