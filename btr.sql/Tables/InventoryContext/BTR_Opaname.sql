﻿CREATE TABLE BTR_Opname(
    OpnameId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Opname_OpanameId DEFAULT(''),
    OpnameDate DATETIME NOT NULL CONSTRAINT DF_BTR_Opname_OpanameDate DEFAULT(''),
    UserId VARCHAR(50) NOT NULL CONSTRAINT DF_BTR_Opname_UserId DEFAULT(''),
    BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_Opname_BrgId DEFAULT(''),
    BrgCode VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_Opname_BrgCode DEFAULT(''),
    WarehouseId VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_Opname_WarehouseId DEFAULT(''),
    
    Qty2Awal INT NOT NULL CONSTRAINT DF_BTR_Opname_Qty2Awal DEFAULT(0),
    Qty2Opname INT NOT NULL CONSTRAINT DF_BTR_Opname_Qty2Opname DEFAULT(0),
    Qty2Adjust INT NOT NULL CONSTRAINT DF_BTR_Opname_Qty2Adjust DEFAULT(0),
    Satuan2 VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_Opname_Satuan2 DEFAULT(''),

    Qty1Awal INT NOT NULL CONSTRAINT DF_BTR_Opname_Qty1Awal DEFAULT(0),
    Qty1Opname INT NOT NULL CONSTRAINT DF_BTR_Opname_Qty1Opname DEFAULT(0),
    Qty1Adjust INT NOT NULL CONSTRAINT DF_BTR_Opname_Qty1Adjust DEFAULT(0),
    Satuan1 VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_Opname_Satuan1 DEFAULT(''),
    
    Nilai DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_Opname_Nilai DEFAULT(0),                    
)