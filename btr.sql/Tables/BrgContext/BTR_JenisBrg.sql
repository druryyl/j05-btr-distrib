﻿CREATE TABLE [dbo].[BTR_JenisBrg]
(
	JenisBrgId VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_JenisBrg_JenisBrgId DEFAULT(''),
	JenisBrgName VARCHAR(15) NOT NULL CONSTRAINT DF_BTR_JenisBrg_JenisBrgName DEFAULT(''),

	CONSTRAINT PK_BTR_JenisBrg PRIMARY KEY CLUSTERED(JenisBrgId)
)
