﻿CREATE TABLE [dbo].[BTR_HargaType]
(
	HargaTypeId VARCHAR(2) NOT NULL CONSTRAINT DF_BTR_HargaType_HargaTypeId DEFAULT(''),
	HargaTypeName VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_HargaType_HargaTypeName DEFAULT(''),
	Margin DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_HargaType_Margin DEFAULT(0),
	CONSTRAINT PK_BTR_HargaType PRIMARY KEY CLUSTERED(HargaTypeId)
)