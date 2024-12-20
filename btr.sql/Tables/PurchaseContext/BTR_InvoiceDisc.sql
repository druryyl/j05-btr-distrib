﻿CREATE TABLE BTR_InvoiceDisc
(
    InvoiceId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_InvoiceDisc_DiscId DEFAULT(''),
    InvoiceItemId VARCHAR(17) NOT NULL CONSTRAINT DF_BTR_InvoiceDisc_DiscItemId DEFAULT(''),
    InvoiceDiscId VARCHAR(19) NOT NULL CONSTRAINT DF_BTR_InvoiceDisc_DiscDiscId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_InvoiceDisc_NoUrut DEFAULT(0),
    BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_InvoiceDisc_BrgId DEFAULT(''),
    DiscProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_InvoiceDisc_DiscProsen DEFAULT(0),
    DiscRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_InvoiceDisc_DiscRp DEFAULT(0),

    CONSTRAINT PK_BTR_InvoiceDisc PRIMARY KEY CLUSTERED (InvoiceDiscId)
)
GO

CREATE INDEX IX_BTR_InvoiceDisc_InvoiceId
    ON BTR_InvoiceDisc (InvoiceId, InvoiceDiscId)
GO