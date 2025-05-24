﻿CREATE TABLE BTR_ReturBeliDisc
(
    ReturBeliId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_ReturBeliDisc_DiscId DEFAULT(''),
    ReturBeliItemId VARCHAR(17) NOT NULL CONSTRAINT DF_BTR_ReturBeliDisc_DiscItemId DEFAULT(''),
    ReturBeliDiscId VARCHAR(19) NOT NULL CONSTRAINT DF_BTR_ReturBeliDisc_DiscDiscId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_ReturBeliDisc_NoUrut DEFAULT(0),
    BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_ReturBeliDisc_BrgId DEFAULT(''),
    DiscProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturBeliDisc_DiscProsen DEFAULT(0),
    DiscRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_ReturBeliDisc_DiscRp DEFAULT(0),

    CONSTRAINT PK_BTR_ReturBeliDisc PRIMARY KEY CLUSTERED (ReturBeliDiscId)
)
GO

CREATE INDEX IX_BTR_ReturBeliDisc_ReturBeliId
    ON BTR_ReturBeliDisc (ReturBeliId, ReturBeliDiscId)
GO