CREATE TABLE BTR_FakturDiscount
(
    FakturId VARCHAR(13) NOT NULL CONSTRAINT DF_BTR_FakturDiscount_FakturId DEFAULT(''),
    FakturItemId VARCHAR(16) NOT NULL CONSTRAINT DF_BTR_FakturDiscount_FakturItemId DEFAULT(''),
    FakturDiscountId VARCHAR(18) NOT NULL CONSTRAINT DF_BTR_FakturDiscount_FakturDiscountId DEFAULT(''),
    NoUrut INT NOT NULL CONSTRAINT DF_BTR_FakturDiscount_NoUrut DEFAULT(0),
    BrgId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_FakturDiscount_BrgId DEFAULT(''),
    DiscountProsen DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturDiscount_DiscountProsen DEFAULT(0),
    DiscountRp DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_FakturDiscount_DiscountRp DEFAULT(0),
    
    CONSTRAINT PK_BTR_FakturDiscount PRIMARY KEY CLUSTERED (FakturDiscountId)
)
GO

CREATE INDEX IX_BTR_FakturDiscount_FakturId
    ON BTR_FakturDiscount (FakturId, FakturDiscountId)
GO