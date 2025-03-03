CREATE TABLE BTR_FakturDiscountKlaim
(
    FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturDiscountKlaim_FakturId DEFAULT(''),
    FakturItemId VARCHAR(16) NOT NULL CONSTRAINT DF_BTR_FakturDiscountKlaim_FakturItemId DEFAULT(''),
    FakturDiscountId VARCHAR(18) NOT NULL CONSTRAINT DF_BTR_FakturDiscountKlaim_FakturDiscountId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_FakturDiscountKlaim_NoUrut DEFAULT(0),
    BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTR_FakturDiscountKlaim_BrgId DEFAULT(''),
    DiscProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturDiscountKlaim_DiscountProsen DEFAULT(0),
    DiscRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturDiscountKlaim_DiscountRp DEFAULT(0),
    
    CONSTRAINT PK_BTR_FakturDiscountKlaim PRIMARY KEY CLUSTERED (FakturDiscountId)
)
GO

CREATE INDEX IX_BTR_FakturDiscountKlaim_FakturId
    ON BTR_FakturDiscountKlaim (FakturId, FakturDiscountId)
GO