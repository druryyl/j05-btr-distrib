﻿CREATE TABLE [dbo].[BTR_Driver]
(
	DriverId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Driver_DriverId DEFAULT(''),
	DriverName VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_Driver_DriverName DEFAULT(''),

	CONSTRAINT PK_BTR_Driver PRIMARY KEY CLUSTERED (DriverId)
)
