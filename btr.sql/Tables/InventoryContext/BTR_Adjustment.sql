-- create table BTR_Adjustment based on AdjustmentModel
CREATE TABLE BTR_Adjustment(
    AdjustmentId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Adjustment_AdjustmentId DEFAULT(''),
    AdjustmentDate DATE NOT NULL CONSTRAINT DF_BTR_Adjustment_AdjustmentDate DEFAULT('3000-01-01'),
    WarehouseId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Adjustment_WarehouseId DEFAULT(''),
    Alasan VARCHAR(255) NOT NULL CONSTRAINT DF_BTR_Adjustment_Alasan DEFAULT(''),
    
    BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_Adjustment_BrgId DEFAULT(''),
    QtyAwalBesar INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAwalBesar DEFAULT(0),
    QtyAwalKecil INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAwalKecil DEFAULT(0),
    QtyAwalInPcs INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAwalInPcs DEFAULT(0),
    
    QtyAdjustBesar INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAdjustBesar DEFAULT(0),
    QtyAdjustKecil INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAdjustKecil DEFAULT(0),
    QtyAdjustInPcs INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAdjustInPcs DEFAULT(0),

    QtyAkhirBesar INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAkhirBesar DEFAULT(0),
    QtyAkhirKecil INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAkhirKecil DEFAULT(0),
    QtyAkhirInPcs INT NOT NULL CONSTRAINT DF_BTR_Adjustment_QtyAkhirInPcs DEFAULT(0),
    
    CONSTRAINT PK_BTR_Adjustment PRIMARY KEY CLUSTERED (AdjustmentId)
)