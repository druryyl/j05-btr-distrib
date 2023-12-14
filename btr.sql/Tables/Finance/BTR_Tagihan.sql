-- create table based on TagihanModel
CREATE TABLE BTR_Tagihan(
	TagihanId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Tagihan_TagihanId DEFAULT(''),
	TagihanDate DATETIME NOT NULL CONSTRAINT DF_BTR_Tagihan_TagihanDate DEFAULT('300-01-01'),
	SalesPersonId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Tagihan_SalesPersonId DEFAULT(''),
	TotalTagihan VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Tagihan_TotalTagihan DEFAULT(0),
	
	CONSTRAINT PK_BTR_Tagihan PRIMARY KEY CLUSTERED(TagihanId)
)
GO
