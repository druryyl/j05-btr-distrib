CREATE TABLE [dbo].[BTR_Klasifikasi]
(
	KlasifikasiId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Klasifikasi_KlasifikasiId DEFAULT(''),
	KlasifikasiName VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_Klasifikasi_KlasifikasiName DEFAULT(''),

	CONSTRAINT PK_BTR_Klasifikasi PRIMARY KEY CLUSTERED (KlasifikasiId)
)