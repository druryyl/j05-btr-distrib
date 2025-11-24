CREATE TABLE [dbo].[BTR_CheckIn]
(
    CheckInId VARCHAR(26) NOT NULL CONSTRAINT DF_BTR_CheckIn_CheckInId DEFAULT(''),
    CheckInDate VARCHAR(10) NOT NULL CONSTRAINT DF_BTR_CheckIn_CheckInDate DEFAULT('3000-01-01'),
    CheckInTime VARCHAR(8) NOT NULL CONSTRAINT DF_BTR_CheckIn_CheckInTime DEFAULT('00:00:00'),
    UserEmail VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_CheckIn_UserEmail DEFAULT(''),
    CheckInLatitude FLOAT NOT NULL CONSTRAINT DF_BTR_CheckIn_CheckInLatitude DEFAULT(0),
    CheckInLongitude FLOAT NOT NULL CONSTRAINT DF_BTR_CheckIn_CheckInLongitude DEFAULT(0),
    Accuracy FLOAT NOT NULL CONSTRAINT DF_BTR_CheckIn_Accuracy DEFAULT(0),
    CustomerId VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_CheckIn_CustomerId DEFAULT(''),
    CustomerCode VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_CheckIn_CustomerCode DEFAULT(''),
    CustomerName VARCHAR(100) NOT NULL CONSTRAINT DF_BTR_CheckIn_CustomerName DEFAULT(''),
    CustomerAddress VARCHAR(200) NOT NULL CONSTRAINT DF_BTR_CheckIn_CustomerAddress DEFAULT(''),
    CustomerLatitude FLOAT NOT NULL CONSTRAINT DF_BTR_CheckIn_CustomerLatitude DEFAULT(0),
    CustomerLongitude FLOAT NOT NULL CONSTRAINT DF_BTR_CheckIn_CustomerLongitude DEFAULT(0),
    StatusSync VARCHAR(20) NOT NULL CONSTRAINT DF_BTR_CheckIn_StatusSync DEFAULT(''),

    CONSTRAINT PK_BTR_CheckIn PRIMARY KEY CLUSTERED (CheckInId ASC)
)

GO

-- Optional: Create indexes for better query performance
--CREATE NONCLUSTERED INDEX IX_BTR_CheckIn_UserEmail 
--ON [dbo].[BTR_CheckIn] (UserEmail)

--CREATE NONCLUSTERED INDEX IX_BTR_CheckIn_CheckInDate 
--ON [dbo].[BTR_CheckIn] (CheckInDate)

--CREATE NONCLUSTERED INDEX IX_BTR_CheckIn_StatusSync 
--ON [dbo].[BTR_CheckIn] (StatusSync)

--CREATE NONCLUSTERED INDEX IX_BTR_CheckIn_CustomerId 
--ON [dbo].[BTR_CheckIn] (CustomerId)

--GO