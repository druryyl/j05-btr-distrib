CREATE TABLE [dbo].[BTR_FakturPajakVoid]
(
	NoFakturPajak VARCHAR(19) NOT NULL CONSTRAINT DF_BTR_FakturPajakVoid_NoFakturPajak DEFAULT(''),
	VoidDate DATETIME NOT NULL CONSTRAINT DF_BTR_FakturPajakVoid_VoidDate DEFAULT('3000-01-01'),
	AlasanVoid VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturPajakVoid_AlasanVoid DEFAULT(''),
	UserId VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_FakturPajakVoid_UserId DEFAULT(''),

	CONSTRAINT PK_FakturPajakVoid_NoFakturPajak PRIMARY KEY CLUSTERED (NoFakturPajak)
)
