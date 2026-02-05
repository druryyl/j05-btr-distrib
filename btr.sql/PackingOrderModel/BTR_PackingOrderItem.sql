CREATE TABLE [dbo].[BTR_PackingOrderItem]
(
	PackingOrderId VARCHAR(26) NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_PackingOrderId DEFAULT (''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_NoUrut DEFAULT (0),
	BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_BrgId DEFAULT (''),
	BrgCode VARCHAR(20) NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_BrgCode DEFAULT (''),
	BrgName VARCHAR(100) NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_BrgName DEFAULT (''),
	QtyBesar INT NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_QtyBesar DEFAULT (0),
	SatBesar VARCHAR(10) NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_SatBesar DEFAULT (''),
	QtyKecil INT NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_QtyKecil DEFAULT (0),
	SatKecil VARCHAR(10) NOT NULL CONSTRAINT DF_BTR__PackingOrderItem_SatKecil DEFAULT (''),

	CONSTRAINT PK_BTRG_PackingOrderItem PRIMARY KEY CLUSTERED (PackingOrderId, NoUrut)
)
