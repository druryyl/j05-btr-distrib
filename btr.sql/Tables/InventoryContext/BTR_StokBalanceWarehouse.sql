﻿CREATE TABLE [dbo].[BTR_StokBalanceWarehouse]
(
	BrgId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_StokBalance_BrgId DEFAULT(''),
	WarehouseId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_StokBalance_WarehouseId DEFAULT(''),
	Qty INT NOT NULL CONSTRAINT DF_BTR_StokBalance_Qty DEFAULT(0),

	CONSTRAINT PK_BTR_StokBalanceWarehouse PRIMARY KEY CLUSTERED(BrgId, WarehouseId)
)
