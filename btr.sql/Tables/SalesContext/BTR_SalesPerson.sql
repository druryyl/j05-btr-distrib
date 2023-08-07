CREATE TABLE BTR_SalesPerson(
    SalesPersonId VARCHAR(5) NOT NULL CONSTRAINT BTR_SalesPerson_SalesPersonId DEFAULT(''),
    SalesPersonName VARCHAR(30) NOT NULL CONSTRAINT BTR_SalesPerson_SalesPersonName DEFAULT(''),
    
    CONSTRAINT PK_BTR_SalesPerson PRIMARY KEY CLUSTERED(SalesPersonId)
)