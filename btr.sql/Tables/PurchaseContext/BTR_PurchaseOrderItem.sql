CREATE TABLE BTR_PurchaseOrderItem(
    PurchaseOrderId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_PurchaseOrderId DEFAULT(''),
    PurchaseOrderItemId VARCHAR(16) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_PurchaseOrderItemId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_NoUrut DEFAULT(0),
    BrgId VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_BrgId DEFAULT(''),
    Qty INT NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_Qty DEFAULT(''),
    Satuan VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_Satuan DEFAULT(''),
    Harga DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_Harga DEFAULT(0),
    SubTotal DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_SubTotal DEFAULT(0),
    DiskonProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_DiskonProsen DEFAULT(0),
    DiskonRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_DiskonRp DEFAULT(0),
    TaxProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_TaxProsen DEFAULT(0),
    TaxRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_TaxRp DEFAULT(0),
    Total DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_PurchaseOrderItem_Total DEFAULT(0),
    
    CONSTRAINT PK_BTR_PurchaseOrderItem PRIMARY KEY CLUSTERED(PurchaseOrderItemId)
)
GO

CREATE INDEX IX_BTR_PurchaseOrderItem_PurchaseOrderId
    ON BTR_PurchaseOrderItem(PurchaseOrderId, PurchaseOrderItemId)
GO