CREATE TABLE BTR_SalesPerson(
    SalesPersonId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_SalesPerson_SalesPersonId DEFAULT(''),
    SalesPersonCode VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_SalesPerson_SalesPersonCode DEFAULT(''),
    SalesPersonName VARCHAR(30) NOT NULL CONSTRAINT DF_BTR_SalesPerson_SalesPersonName DEFAULT(''),
    WilayahId VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_SalesPerson_WilayahId DEFAULT(''),

    CONSTRAINT PK_BTR_SalesPerson PRIMARY KEY CLUSTERED(SalesPersonId)
)