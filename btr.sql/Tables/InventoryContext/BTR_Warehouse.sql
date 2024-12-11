CREATE TABLE BTR_Warehouse(
    WarehouseId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Warehouse_WarehouseId DEFAULT(''),
    WarehouseName VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_Warehouse_WarehouseName DEFAULT(''),
    IsSpecial BIT NOT NULL CONSTRAINT DF_BTR_Warehouse_IsSpecial DEFAULT(0),
    IsAktif BIT NOT NULL CONSTRAINT DF_BTR_Warehouse_IsAktif DEFAULT(1),

    CONSTRAINT PK_BTR_Warehouse PRIMARY KEY CLUSTERED(WarehouseId)
)
