CREATE TABLE BTR_PurchaseOrder(
    PurchaseOrderId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_PurchaseOrder_PurchaseOrderId DEFAULT(''),
    PurchaseOrderDate DATETIME NOT NULL CONSTRAINT DF_BTR_PurchaseOrder_PurchaseOrderDate DEFAULT('3000-01-01'),
    UserId VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_PurchaseOrder_UserId DEFAULT(''),
    SupplierId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_PurchaseOrder_SupplierId DEFAULT(''),
    WarehouseId VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_PurchaseOrder_WarehouseId DEFAULT(''),

    CONSTRAINT PK_BTR_PurchaseOrder PRIMARY KEY CLUSTERED(PurchaseOrderId) 
)
GO

CREATE INDEX IX_BTR_PurchaseOrder_PurchaseOrderDate
    ON BTR_PurchaseOrder (PurchaseOrderDate, PurchaseOrderId)
    WITH(FILLFACTOR = 95)
GO

CREATE INDEX IX_BTR_PurchaseOrder_SupplierId
    ON BTR_PurchaseOrder (SupplierId, PurchaseOrderId)
    WITH(FILLFACTOR = 75)
GO
