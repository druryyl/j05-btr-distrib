﻿CREATE TABLE [dbo].[BTR_Wilayah]
(
	WilayahId VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_Wilayah_WilayahId DEFAULT(''),
	WilayahName VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_Wilayah_WilayahName DEFAULT(''),

	CONSTRAINT PK_BTR_Wilayah PRIMARY KEY CLUSTERED (WilayahId)
)
