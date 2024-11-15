-- create table for ReturJualItemDiscModel
CREATE TABLE BTR_ReturJualItemDisc(
	ReturJualId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_ReturJualId DEFAULT(''),
	ReturJualItemId VARCHAR(16) NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_ReturJualItemId DEFAULT(''),
	ReturJualItemDiscId VARCHAR(18) NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_ReturJualItemDiscId DEFAULT(''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_NoUrut DEFAULT(0),
	BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_BrgId DEFAULT(''),
	
	BaseHrg DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_BaseHrg DEFAULT(0),
	DiscRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_DiscRp DEFAULT(0),
	DiscProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturJualItemDisc_DiscProsen DEFAULT(0),

	CONSTRAINT PK_BTR_ReturJualItemDisc PRIMARY KEY CLUSTERED(ReturJualItemDiscId)
)