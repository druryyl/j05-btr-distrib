CREATE TABLE [dbo].[BTR_PackingOrderDepo]
(
	PackingOrderId VARCHAR(26) NOT NULL CONSTRAINT DF_BTR_PackingOrderDepo_PackingOrderId DEFAULT(''),
	DepoId VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_PackingOrderDepo_DepoId DEFAULT((0)),

	CONSTRAINT PK_BTR_PackingOrderDepo PRIMARY KEY CLUSTERED (PackingOrderId, DepoId)
)
