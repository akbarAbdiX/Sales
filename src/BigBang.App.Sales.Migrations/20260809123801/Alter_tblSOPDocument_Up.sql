IF OBJECT_ID(N'dbo.tblSOPProductType', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblSOPProductType (
        Id INT IDENTITY(1,1) NOT NULL,
        Code NVARCHAR(20) NOT NULL,
        Name NVARCHAR(250) NOT NULL,

        CONSTRAINT PK_tblSOPProductType PRIMARY KEY (Id),
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'UniqueTaxIdentifier'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD UniqueTaxIdentifier nvarchar(10) NULL;
END
GO
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'ProductTypeId'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD ProductTypeId INT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_tblSOPDocument_tblSOPProductType'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD CONSTRAINT FK_tblSOPDocument_tblSOPProductType
        FOREIGN KEY (ProductTypeId)
        REFERENCES dbo.tblSOPProductType (Id);
END

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPPaymentTerm')
      AND name = N'SettlementMethod'
)
BEGIN
    ALTER TABLE dbo.tblSOPPaymentTerm
    ADD SettlementMethod TINYINT NULL;
END
GO
IF OBJECT_ID(N'dbo.tblSOPPaymentVatShare', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblSOPPaymentVatShare
    (
        Id INT IDENTITY(1,1) NOT NULL,
        FromDate DATE NOT NULL,
        ToDate DATE NULL,
        VatShare bigint  NOT NULL,

        CONSTRAINT PK_tblSOPPaymentVatShare PRIMARY KEY (Id)
    );
END
GO

