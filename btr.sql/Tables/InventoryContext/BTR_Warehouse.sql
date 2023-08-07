CREATE TABLE BTR_Warehouse(
    WarehouseId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Warehouse_WarehouseId DEFAULT(''),
    WarehouseName VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_Warehouse_WarehouseName DEFAULT(''),
    
    CONSTRAINT PK_BTR_Warehouse PRIMARY KEY CLUSTERED(WarehouseId)
)