IF OBJECT_ID('dbo.Customers', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Customers
    (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Customers PRIMARY KEY,
        CustomerNumber NVARCHAR(50) NOT NULL,
        CompanyName NVARCHAR(150) NOT NULL,
        ContactName NVARCHAR(150) NOT NULL CONSTRAINT DF_Customers_ContactName DEFAULT '',
        Email NVARCHAR(200) NOT NULL CONSTRAINT DF_Customers_Email DEFAULT '',
        Phone NVARCHAR(30) NOT NULL CONSTRAINT DF_Customers_Phone DEFAULT '',
        City NVARCHAR(100) NOT NULL CONSTRAINT DF_Customers_City DEFAULT '',
        State NVARCHAR(50) NOT NULL CONSTRAINT DF_Customers_State DEFAULT '',
        CreatedDate DATETIME2 NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Customers_IsActive DEFAULT 1
    );

    CREATE UNIQUE INDEX UX_Customers_CustomerNumber ON dbo.Customers(CustomerNumber);
    CREATE INDEX IX_Customers_CompanyName ON dbo.Customers(CompanyName);
    CREATE INDEX IX_Customers_State ON dbo.Customers(State);
END;
