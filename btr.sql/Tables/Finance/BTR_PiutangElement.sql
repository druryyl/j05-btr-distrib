CREATE TABLE [dbo].[BTR_PiutangElement]
(
	PiutangId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PiutangElement_PiutangId DEFAULT(''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTR_PiutangELement_NoUrut DEFAULT(0),
    ElementTag INT NOT NULL CONSTRAINT DF_BTR_PiutangElement_ElementTag DEFAULT(0),
	ElementName VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_PiutangElement_ElemantName DEFAULT(''),
	NilaiPlus DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PiutangElement_NilaiPlus DEFAULT(''),
	NilaiMinus DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PiutangElement_NilaiMinus DEFAULT(''),
	ElementDate DATETIME NOT NULL CONSTRAINT DF_BTR_PiutangElement_ElementDate DEFAULT('3000-01-01'),

	CONSTRAINT PK_BTR_PiutangElement PRIMARY KEY CLUSTERED(PiutangId, NoUrut)
)
