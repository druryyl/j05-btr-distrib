CREATE TABLE BTR_Customer(
    CustomerId VARCHAR(5) NOT NULL CONSTRAINT DF_BTR_Customer_CustomerId DEFAULT(''),
    CustomerName VARCHAR(30) NOT NULL CONSTRAINT DF_BTR_Customer_CustomerName DEFAULT(''),
    Plafond DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_Customer_Plafond DEFAULT(0),
    CreditBalance DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_Customer_CreditBalance DEFAULT(0),

    Address1 DECIMAL(18,2) NOT NULL CONSTRAINT DF_BTR_Customer_Address1 DEFAULT(''),
    
    CONSTRAINT  PK_BTR_Customer PRIMARY KEY CLUSTERED(CustomerId)
)