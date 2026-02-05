CREATE TABLE [dbo].[BTR_PackingOrder]
(
	PackingOrderId VARCHAR(26) NOT NULL CONSTRAINT DF_BTR_PackingOrder_PackingOrderId DEFAULT (''),
	PackingOrderDate DATETIME NOT NULL CONSTRAINT DF_BTR_PackingOrder_PackingOrderDate DEFAULT ('3000-01-01'),

	CustomerId VARCHAR(26) NOT NULL CONSTRAINT DF_BTR_PackingOrder_CustomerId DEFAULT (''),
	CustomerCode VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_PackingOrder_CustomerCode DEFAULT (''),
	CustomerName VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_PackingOrder_CustomerName DEFAULT (''),
	Alamat VARCHAR(200) NOT NULL CONSTRAINT DF_BTR_PackingOrder_AlamatKirim DEFAULT (''),
	NoTelp VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_PackingOrder_NoTelp DEFAULT (''),
	Latitude DECIMAL(18, 15) NOT NULL CONSTRAINT DF_BTR_PackingOrder_Latitude DEFAULT (0),
	Longitude DECIMAL(18, 15) NOT NULL CONSTRAINT DF_BTR_PackingOrder_Longitude DEFAULT (0),
	Accuracy INT NOT NULL CONSTRAINT DF_BTR_PackingOrder_Accuracy DEFAULT (0),

	FakturId VARCHAR(26) NOT NULL CONSTRAINT DF_BTR_PackingOrder_FakturId DEFAULT (''),
	FakturCode VARCHAR(26) NOT NULL CONSTRAINT DF_BTR_PackingOrder_FakturCode DEFAULT (''),
	FakturDate DATETIME NOT NULL CONSTRAINT DF_BTR_PackingOrder_FakturDate DEFAULT ('3000-01-01'),
	AdminName VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_PackingOrder_AdminName DEFAULT (''),

	CONSTRAINT PK_BTRG_PackingOrder PRIMARY KEY CLUSTERED (PackingOrderId)
)
GO

CREATE INDEX IX_BTR_PackingOrder_FakturId 
	ON [dbo].[BTR_PackingOrder] (FakturId, PackingOrderId)
GO

