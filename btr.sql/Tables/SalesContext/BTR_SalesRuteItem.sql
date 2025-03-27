CREATE TABLE [dbo].[BTR_SalesRuteItem]
(
	SalesRuteId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_SalesRuteItem_SalesRuteId DEFAULT(''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTR_SalesRuteItem_NoUrut DEFAULT(0),
	CustomerId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_SalesRuteItem_CustomerId DEFAULT(''),

	CONSTRAINT PK_BTR_SalesRuteItem PRIMARY KEY CLUSTERED (SalesRuteId, NoUrut)
)
GO

CREATE UNIQUE INDEX UX_BTR_SalesRuteItem_CustomerId
	ON BTR_SalesRuteItem (SalesRuteId, CustomerId)
GO
