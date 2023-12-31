﻿CREATE TABLE [dbo].[BTR_Packing]
(
	PackingId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Packing_PackingId DEFAULT(''),
	PackingDate DATETIME NOT NULL CONSTRAINT DF_BTR_Packing_PackingDate DEFAULT(''),
	
	WarehouseId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Packing_WarehouseId DEFAULT(''),
	DriverId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Packing_DriverId DEFAULT(''),
	DeliveryDate DATETIME NOT NULL CONSTRAINT DF_BTR_Packing_DeliveryDate DEFAULT('3000-01-01'),
	Route VARCHAR(255) NOT NULL CONSTRAINT DF_BTR_Packing_Route DEFAULT(''),

	CONSTRAINT PK_BTR_Packing PRIMARY KEY CLUSTERED (PackingId)
)
GO

CREATE INDEX IX_BTR_Packing_DeliveryDate
	ON BTR_Packing (DeliveryDate, PackingId)
	WITH(FILLFACTOR=90)
GO