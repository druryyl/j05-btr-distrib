CREATE TABLE [dbo].[BTR_SalesRute]
(
	SalesRuteId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_SalesRute_SalesRuteId DEFAULT(''),
	SalesPersonId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_SalesRute_SalesPersonId DEFAULT(''),
	HariId VARCHAR(1) NOT NULL CONSTRAINT DF_BTR_SalesRute_HariId DEFAULT(''),

	CONSTRAINT PK_BTR_SalesRute PRIMARY KEY CLUSTERED(SalesRuteId)
)
GO

CREATE UNIQUE INDEX IX_BTR_SalesRute_SalesPersonId
	ON [dbo].[BTR_SalesRute](SalesPersonId, HariId)
GO
