﻿CREATE TABLE [dbo].[BTR_Doc]
(
        DocId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Doc_DocId DEFAULT(''),
        DocType VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Doc_DocType DEFAULT(''),
        DocDate DATETIME NOT NULL CONSTRAINT DF_BTR_Doc_DocDate DEFAULT('3000-01-01'),
        WarehouseId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_Doc_WarehouseId DEFAULT(''),
        DocPrintStatus INT NOT NULL CONSTRAINT DF_BTR_Doc_DocPrintStatus DEFAULT(0),

        CONSTRAINT PK_BTR_Doc PRIMARY KEY CLUSTERED(DocId)
)