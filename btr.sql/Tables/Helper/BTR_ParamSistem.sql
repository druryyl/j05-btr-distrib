CREATE TABLE [dbo].[BTR_ParamSistem]
(
	ParamCode VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_ParamSistem_ParamCode DEFAULT (''),
	ParamValue VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_ParamSistem_ParamValue DEFAULT (''),

	CONSTRAINT PK_BTR_ParamSistem PRIMARY KEY (ParamCode)
)
